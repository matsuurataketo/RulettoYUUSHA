using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LodeScene : MonoBehaviour
{
    [SerializeField] private string SceneName;
    // Start is called before the first frame update
    public void OnClickGameStart()
    {
        SceneManager.LoadScene(SceneName);
    }
}
