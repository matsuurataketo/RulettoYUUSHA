using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField,Header("タイマー画像")] private Image uiFill;
    [SerializeField,Header("タイマーテキスト")] private TextMeshProUGUI uiText;
    [SerializeField, Header("時間制限")] private float CountTime;

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
        audioManager.PlaySound("ボタン選択カウントダウンタイマー");
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

        // タイマーが0になった時の処理をここに追加できます。
        audioManager.StopSound("ボタン選択カウントダウンタイマー");
        uiFill.fillAmount = 0;
        uiText.text = "00:00";
        uiText.gameObject.SetActive(false);
        uiFill.gameObject.SetActive(false);
    }
}