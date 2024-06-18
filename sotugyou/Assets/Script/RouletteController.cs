using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RouletteController : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public RouletteMaker rMaker;

    private string result;//ルーレットの結果の格納変数
    [SerializeField] private TextMeshProUGUI resultText;//結果の表示TEXT
    public float rotationSpeed = 5.0f;//ルーレットの回転スピード
    private float lastScrollWheelInputTime; // 最後にマウススクロールホイールの入力があった時間
    public float stopThreshold = 1.0f; // ルーレットが停止したとみなす閾値（秒
    private bool ScrollWheel = false;//最初のマウスホイール制御変数
    [SerializeField] private float rouletteSpeed; // ルーレットの速度を保持する変数
    public float RouletteSpeed => rouletteSpeed; // プロパティを介して外部からアクセスできるようにする
    private Quaternion previousRotation;//ルーレットのｚ回転の変数
    private int frameCount = 0;//回り始めてからのフレームカウンター
    private int comparisonInterval = 60; // 比較間隔

    Slider _slider; //HPバー
    [SerializeField] GameObject shieldRoulettoObject;//装備決めのシーンで使用
    [SerializeField] GameObject WponsRoulrtto;//装備決めのシーンで使用
    [SerializeField] GameObject[] RoulettoORButton;//スキルルーレット
    UIManager UIManager;
    ScrollSelect ScrollSelect;
    HPmanegment HPmanegment;

    [SerializeField] GameObject enemyroulette;
    [SerializeField] EnemyRoulette enemyScript;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;



    private void Start()
    {
        roulettoGimick = FindObjectOfType<RoulettoGimick>();
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        //_slider = GameObject.Find("EnemyHP").GetComponent<Slider>();
        //_slider.value = 1f;
    }

    private void Update()
    {
        if (enemyScript != null && !enemyScript.IsSpinning&&!rulettogimickflag)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // マウスホイールが回された場合
                ScrollWheel = true; // フラグを下ろして、以降の処理を実行可能にする

            rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ルーレットの速度を更新する
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);
        }

        float tolerance = 0.1f; // Adjust this value based on the precision you need

        frameCount++;

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            float angleDifference = Quaternion.Angle(roulette.transform.rotation, previousRotation);
            if (angleDifference < tolerance && ScrollWheel == true)
            {
                ScrollWheel = false;
                Debug.Log("回転は同じです。");
                ShowResult(roulette.transform.eulerAngles.z);
            }
            else
            {
                previousRotation = roulette.transform.rotation;
            }
        }
    }

    private void ShowResult(float x)
    {
        //resultText.text = "Wepon:";
        for (int i = 1; i <= rMaker.choices.Count; i++)
        {
            if (((rotatePerRoulette * (i - 1) <= x) && x <= (rotatePerRoulette * i)) ||
                (-(360 - ((i - 1) * rotatePerRoulette)) >= x && x >= -(360 - (i * rotatePerRoulette))))
            {
                result = rMaker.choices[i - 1];
            }
        }

        switch (result)
        {
            case "きょう":
                StartCoroutine(PlayRouletteGame(result, 1f, "\n攻撃:"));
                break;
            case "じゃく":
                StartCoroutine(PlayRouletteGame(result, 0.5f, "\n攻撃:"));
                break;
            case "みす":
                StartCoroutine(PlayRouletteGame(result, 0f, "\n攻撃:"));
                break;
            case "おいしい":
                StartCoroutine(PlayRouletteGame(result, 50f, "\n回復:"));
                break;
            case "にがい":
                StartCoroutine(PlayRouletteGame(result, 30f, "\n回復:"));
                break;
            case "からい":
                StartCoroutine(PlayRouletteGame(result, 10f, "\n回復:"));
                break;
            default:
                break;
        }
    }

    private void SkillRouletto(string HitText)
    {
        resultText.text = "";
        resultText.text = resultText.text + HitText + result + "的中！";

        //ルーレットのimageを非表示にしてる
        {
            // 親オブジェクトを取得する
            GameObject parentObject = RoulettoORButton[2];

            // 親オブジェクトのすべての子供をループで取得して非アクティブにする
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        RoulettoORButton[3].SetActive(false);
    }

    private IEnumerator PlayRouletteGame(string result, float hpChange, string message)
    {
        SkillRouletto(message);
        //ミニゲームスタート
        rulettogimickflag = true;
        roulettoGimick.StartRouletteGame();
        // 10秒間待機
        yield return new WaitForSeconds(10);
        if (result == "きょう" || result == "じゃく" || result == "みす")
        {
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "おいしい" || result == "にがい" || result == "からい")
        {
            HPmanegment.UpdatePlayerUPHP(hpChange);
        }
        rulettogimickflag = false;

        //敵のルーレット開始
        enemyroulette.SetActive(true);
        enemyScript.StartRoulette();
        // ルーレットが停止するまで待機
        yield return new WaitUntil(() => !enemyScript.IsSpinning);
        

        // ルーレットが停止した後の処理
        enemyroulette.SetActive(false);
        RoulettoORButton[0].SetActive(true);
        RoulettoORButton[1].SetActive(true);

        UIManager.StartCountDown();
        ScrollSelect.currentTime = 0f;

        RoulettoORButton[2].SetActive(false);
        RoulettoORButton[4].SetActive(false);
    }
}