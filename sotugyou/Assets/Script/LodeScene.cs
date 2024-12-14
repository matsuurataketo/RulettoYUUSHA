using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField, Header("�ړ�����V�[���̖��O")] private string SceneName;
    public Image fadeImage; // �t�F�[�h�p��Image
    public float fadeDuration = 1f; // �t�F�[�h����
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
        // �G�f�B�^�œ�����m�F����ꍇ
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ���s�t�@�C���ŏI������ꍇ
        Application.Quit();
#endif
    }
    public void PushButton()
    {
        Debug.Log("����������܂�");
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
