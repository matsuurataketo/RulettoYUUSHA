using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class UIElement
{
    public string name;  // UI要素の名前（説明）
    public GameObject element;  // 対応するUI要素
}

public class UIListController : MonoBehaviour
{
    // Text, Image, Buttonをそれぞれ説明付きでリスト管理
    public List<UIElement> textElements;
    public List<UIElement> imageElements;
    public List<UIElement> buttonElements;

    void Start()
    {
        HideTextUI();
        HideImageUI();
    }

    // 全てのUI要素を非表示にするメソッド
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


    // リスト内の特定のText要素（インデックス指定）を表示/非表示にするメソッド
    public void ToggleSpecificText(int index)
    {
        if (index >= 0 && index < textElements.Count)
        {
            textElements[index].element.SetActive(!textElements[index].element.activeSelf);
        }
    }

    // リスト内の特定のImage要素（インデックス指定）を表示/非表示にするメソッド
    public void ToggleSpecificImage(int index)
    {
        if (index >= 0 && index < imageElements.Count)
        {
            imageElements[index].element.SetActive(!imageElements[index].element.activeSelf);
        }
    }

    // リスト内の特定のButton要素（インデックス指定）を表示/非表示にするメソッド
    public void ToggleSpecificButton(int index)
    {
        if (index >= 0 && index < buttonElements.Count)
        {
            buttonElements[index].element.SetActive(!buttonElements[index].element.activeSelf);
        }
    }

    public void KougekiRoulettoText(int index, string newText)
    {
        if (index >= 0 && index < textElements.Count)
        {
            // textElements[index]のelementからTextMeshProUGUIコンポーネントを取得
            TextMeshProUGUI textComponent = textElements[index].element.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                // TextMeshProUGUIコンポーネントのtextプロパティを新しいテキストに変更
                textComponent.text = newText;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUIコンポーネントが見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning("指定されたインデックスが範囲外です。");
        }
    }
}