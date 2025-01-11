using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z�����̈ړ����x
    public string targetSceneName = "MaingameScene 1"; // �J�ڐ�̃V�[����
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
            // Z�����Ɉړ�
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���C�_�[�ɂԂ������ۂ̏���
        if (other.CompareTag("Suraimu")) // �R���C�_�[�̃^�O��"Target"�̏ꍇ
        {
            Hit = true;
            StartCoroutine(lodeScene.FadeOut(targetSceneName));
            Debug.Log("Collision detected! Changing scene...");
            
        }
    }
}
