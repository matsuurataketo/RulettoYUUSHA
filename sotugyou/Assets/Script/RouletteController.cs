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
    [HideInInspector] public List<float> rotatePerRouletteStartAngle;
    [HideInInspector] public List<float> rotatePerRouletteEndAngle;
    [HideInInspector] public RouletteMaker rMaker;

    private string result;//ルーレットの結果の格納変数
    [SerializeField,Header("ルーレットの結果表示テキスト")] private TextMeshProUGUI resultText;//結果の表示TEXT
    [Header("ルーレットの回転スピード")] public float rotationSpeed = 5.0f;//ルーレットの回転スピード
    private float lastScrollWheelInputTime; // 最後にマウススクロールホイールの入力があった時間
    private bool ScrollWheel = false;//最初のマウスホイール制御変数
    [SerializeField, Header("ルーレットのリアルな回転速度")] private float rouletteSpeed; // ルーレットの速度を保持する変数
    public float RouletteSpeed => rouletteSpeed; // プロパティを介して外部からアクセスできるようにする
    private Quaternion previousRotation;//ルーレットのｚ回転の変数
    private int frameCount = 0;//回り始めてからのフレームカウンター
    private int comparisonInterval = 60; // 比較間隔

    Slider _slider; //HPバー
    [SerializeField, Header("敵ルーレットの停止矢印")] GameObject EnemyroulettoYazirusi;//
    [SerializeField, Header("自分ルーレットの停止矢印")] GameObject RoulettoYazirusi;//
    [SerializeField , Header("ミニゲームScriptオブジェクト")] GameObject RoulettoGame;
    [SerializeField, Header("ミニゲームに使う画像プレファブ")] GameObject RightLeftImage;
    [SerializeField, Header("SetActivの切り替えを行うオブジェクト")] GameObject[] RoulettoORButton;//スキルルーレット

    UIManager UIManager;
    ScrollSelect ScrollSelect;      
    HPmanegment HPmanegment;
    CountDownTimer countDownTimer;
    ActivScene activScene;

    [SerializeField, Header("敵のルーレットオブジェクト")] GameObject enemyroulette;
    [SerializeField, Header("EnemyRouletteがアタッチされているオブジェクト")] EnemyRoulette enemyScript;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;



    private void Start()
    {
        roulettoGimick = FindObjectOfType<RoulettoGimick>();
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        countDownTimer=GameObject.Find("MiniGameTimer").GetComponent<CountDownTimer>();
        activScene = GameObject.Find("ActiveScene").GetComponent<ActivScene>();

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
        for (int i = 0; i < rMaker.choices.Count; i++)
        {
            if (rotatePerRouletteStartAngle[i] <= x && x < rotatePerRouletteEndAngle[i] ||
                -(360 - rotatePerRouletteStartAngle[i]) >= x && x >= -(360 - rotatePerRouletteEndAngle[i]))
            {
                result = rMaker.choices[i];

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
                //StartCoroutine(PlayRouletteGame(result, 50f, "\n回復:"));
                Debug.Log("おいしい");
                break;
            case "にがい":
                //StartCoroutine(PlayRouletteGame(result, 30f, "\n回復:"));
                Debug.Log("にがい");
                break;
            case "からい":
                //StartCoroutine(PlayRouletteGame(result, 10f, "\n回復:"));
                Debug.Log("からい");
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
            RoulettoYazirusi.SetActive(false);
        }

    }

    private IEnumerator PlayRouletteGame(string result, float hpChange, string message)
    {
        SkillRouletto(message);
        //ミニゲームスタート
        countDownTimer.StartCountDown();
        RoulettoGame.SetActive(true);
        RightLeftImage.SetActive(true);
        rulettogimickflag = true;
        roulettoGimick.StartRouletteGame();
        // 10秒間待機
        yield return new WaitForSeconds(5);
        if (result == "きょう" || result == "じゃく" || result == "みす")
        {
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "おいしい" || result == "にがい" || result == "からい")
        {
            HPmanegment.UpdatePlayerUPHP(hpChange);
        }
        RoulettoGame.SetActive(false);
        RightLeftImage.SetActive(false);
        

        //敵のルーレット開始
        activScene.StartEnemyEffect();
        yield return new WaitUntil(() => activScene.HasCompleted);
        enemyroulette.SetActive(true);
        EnemyroulettoYazirusi.SetActive(true);
        enemyScript.StartRoulette();
        // ルーレットが停止するまで待機
        yield return new WaitUntil(() => !enemyScript.IsSpinning);
        

        // ルーレットが停止した後の処理
        enemyroulette.SetActive(false);
        EnemyroulettoYazirusi.SetActive(false);
        RoulettoORButton[0].SetActive(true);
        RoulettoORButton[1].SetActive(true);

        UIManager.StartCountDown();
        activScene.StartPlayerEffect();
        ScrollSelect.currentTime = 0f;

        RoulettoORButton[2].SetActive(false);
        RoulettoYazirusi.SetActive(false);
        RoulettoORButton[3].SetActive(false);
        rulettogimickflag = false;
    }
}