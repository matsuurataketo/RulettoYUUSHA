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
        // ���ׂẴJ�����𖳌��ɂ���i��: �؂�ւ��p�J����������ꍇ�j
        foreach (Camera cam in Camera.allCameras)
        {
            cam.gameObject.SetActive(false);
        }
        // ���C���J������L����
        mainCamera.gameObject.SetActive(true);
    }
}
