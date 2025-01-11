using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z方向の移動速度
    
    public LodeScene lodeScene;
    public bool Hit;
    public bool enemywork;
    public AudioManager audioManager;
    public Animator animator;
    public GameObject image;

    private void Start()
    {
        image.SetActive(false);
        Hit = true;
        enemywork = false;
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
            image.SetActive(true);
            audioManager.PlaySound("!");
            animator.SetBool("IsWork", false);
            enemywork=true;
            Hit = true;
            Debug.Log("Collision detected! Changing scene...");
            
        }
    }
}
