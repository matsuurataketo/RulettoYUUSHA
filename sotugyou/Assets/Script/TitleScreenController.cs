using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public Button startButton;  // スタートボタンをInspectorで設定

    void Start()
    {
        // シーンが開始されたらスタートボタンにフォーカスを合わせる
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    void Update()
    {
        // スペースキーが押されたらスタートボタンを実行
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startButton.onClick.Invoke();  // スタートボタンのクリックイベントを呼び出す
        }
    }
}