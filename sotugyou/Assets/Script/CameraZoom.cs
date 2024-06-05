using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera cam;
    public float zoomSpeed = 2.0f; // ズーム速度
    public float minZoom = 40f; // 最小ズーム（カメラのフィールドオブビュー）
    public float maxZoom = 60f; // 最大ズーム（カメラのフィールドオブビュー）

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
