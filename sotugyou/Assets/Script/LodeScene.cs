using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField, Header("移動するシーンの名前")] private string SceneName;
    // Start is called before the first frame update
    public void OnClickGameStart()
    {
        SceneManager.LoadScene(SceneName);
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
}
