using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;
    public float zoomSpeed = 2.0f; // �Y�[�����x
    public float minZoom = 40f; // �ŏ��Y�[���i�J�����̃t�B�[���h�I�u�r���[�j
    public float maxZoom = 60f; // �ő�Y�[���i�J�����̃t�B�[���h�I�u�r���[�j

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    public void ZoomIn()
    {
        if (cam.fieldOfView > minZoom)
        {
            cam.fieldOfView -= zoomSpeed * Time.deltaTime;
        }
    }

    public void ZoomOut()
    {
        if (cam.fieldOfView < maxZoom)
        {
            cam.fieldOfView += zoomSpeed * Time.deltaTime;
        }
    }
}
