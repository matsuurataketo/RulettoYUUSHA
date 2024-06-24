using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField, Header("ˆÚ“®‚·‚éƒV[ƒ“‚Ì–¼‘O")] private string SceneName;
    // Start is called before the first frame update
    public void OnClickGameStart()
    {
        SceneManager.LoadScene(SceneName);
    }
}
