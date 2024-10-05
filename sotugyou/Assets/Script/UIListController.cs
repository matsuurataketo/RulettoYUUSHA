using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class UIElement
{
    public string name;  // UI�v�f�̖��O�i�����j
    public GameObject element;  // �Ή�����UI�v�f
}

public class UIListController : MonoBehaviour
{
    // Text, Image, Button�����ꂼ������t���Ń��X�g�Ǘ�
    public List<UIElement> textElements;
    public List<UIElement> imageElements;
    public List<UIElement> buttonElements;

    void Start()
    {
        HideImageUI();
    }

    // �S�Ă�UI�v�f���\���ɂ��郁�\�b�h
    public void HideTextUI()
    {
        foreach (UIElement textElement in textElements)
        {
            textElement.element.SetActive(false);
        }

        
    }

    public void HideImageUI()
    {
        foreach (UIElement imageElement in imageElements)
        {
            imageElement.element.SetActive(false);
        }
    }

    public void HideButtonUI()
    {
        foreach (UIElement buttonElement in buttonElements)
        {
            buttonElement.element.SetActive(false);
        }
    }


    // ���X�g���̓����Text�v�f�i�C���f�b�N�X�w��j��\��/��\���ɂ��郁�\�b�h
    public void ToggleSpecificText(int index)
    {
        if (index >= 0 && index < textElements.Count)
        {
            textElements[index].element.SetActive(!textElements[index].element.activeSelf);
        }
    }

    // ���X�g���̓����Image�v�f�i�C���f�b�N�X�w��j��\��/��\���ɂ��郁�\�b�h
    public void ToggleSpecificImage(int index)
    {
        if (index >= 0 && index < imageElements.Count)
        {
            imageElements[index].element.SetActive(!imageElements[index].element.activeSelf);
        }
    }

    // ���X�g���̓����Button�v�f�i�C���f�b�N�X�w��j��\��/��\���ɂ��郁�\�b�h
    public void ToggleSpecificButton(int index)
    {
        if (index >= 0 && index < buttonElements.Count)
        {
            buttonElements[index].element.SetActive(!buttonElements[index].element.activeSelf);
        }
    }
}