using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z方向の移動速度
    public string targetSceneName = "MaingameScene 1"; // 遷移先のシーン名
    public LodeScene lodeScene;
    public bool Hit;

    private void Start()
    {
        Hit = true;
    }
    void Update()
    {
        if (!Hit)
        {
            // Z方向に移動
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // コライダーにぶつかった際の処理
        if (other.CompareTag("Suraimu")) // コライダーのタグが"Target"の場合
        {
            Hit = true;
            StartCoroutine(lodeScene.FadeOut(targetSceneName));
            Debug.Log("Collision detected! Changing scene...");
            
        }
    }
}
