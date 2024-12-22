using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RendaPshImage : MonoBehaviour
{
    public Image image1; // 最初のImage
    public Image image2; // 切り替え先のImage
    public float switchDuration = 0.1f; // 切り替え間隔

    void Update()
    {
        // スペースキーを押したらアニメーション開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SwitchImages());
        }
    }

    private System.Collections.IEnumerator SwitchImages()
    {
        image1.enabled = false; // Image1をオン/オフ
        image2.enabled = true; // Image2をオン/オフ
        yield return new WaitForSeconds(switchDuration); // 指定時間待つ

        // 最後に元の状態に戻す
        image1.enabled = true;
        image2.enabled = false;
    }
}
