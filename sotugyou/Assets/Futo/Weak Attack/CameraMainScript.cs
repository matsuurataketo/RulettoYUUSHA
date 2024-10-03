using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainScript : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void ReturnToMainCamera()
    {
        // すべてのカメラを無効にする（例: 切り替え用カメラがある場合）
        foreach (Camera cam in Camera.allCameras)
        {
            cam.gameObject.SetActive(false);
        }
        // メインカメラを有効化
        mainCamera.gameObject.SetActive(true);
    }
}
