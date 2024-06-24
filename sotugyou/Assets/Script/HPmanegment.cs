using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class HPmanegment : MonoBehaviour
{
    [SerializeField, Header("プレイヤーHP表示テキスト")] private TextMeshProUGUI PlayerHPText;
    [SerializeField,Header("敵HP表示テキスト")] private TextMeshProUGUI EnemyHPText;
    [SerializeField, Header("装備表示テキスト")] private TextMeshProUGUI WeponText;
    [Header("プレイヤーHP")] public float PlayerHP = 100; // プレイヤーのHPを初期化
    [Header("敵HP")] public float EnemyHP = 100; // エネミーのHPを初期化
    [Header("プレイヤーの基礎攻撃力")] public float PlayerAtack = 30;
    [Header("武器の持つ攻撃力")] public float Weponstate=0;

    // Start is called before the first frame update
    void Start()
    {
        switch (WeponRouletto.result) {
            case "ken":
                Weponstate = 20;
                WeponText.text = "ぷれいやー武器:" + WeponRouletto.result;
                break;
            case "yari":
                Weponstate = 10;
                WeponText.text = "ぷれいやー武器:" + WeponRouletto.result;
                break;
        }

        // HPの初期値をUIに反映
        UpdateUI();
    }

    public void UpdatePlayerDownHP(float newHP)
    {
        PlayerHP -= newHP;
        UpdateUI();
    }
    public void UpdatePlayerUPHP(float newHP)
    {
        PlayerHP += newHP;
        UpdateUI();
    }

    // エネミーのHPを更新するメソッド
    public void UpdateEnemyDownHP(float newHP)
    {
        EnemyHP -= (30 + Weponstate) * newHP;
        UpdateUI();
    }

    // エネミーのHPを更新するメソッド
    public void UpdateEnemyUPHP(float newHP)
    {
        EnemyHP += newHP;
        UpdateUI();
    }

    // UIにHPの値を反映するメソッド
    void UpdateUI()
    {
        if (PlayerHP <= 0)
        {
            PlayerHP = 0;
            PlayerHPText.text = "ぷれいやー: " + PlayerHP.ToString();
            StartCoroutine(Weit("EndScene"));
        }
        else if (EnemyHP <= 0)
        {
            EnemyHP = 0;
            EnemyHPText.text = "てき: " + EnemyHP.ToString();
            StartCoroutine(Weit("CrearScene"));
        }
        else
        {
            PlayerHPText.text = "ぷれいやー: " + PlayerHP.ToString();
            EnemyHPText.text = "てき: " + EnemyHP.ToString();
        }
    }

    IEnumerator Weit(string sceneName)
    {
        Time.timeScale = 0; // ゲームを一時停止
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1; // ゲームを再開
        SceneManager.LoadScene(sceneName);
    }
}
