using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActivScene : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    private bool isIncreasing = true;
    private bool hasCompleted = false;
    public bool HasCompleted => hasCompleted;

    void Start()
    {
        PleyerInitText();
        StartPlayerEffect();
    }

    private void PleyerInitText()
    {
        text.text = "�����̃^�[��";
        Color color = text.color;
        color.a = 0;
        text.color = color;
    }
    private void EnemyInitText()
    {
        text.text = "�G�̃^�[��";
        Color color = text.color;
        color.a = 0;
        text.color = color;
    }

    public void StartPlayerEffect()
    {
        hasCompleted = false;
        isIncreasing = true;
        image.fillAmount = 0;
        PleyerInitText();
        StartCoroutine(EffectCoroutine());
    }

    public void StartEnemyEffect()
    {
        hasCompleted = false;
        isIncreasing = true;
        image.fillAmount = 0;
        EnemyInitText();
        StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        while (!hasCompleted)
        {
            if (isIncreasing)
            {
                image.fillAmount += 0.01f;
                SetTextAlpha(image.fillAmount); // �e�L�X�g�̃A���t�@�l���X�V
                if (image.fillAmount >= 1)
                {
                    isIncreasing = false;
                }
            }
            else
            {
                image.fillAmount -= 0.01f;
                SetTextAlpha(image.fillAmount); // �e�L�X�g�̃A���t�@�l���X�V
                if (image.fillAmount <= 0)
                {
                    hasCompleted = true;
                }
            }
            yield return new WaitForSeconds(0.01f); // �X�V�Ԋu�������҂�
        }
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}