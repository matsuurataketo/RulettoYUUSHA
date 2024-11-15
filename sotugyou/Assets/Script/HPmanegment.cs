using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // スライダー用
using System.Collections;
using UnityEngine.Playables;

public class HPmanegment : MonoBehaviour
{
    [SerializeField, Header("プレイヤーHP表示テキスト")] private TextMeshProUGUI PlayerHPText;
    [SerializeField, Header("敵HP表示テキスト")] private TextMeshProUGUI EnemyHPText;
    [SerializeField, Header("装備表示テキスト")] private TextMeshProUGUI WeponText;
    [SerializeField, Header("プレイヤーHPスライダー")] private Slider PlayerHPSlider;
    [SerializeField, Header("敵HPスライダー")] private Slider EnemyHPSlider;
    [SerializeField, Header("プレイヤーHPスライダーのFill Image")] private Image PlayerHPFillImage;
    [SerializeField, Header("敵HPスライダーのFill Image")] private Image EnemyHPFillImage;


    [Header("プレイヤーHP")] public float PlayerHP = 100;
    [Header("敵HP")] public float EnemyHP = 100;
    [Header("プレイヤーの基礎攻撃力")] public float PlayerAtack = 30;
    [Header("武器の持つ攻撃力")] public float Weponstate = 0;
    [SerializeField, Header("HP減少/増加にかける時間(秒)")] private float damageDuration = 1.0f; // HP減少・増加にかける時間

    private Color greenColor = Color.green;
    private Color yellowColor = Color.yellow;
    private Color orangeColor = new Color(1f, 0.64f, 0f); // オレンジ
    private Color redColor = Color.red;
    public RoulettoGimick roulettoGimick;

    void Start()
    {
        switch (WeponRouletto.result)
        {
            case "ken":
                Weponstate = 20;
                WeponText.text = "ぷれいやー武器:" + WeponRouletto.result;
                break;
            case "yari":
                Weponstate = 10;
                WeponText.text = "ぷれいやー武器:" + WeponRouletto.result;
                break;
        }

        PlayerHPSlider.maxValue = PlayerHP;
        EnemyHPSlider.maxValue = EnemyHP;

        UpdateUI();
    }

    public void UpdatePlayerDownHP(float newHP)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (newHP > 0)
            audioManager.PlaySound("HPダウンの音");
        StartCoroutine(SmoothHPChange(PlayerHPSlider, PlayerHPText, PlayerHP, PlayerHP - newHP));
        PlayerHP = Mathf.Max(0, PlayerHP - newHP);  // HPを更新
    }

    public void UpdatePlayerUPHP(float newHP)
    {
        float Crearnum = 1 + (roulettoGimick.CrearNum / 10);
        float totalHeal = newHP * Crearnum;
        StartCoroutine(SmoothHPChange(PlayerHPSlider, PlayerHPText, PlayerHP, PlayerHP + totalHeal));
        PlayerHP = Mathf.Min(PlayerHP + totalHeal, PlayerHPSlider.maxValue);  // HPが最大値を超えないように
    }

    public void UpdateEnemyDownHP(float newHP)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlaySound("HPダウンの音");
        float Crearnum = 1+(roulettoGimick.CrearNum/10);
        float totalDamage = newHP * Crearnum;
        StartCoroutine(SmoothHPChange(EnemyHPSlider, EnemyHPText, EnemyHP, EnemyHP - totalDamage));
        EnemyHP = Mathf.Max(0, EnemyHP - totalDamage);  // HPを更新
    }

    public void UpdateEnemyUPHP(float newHP)
    {
        StartCoroutine(SmoothHPChange(EnemyHPSlider, EnemyHPText, EnemyHP, EnemyHP + newHP));
        EnemyHP = Mathf.Min(EnemyHP + newHP, EnemyHPSlider.maxValue);  // HPが最大値を超えないように
    }

    IEnumerator SmoothHPChange(Slider hpSlider, TextMeshProUGUI hpText, float startHP, float endHP)
    {
        float elapsedTime = 0f;
        endHP = Mathf.Clamp(endHP, 0, hpSlider.maxValue);

        while (elapsedTime < damageDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentHP = Mathf.Lerp(startHP, endHP, elapsedTime / damageDuration);
            hpSlider.value = currentHP;
            hpText.text = currentHP.ToString("F0");
            yield return null;
        }

        hpSlider.value = endHP;
        hpText.text = endHP.ToString("F0");

        UpdateUI(); // 色の更新もする
    }

    void  UpdateUI()
    {
        // プレイヤーHPスライダーの更新
        PlayerHPText.text = "ぷれいやー: " + PlayerHP.ToString();
        PlayerHPSlider.value = PlayerHP;
        UpdateHPBarColor(PlayerHPSlider, PlayerHPFillImage, PlayerHP);

        // 敵HPスライダーの更新
        EnemyHPText.text = "てき: " + EnemyHP.ToString();
        EnemyHPSlider.value = EnemyHP;
        UpdateHPBarColor(EnemyHPSlider, EnemyHPFillImage, EnemyHP);
    }

    void UpdateHPBarColor(Slider slider, Image fillImage, float hp)
    {
        if (hp > 70)
        {
            fillImage.color = greenColor;
        }
        else if (hp > 40)
        {
            fillImage.color = yellowColor;
        }
        else if (hp > 20)
        {
            fillImage.color = orangeColor;
        }
        else
        {
            fillImage.color = redColor;
        }
        
    }
}