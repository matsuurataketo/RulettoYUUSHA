using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WeponRouletto : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public WeponRouletteMaker rMaker;

    public static string result;//ルーレットの結果の格納変数
    [SerializeField] private TextMeshProUGUI resultText;//結果の表示TEXT
    public float rotationSpeed = 5.0f;//ルーレットの回転スピード
    private float lastScrollWheelInputTime; // 最後にマウススクロールホイールの入力があった時間
    public float stopThreshold = 1.0f; // ルーレットが停止したとみなす閾値（秒
    private bool ScrollWheel = false;//最初のマウスホイール制御変数
    [SerializeField] private float rouletteSpeed; // ルーレットの速度を保持する変数
    public float RouletteSpeed => rouletteSpeed; // プロパティを介して外部からアクセスできるようにする
    private Quaternion previousRotation;//ルーレットのｚ回転の変数
    private int frameCount = 0;//回り始めてからのフレームカウンター
    private int comparisonInterval = 210; // 比較間隔
    [SerializeField] GameObject WponsRoulrtto;//装備決めのシーンで使用

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // マウスホイールが回された場合
            ScrollWheel = true; // フラグを下ろして、以降の処理を実行可能にする

        rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ルーレットの速度を更新する
        roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            if (Quaternion.Angle(roulette.transform.rotation, previousRotation) == 0f && ScrollWheel == true)
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

        frameCount++;
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
            //武器選択ルーレット
            case "ken":
                //武器が剣のとき
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                Weit();
                SceneManager.LoadScene("MaingameScene");
                break;
            case "yari":
                //武器が槍のとき
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                Weit();
                SceneManager.LoadScene("MaingameScene");
                break;
            case "kobusi":
                //武器が拳のとき
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                break;
            case "tue":
                //武器が杖のとき
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                break;

            default:
                break;
        }
    }

    private void ShowWeponRouletto(bool activ, bool notactiv, string HitText)
    {
        resultText.text = resultText.text + HitText + result + "的中！";
    }
    IEnumerator Weit()
    {
        yield return new WaitForSeconds(5.0f);
        //遅らせたい処理
    }
}
