using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // �X���C�_�[�p
using System.Collections;
using UnityEngine.Playables;

public class HPmanegment : MonoBehaviour
{
    [SerializeField, Header("�v���C���[HP�\���e�L�X�g")] private TextMeshProUGUI PlayerHPText;
    [SerializeField, Header("�GHP�\���e�L�X�g")] private TextMeshProUGUI EnemyHPText;
    [SerializeField, Header("�����\���e�L�X�g")] private TextMeshProUGUI WeponText;
    [SerializeField, Header("�v���C���[HP�X���C�_�[")] private Slider PlayerHPSlider;
    [SerializeField, Header("�GHP�X���C�_�[")] private Slider EnemyHPSlider;
    [SerializeField, Header("�v���C���[HP�X���C�_�[��Fill Image")] private Image PlayerHPFillImage;
    [SerializeField, Header("�GHP�X���C�_�[��Fill Image")] private Image EnemyHPFillImage;


    [Header("�v���C���[HP")] public float PlayerHP = 100;
    [Header("�GHP")] public float EnemyHP = 100;
    [Header("�v���C���[�̊�b�U����")] public float PlayerAtack = 30;
    [Header("����̎��U����")] public float Weponstate = 0;
    [SerializeField, Header("HP����/�����ɂ����鎞��(�b)")] private float damageDuration = 1.0f; // HP�����E�����ɂ����鎞��

    private Color greenColor = Color.green;
    private Color yellowColor = Color.yellow;
    private Color orangeColor = new Color(1f, 0.64f, 0f); // �I�����W
    private Color redColor = Color.red;
    public RoulettoGimick roulettoGimick;

    void Start()
    {
        switch (WeponRouletto.result)
        {
            case "ken":
                Weponstate = 20;
                WeponText.text = "�Ղꂢ��[����:" + WeponRouletto.result;
                break;
            case "yari":
                Weponstate = 10;
                WeponText.text = "�Ղꂢ��[����:" + WeponRouletto.result;
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
            audioManager.PlaySound("HP�_�E���̉�");
        StartCoroutine(SmoothHPChange(PlayerHPSlider, PlayerHPText, PlayerHP, PlayerHP - newHP));
        PlayerHP = Mathf.Max(0, PlayerHP - newHP);  // HP���X�V
    }

    public void UpdatePlayerUPHP(float newHP)
    {
        float Crearnum = 1 + (roulettoGimick.CrearNum / 10);
        float totalHeal = newHP * Crearnum;
        StartCoroutine(SmoothHPChange(PlayerHPSlider, PlayerHPText, PlayerHP, PlayerHP + totalHeal));
        PlayerHP = Mathf.Min(PlayerHP + totalHeal, PlayerHPSlider.maxValue);  // HP���ő�l�𒴂��Ȃ��悤��
    }

    public void UpdateEnemyDownHP(float newHP)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlaySound("HP�_�E���̉�");
        float Crearnum = 1+(roulettoGimick.CrearNum/10);
        float totalDamage = newHP * Crearnum;
        StartCoroutine(SmoothHPChange(EnemyHPSlider, EnemyHPText, EnemyHP, EnemyHP - totalDamage));
        EnemyHP = Mathf.Max(0, EnemyHP - totalDamage);  // HP���X�V
    }

    public void UpdateEnemyUPHP(float newHP)
    {
        StartCoroutine(SmoothHPChange(EnemyHPSlider, EnemyHPText, EnemyHP, EnemyHP + newHP));
        EnemyHP = Mathf.Min(EnemyHP + newHP, EnemyHPSlider.maxValue);  // HP���ő�l�𒴂��Ȃ��悤��
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

        UpdateUI(); // �F�̍X�V������
    }

    void  UpdateUI()
    {
        // �v���C���[HP�X���C�_�[�̍X�V
        PlayerHPText.text = "�Ղꂢ��[: " + PlayerHP.ToString();
        PlayerHPSlider.value = PlayerHP;
        UpdateHPBarColor(PlayerHPSlider, PlayerHPFillImage, PlayerHP);

        // �GHP�X���C�_�[�̍X�V
        EnemyHPText.text = "�Ă�: " + EnemyHP.ToString();
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