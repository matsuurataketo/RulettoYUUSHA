using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;
using Unity.VisualScripting;

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
    [SerializeField,Header("タイムラインをアッタッチ")]TimelineManager timelinemanager;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;
    bool scrollSoundPlayed = false; // SEが再生されたかどうかを示すフラグ
    AudioManager audioManager;
    UIListController Uilistcontroller;
    bool scrollWheelEnabled = true;  // マウスホイールの入力を有効/無効にするフラグ
    bool LedyImage = false;
    bool LedyButton = false;
    float UpRouletteRates;
    float DownRouletteRates;


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
    }

    private void Update()
    {
        if (LedyImage == false)
        {
            UpRouletteRates = rMaker.rouletteRates[0];
            DownRouletteRates = rMaker.rouletteRates[1];
            Uilistcontroller.ToggleSpecificImage(2);
            for (int i = 0; i < Uilistcontroller.textElements.Count; i++)
                Uilistcontroller.ToggleSpecificText(i);
            Uilistcontroller.ToggleSpecificImage(3);
            Uilistcontroller.KougekiRoulettoText(0, rMaker.choices[3] + "・・" + rMaker.rouletteRates[0] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(1, rMaker.choices[2] + "・・" + rMaker.rouletteRates[1] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(2, rMaker.choices[1] + "・・" + rMaker.rouletteRates[2] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(3, rMaker.choices[0] + "・・" + rMaker.rouletteRates[3] * 100 + "%");
            LedyImage = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && LedyButton == false)
        {
            Uilistcontroller.ToggleSpecificImage(2);
            LedyButton = true;
        }

        if (enemyScript != null && !enemyScript.IsSpinning&&!rulettogimickflag&& scrollWheelEnabled && LedyButton)
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

        if (rMaker.randomGame == 0&& scrollWheelEnabled)
        {
            //Debug.Log("ボタン連打");
            if (Input.GetKeyDown(KeyCode.Space) && ScrollWheel == true)
            {
                audioManager.PlaySound("連打音");
                rMaker.IncreaseRandomAngle();
                UpRouletteRates += 0.0025f;//割合変更
                DownRouletteRates -= 0.0025f;//割合変更
                Uilistcontroller.KougekiRoulettoText(0, rMaker.choices[3] + "・・" + (UpRouletteRates * 100).ToString("F2") + "%");
                Uilistcontroller.KougekiRoulettoText(1, rMaker.choices[2] + "・・" + (DownRouletteRates * 100).ToString("F2") + "%");
                Debug.Log("拡張してます");
            }
        }
        else if (rMaker.randomGame == 1&& scrollWheelEnabled)
        {
            //Debug.Log("ボタン一撃");
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
                Debug.Log(result);

            }
        }

        switch (result)
        {
            case "強技":
                StartCoroutine(PlayRouletteGame(result, 35, "\n攻撃:"));
                Debug.Log(result);
                break;
            case "弱技":
                StartCoroutine(PlayRouletteGame(result, 15, "\n攻撃:"));
                Debug.Log("じゃく");
                break;
            case "中技":
                StartCoroutine(PlayRouletteGame(result, 25, "\n攻撃:"));
                Debug.Log("中");
                break;
            case "確死":
                StartCoroutine(PlayRouletteGame(result, 100f, "\n攻撃:"));
                Debug.Log("確死");
                break;
            case "おいしい":
                StartCoroutine(PlayRouletteGame(result, 40f, "\n回復:"));
                Debug.Log("おいしい");
                break;
            case "にがい":
                StartCoroutine(PlayRouletteGame(result, 20f, "\n回復:"));
                Debug.Log("にがい");
                break;
            case "極上回復":
                StartCoroutine(PlayRouletteGame(result, 80f, "\n回復:"));
                Debug.Log("極上回復");
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
            for (int i = 0; i < Uilistcontroller.textElements.Count; i++)//Textリストを非表示/表示
                Uilistcontroller.ToggleSpecificText(i);
            Uilistcontroller.ToggleSpecificImage(3);
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

        if (result == "強技" || result == "弱技" || result == "中技" || result == "確死")
        {
            // タイムラインを再生
            timelinemanager.PlayTimeline(2);

            // タイムラインの再生が終了するまで待機
            yield return new WaitUntil(() => timelinemanager.playableDirectors[2].state != PlayState.Playing);

            // HPを減少させる処理
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "おいしい" || result == "にがい" || result == "からい" || result == "極上回復")
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

        // タイムラインを再生
        timelinemanager.PlayTimeline(1);
        // タイムラインの再生が終了するまで待機
        yield return new WaitUntil(() => timelinemanager.playableDirectors[1].state != PlayState.Playing);

        enemyScript.ShowResult(enemyScript.roulette.transform.eulerAngles.z);

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
        LedyButton = false;
        LedyImage = false;
    }
}