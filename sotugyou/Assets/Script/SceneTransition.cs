using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // �t�F�[�h�p��Image
    public float fadeDuration = 1f; // �t�F�[�h����
    public float waitDurationVictory = 2f; // �������̑ҋ@����
    public float waitDurationDefeat = 1.5f; // �s�k���̑ҋ@����
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
        // �t�F�[�h�A�E�g��1.5�b�Ԃ����Ď��s
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        for (float t = 0; t < fadeDuration + waitDuration; t += Time.deltaTime)
        {
            if (t >= waitDuration) // �ҋ@���Ԃ��߂�����t�F�[�h�A�E�g�J�n
            {
                color.a = (t - waitDuration) / fadeDuration;
                fadeImage.color = color;
            }
            yield return null;
        }

        // ���S�Ƀt�F�[�h�A�E�g������ɃV�[�������[�h
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
