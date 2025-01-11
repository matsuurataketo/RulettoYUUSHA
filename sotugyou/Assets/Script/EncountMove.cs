using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z�����̈ړ����x
    
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
            // Z�����Ɉړ�
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���C�_�[�ɂԂ������ۂ̏���
        if (other.CompareTag("Suraimu")) // �R���C�_�[�̃^�O��"Target"�̏ꍇ
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
