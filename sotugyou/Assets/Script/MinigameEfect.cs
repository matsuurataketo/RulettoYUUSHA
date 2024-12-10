using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameEfect : MonoBehaviour
{

    public GameObject particlePrefab; // パーティクルのプレハブをここに設定する
    public Transform spawnPoint;     // パーティクルを生成する位置（任意）
    // Start is called before the first frame update
    public void PlayParticle()
    {
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f); // x軸を-90度に回転
        GameObject particleInstance = Instantiate(particlePrefab, spawnPoint.position, rotation);

        // 2秒後にパーティクルを削除
        Destroy(particleInstance, 2f);
    }
}
