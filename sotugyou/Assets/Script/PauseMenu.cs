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
        // ESCキーが押されたらポーズメニューを表示/非表示にする
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
        // ゲームを一時停止する
        Time.timeScale = 0f;
        // ポーズメニューを表示する
        pauseMenuUI.SetActive(true);
        pauseMenuUI2.SetActive(true);
    }

    public void ResumeGame()
    {
        PlaypauseSound();
        // ゲームを再開する
        Time.timeScale = 1f;
        // ポーズメニューを非表示にする
        pauseMenuUI.SetActive(false);
        pauseMenuUI2.SetActive(false);
    }
    private void PlaypauseSound()
    {
        if (PauseSound != null && PauseSound.clip != null)
        {
            PauseSound.Play(); // AudioSourceに設定されたオーディオクリップを再生
        }
    }
}
