using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // フェード用のImage
    public float fadeDuration = 1f; // フェード時間
    public float waitDurationVictory = 2f; // 勝利時の待機時間
    public float waitDurationDefeat = 1.5f; // 敗北時の待機時間
    HPmanegment hPmanegment;

    private void Start()
    {
        hPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (hPmanegment.EnemyHP <= 0)
        {
            StartCoroutine(FadeAndSwitchScene("CrearScene", waitDurationVictory));
        }
        else if (hPmanegment.PlayerHP <= 0)
        {
            StartCoroutine(FadeAndSwitchScene("EndScene", waitDurationDefeat));
        }
    }

    private IEnumerator FadeAndSwitchScene(string sceneName,float waitDuration)
    {
        // フェードアウトを1.5秒間かけて実行
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        for (float t = 0; t < fadeDuration + waitDuration; t += Time.deltaTime)
        {
            if (t >= waitDuration) // 待機時間が過ぎたらフェードアウト開始
            {
                color.a = (t - waitDuration) / fadeDuration;
                fadeImage.color = color;
            }
            yield return null;
        }

        // 完全にフェードアウトした後にシーンをロード
        color.a = 1f;
        fadeImage.color = color;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = 1f - t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }
}
