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
            NavigateToButton(0); // 1のボタンに移動
        }
        else if (scroll < 0f) // 下方向にスクロール
        {
            NavigateToButton(1); // 0のボタンに移動
        }

        // スペースキーで現在選択されているボタンを押す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }

    void NavigateToButton(int buttonIndex)
    {
        // 指定されたボタンに移動
        if (buttonIndex >= 0 && buttonIndex < buttons.Length)
        {
            currentButtonIndex = buttonIndex;
            EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
        }
    }
}