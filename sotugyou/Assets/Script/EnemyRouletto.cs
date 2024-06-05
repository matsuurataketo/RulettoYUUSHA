using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyRoulette : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public EnemyMaker rMaker;

    private string result; // ルーレットの結果の格納変数
    [SerializeField] private TextMeshProUGUI resultText; // 結果の表示TEXT
    public float initialRotationSpeed = 1000f; // ルーレットの初期回転スピード
    private float rouletteSpeed; // ルーレットの速度を保持する変数
    private bool isSpinning = false; // ルーレットが回転しているかどうかのフラグ
    public float minDecelerationRate = 0.98f; // 最小減速率
    public float maxDecelerationRate = 0.995f; // 最大減速率
    public float minimumSpeed = 0.1f; // 最低速度
    public bool IsSpinning => isSpinning;

    HPmanegment HPmanegment;


   

    private void Start()
    {
        if (roulette == null)
        {
            roulette = gameObject; // ルーレットが設定されていない場合、自身をルーレットとして設定
        }
    }

    private void Update()
    {
        if (isSpinning)
        {
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartRoulette()
    {
        if (!isSpinning)
        {
            StartCoroutine(SpinRoulette());
        }
    }

    private IEnumerator SpinRoulette()
    {
        isSpinning = true;
        rouletteSpeed = initialRotationSpeed;

        // ルーレットの速度を徐々に減速させる
        while (rouletteSpeed > minimumSpeed)
        {
            // ランダムな減速率を適用
            float decelerationRate = Random.Range(minDecelerationRate, maxDecelerationRate);
            rouletteSpeed *= decelerationRate;

            yield return null; // 次のフレームまで待機
        }

        rouletteSpeed = 0;
        isSpinning = false;
        ShowResult(roulette.transform.eulerAngles.z);
    }

    private void ShowResult(float x)
    {
        for (int i = 1; i <= rMaker.choices.Count; i++)
        {
            if (((rotatePerRoulette * (i - 1) <= x) && x <= (rotatePerRoulette * i)) ||
                (-(360 - ((i - 1) * rotatePerRoulette)) >= x && x >= -(360 - (i * rotatePerRoulette))))
            {
                result = rMaker.choices[i - 1];
            }
        }

        switch (result)
        {
            // 技選択ルーレット
            case "きょう":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(50);
                break;
            case "じゃく":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(30);
                break;
            case "misu":
                HPmanegment.UpdatePlayerDownHP(0);
                break;

            // 技選択ルーレット
            case "oisii":
                HPmanegment.UpdateEnemyUPHP(50);
                break;
            case "nigai":
                HPmanegment.UpdateEnemyUPHP(30);
                break;
            case "karai":
                HPmanegment.UpdateEnemyUPHP(10);
                break;

            default:
                break;
        }
    }

    private void EnemyAttack()
    {
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
    }
}
