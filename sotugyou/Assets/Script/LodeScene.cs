using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField, Header("移動するシーンの名前")] private string SceneName;
    public Image fadeImage; // フェード用のImage
    public float fadeDuration = 1f; // フェード時間
    public bool fadeIn = true;

    // Start is called before the first frame update
    private void Start()
    {
        if (fadeIn == true)
            StartCoroutine(FadeIn());
    }
    public void OnClickGameStart()
    {
        StartCoroutine(FadeOut(SceneName));
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        // エディタで動作を確認する場合
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 実行ファイルで終了する場合
        Application.Quit();
#endif
    }
    public void PushButton()
    {
        Debug.Log("何かおこります");
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
    private IEnumerator FadeOut(string sceneName)
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

        SceneManager.LoadScene(sceneName);
    }
}
