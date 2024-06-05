using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class HPmanegment : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText;
    [SerializeField] private TextMeshProUGUI EnemyHPText;
    [SerializeField] private TextMeshProUGUI WeponText;
    public float PlayerHP = 100; // �v���C���[��HP��������
    public float EnemyHP = 100; // �G�l�~�[��HP��������
    public float PlayerAtack = 30;
    public float Weponstate=0;

    // Start is called before the first frame update
    void Start()
    {
        switch (WeponRouletto.result) {
            case "ken":
                Weponstate = 20;
                WeponText.text = "�Ղꂢ��[����:" + WeponRouletto.result;
                break;
            case "yari":
                Weponstate = 10;
                WeponText.text = "�Ղꂢ��[����:" + WeponRouletto.result;
                break;
        }

        // HP�̏����l��UI�ɔ��f
        UpdateUI();
    }

    public void UpdatePlayerDownHP(int newHP)
    {
        PlayerHP -= newHP;
        UpdateUI();
    }
    public void UpdatePlayerUPHP(int newHP)
    {
        PlayerHP += newHP;
        UpdateUI();
    }

    // �G�l�~�[��HP���X�V���郁�\�b�h
    public void UpdateEnemyDownHP(float newHP)
    {
        EnemyHP -= (30 + Weponstate) * newHP;
        UpdateUI();
    }

    // �G�l�~�[��HP���X�V���郁�\�b�h
    public void UpdateEnemyUPHP(int newHP)
    {
        EnemyHP += newHP;
        UpdateUI();
    }

    // UI��HP�̒l�𔽉f���郁�\�b�h
    void UpdateUI()
    {
        if (PlayerHP <= 0)
        {
            PlayerHP = 0;
            PlayerHPText.text = "�Ղꂢ��[: " + PlayerHP.ToString();
            StartCoroutine(Weit("EndScene"));
        }
        else if (EnemyHP <= 0)
        {
            EnemyHP = 0;
            EnemyHPText.text = "�Ă�: " + EnemyHP.ToString();
            StartCoroutine(Weit("CrearScene"));
        }
        else
        {
            PlayerHPText.text = "�Ղꂢ��[: " + PlayerHP.ToString();
            EnemyHPText.text = "�Ă�: " + EnemyHP.ToString();
        }
    }

    IEnumerator Weit(string sceneName)
    {
        Time.timeScale = 0; // �Q�[�����ꎞ��~
        yield return new WaitForSecondsRealtime(1.0f);
        Time.timeScale = 1; // �Q�[�����ĊJ
        SceneManager.LoadScene(sceneName);
    }
}
