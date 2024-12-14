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
    HPmanegment hPmanegment;

    private void Start()
    {
        hPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        StartCoroutine(FadeIn());
    }
    private void Update()
    {
        if (hPmanegment.EnemyHP <= 0)
            SwitchCameraAndScene("CrearScene");
    }

    public void SwitchCameraAndScene(string sceneName)
    {
        StartCoroutine(FadeOutAndSwitchCamera(sceneName));
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

    private IEnumerator FadeOutAndSwitchCamera(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;

        // シーンをロードする
        SceneManager.LoadScene(sceneName);
    }
}
