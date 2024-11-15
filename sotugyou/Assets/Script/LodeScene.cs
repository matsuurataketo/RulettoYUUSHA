using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField, Header("�ړ�����V�[���̖��O")] private string SceneName;
    // Start is called before the first frame update
    public void OnClickGameStart()
    {
        SceneManager.LoadScene(SceneName);
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
}
