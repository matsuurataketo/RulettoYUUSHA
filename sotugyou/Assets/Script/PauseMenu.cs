using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject pauseMenuUI2;
    private AudioSource PauseSound;

    private void Start()
    {
        Time.timeScale = 1f;
        PauseSound = GetComponent<AudioSource>();
    }
    void Update()
    {
        // ESC�L�[�������ꂽ��|�[�Y���j���[��\��/��\���ɂ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        PlaypauseSound();
        // �Q�[�����ꎞ��~����
        Time.timeScale = 0f;
        // �|�[�Y���j���[��\������
        pauseMenuUI.SetActive(true);
        pauseMenuUI2.SetActive(true);
    }

    public void ResumeGame()
    {
        PlaypauseSound();
        // �Q�[�����ĊJ����
        Time.timeScale = 1f;
        // �|�[�Y���j���[���\���ɂ���
        pauseMenuUI.SetActive(false);
        pauseMenuUI2.SetActive(false);
    }
    private void PlaypauseSound()
    {
        if (PauseSound != null && PauseSound.clip != null)
        {
            PauseSound.Play(); // AudioSource�ɐݒ肳�ꂽ�I�[�f�B�I�N���b�v���Đ�
        }
    }
}
