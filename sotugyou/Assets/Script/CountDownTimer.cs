using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField,Header("�^�C�}�[�摜")] private Image uiFill;
    [SerializeField,Header("�^�C�}�[�e�L�X�g")] private TextMeshProUGUI uiText;
    [SerializeField, Header("���Ԑ���")] private float CountTime;

    private void Start()
    {
     
    }

    public void StartCountDown()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlaySound("�{�^���I���J�E���g�_�E���^�C�}�[");
        uiText.gameObject.SetActive(true);
        uiFill.gameObject.SetActive(true);
        float timer = CountTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            uiFill.fillAmount = Mathf.InverseLerp(0, CountTime, timer);
            uiText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            yield return null;
        }

        // �^�C�}�[��0�ɂȂ������̏����������ɒǉ��ł��܂��B
        audioManager.StopSound("�{�^���I���J�E���g�_�E���^�C�}�[");
        uiFill.fillAmount = 0;
        uiText.text = "00:00";
        uiText.gameObject.SetActive(false);
        uiFill.gameObject.SetActive(false);
    }
}