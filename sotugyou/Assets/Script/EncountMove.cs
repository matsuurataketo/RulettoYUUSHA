using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncountMove : MonoBehaviour
{
    public float speed = 5f; // Z�����̈ړ����x
    public string targetSceneName = "MaingameScene 1"; // �J�ڐ�̃V�[����

    void Update()
    {
        // Z�����Ɉړ�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �R���C�_�[�ɂԂ������ۂ̏���
        if (other.CompareTag("Suraimu")) // �R���C�_�[�̃^�O��"Target"�̏ꍇ
        {
            Debug.Log("Collision detected! Changing scene...");
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
