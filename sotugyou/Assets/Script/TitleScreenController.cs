using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
        public Button[] buttons;  // UI上のボタンをInspectorで設定
    private int currentButtonIndex = 0;

    void Start()
    {
        // 最初のボタンを選択
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    void Update()
    {
        // マウスホイールを検知してボタン間を移動
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // 上方向にスクロール
        {
            NavigateToPreviousButton();
        }
        else if (scroll < 0f) // 下方向にスクロール
        {
            NavigateToNextButton();
        }

        // スペースキーで現在選択されているボタンを押す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }

    void NavigateToNextButton()
    {
        // 次のボタンに移動
        currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    void NavigateToPreviousButton()
    {
        // 前のボタンに移動
        currentButtonIndex--;
        if (currentButtonIndex < 0)
        {
            currentButtonIndex = buttons.Length - 1;
        }
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }
}