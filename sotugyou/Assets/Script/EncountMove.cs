using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z方向の移動速度
    public string targetSceneName = "MaingameScene 1"; // 遷移先のシーン名

    void Update()
    {
        // Z方向に移動
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // コライダーにぶつかった際の処理
        if (other.CompareTag("Suraimu")) // コライダーのタグが"Target"の場合
        {
            Debug.Log("Collision detected! Changing scene...");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
