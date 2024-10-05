using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;

public class RouletteController : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public List<float> rotatePerRouletteStartAngle;
    [HideInInspector] public List<float> rotatePerRouletteEndAngle;
    [HideInInspector] public RouletteMaker rMaker;

    private string result;//ルーレットの結果の格納変数
    [SerializeField,Header("ルーレットの結果表示テキスト")] private TextMeshProUGUI resultText;//結果の表示TEXT
    [Header("ルーレットの回転スピード")] public float rotationSpeed = 7.0f;//ルーレットの回転スピード
    private float lastScrollWheelInputTime; // 最後にマウススクロールホイールの入力があった時間
    public bool ScrollWheel = false;//最初のマウスホイール制御変数
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
    [SerializeField,Header("タイムラインをアッタッチ")]TimelineController timelineController;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;
    bool scrollSoundPlayed = false; // SEが再生されたかどうかを示すフラグ
    AudioManager audioManager;
    UIListController Uilistcontroller;
    bool scrollWheelEnabled = true;  // マウスホイールの入力を有効/無効にするフラグ


    private void Start()
    {
        roulettoGimick = FindObjectOfType<RoulettoGimick>();
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        countDownTimer=GameObject.Find("MiniGameTimer").GetComponent<CountDownTimer>();
        activScene = GameObject.Find("ActiveScene").GetComponent<ActivScene>();
        audioManager = FindObjectOfType<AudioManager>();
        Uilistcontroller = FindObjectOfType<UIListController>();
        //_slider = GameObject.Find("EnemyHP").GetComponent<Slider>();
        //_slider.value = 1f;
    }

    private void Update()
    {
        if (enemyScript != null && !enemyScript.IsSpinning&&!rulettogimickflag&& scrollWheelEnabled)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // マウスホイールが回された場合
            {
                if (!scrollSoundPlayed) // SEがまだ再生されていない場合
                {
                    audioManager.PlaySound("ルーレットSE");
                    scrollSoundPlayed = true;
                }
                   
                ScrollWheel = true; // フラグを下ろして、以降の処理を実行可能にする
            }

            rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ルーレットの速度を更新する
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);
        }

        float tolerance = 0.1f; // Adjust this value based on the precision you need

        frameCount++;

        if (rMaker.randomGame == 0)
        {
            Debug.Log("ボタン連打");
            if (Input.GetKeyDown(KeyCode.Space) && ScrollWheel == true)
            {
                audioManager.PlaySound("連打音");
                rMaker.IncreaseRandomAngle();
                Debug.Log("拡張してます");
            }
        }
        else if (rMaker.randomGame == 1)
        {
            Debug.Log("ボタン一撃");
            rotationSpeed = 21f;
            if (Input.GetKey(KeyCode.Space) && ScrollWheel == true)
            {
                Debug.Log("止めています");
                rotationSpeed = 0f;
                scrollWheelEnabled = false;
            }
        }

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            
            float angleDifference = Quaternion.Angle(roulette.transform.rotation, previousRotation);
            if (angleDifference < tolerance && ScrollWheel == true)
            {
                ScrollWheel = false;
                scrollSoundPlayed = false;
                scrollWheelEnabled = false;
                audioManager.StopSound("ルーレットSE");
                Debug.Log("回転は同じです。");
                ShowResult(roulette.transform.eulerAngles.z);
                rotationSpeed = 5.0f;
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
                StartCoroutine(PlayRouletteGame(result, 45, "\n攻撃:"));
                Debug.Log(result);
                break;
            case "じゃく":
                StartCoroutine(PlayRouletteGame(result, 15, "\n攻撃:"));
                Debug.Log("じゃく");
                break;
            case "中":
                StartCoroutine(PlayRouletteGame(result, 30, "\n攻撃:"));
                Debug.Log("中");
                break;
            case "確死":
                StartCoroutine(PlayRouletteGame(result, 100f, "\n攻撃:"));
                Debug.Log("確死");
                break;
            case "おいしい":
                StartCoroutine(PlayRouletteGame(result, 50f, "\n回復:"));
                Debug.Log("おいしい");
                break;
            case "にがい":
                StartCoroutine(PlayRouletteGame(result, 30f, "\n回復:"));
                Debug.Log("にがい");
                break;
            case "からい":
                StartCoroutine(PlayRouletteGame(result, 10f, "\n回復:"));
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

            if (rMaker.randomGame == 1)
                Uilistcontroller.ToggleSpecificImage(0);  // Imageリストの最初の要素を表示/非表示にする
            if (rMaker.randomGame == 0)
                Uilistcontroller.ToggleSpecificImage(1);  // Imageリストの最初の要素を表示/非表示にする
        }

    }

    private IEnumerator PlayRouletteGame(string result, float hpChange, string message)
    {
        yield return new WaitForSeconds(1f);
        SkillRouletto(message);
        //ミニゲームスタート
        countDownTimer.StartCountDown();
        RoulettoGame.SetActive(true);
        RightLeftImage.SetActive(true);
        rulettogimickflag = true;
        roulettoGimick.StartRouletteGame();

        // 10秒間待機
        yield return new WaitForSeconds(5); 

        RoulettoGame.SetActive(false);
        RightLeftImage.SetActive(false);

        if (result == "きょう" || result == "じゃく" || result == "中"||result=="確死")
        {
            // タイムラインを再生
            timelineController.PlayTimeline();

            // タイムラインの再生が終了するまで待機
            yield return new WaitUntil(() => timelineController.playableDirector.state != PlayState.Playing);

            // HPを減少させる処理
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "おいしい" || result == "にがい" || result == "からい")
        {
            HPmanegment.UpdatePlayerUPHP(hpChange);
        }

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
        rotationSpeed = 5.0f;
        scrollWheelEnabled = true;
    }
}