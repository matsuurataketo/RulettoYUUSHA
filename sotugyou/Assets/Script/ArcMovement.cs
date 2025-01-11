using UnityEngine;

public class ArcJump : MonoBehaviour
{
    public float jumpHeight = 2f;     // 跳ねる高さ
    public float speed = 1f;         // 移動速度
    public int jumpDistance = 30;    // 跳ねる距離
    public int jumpCount = 3;        // 跳ねる回数

    private Vector3[] targets;       // 目標地点リスト
    private int currentTargetIndex = 0;
    private float timeElapsed = 0f;
    private EncountMove encountMove;
    public LodeScene lodeScene;
    public string targetSceneName = "MaingameScene 1"; // 遷移先のシーン名
    public GameObject BikriImag;
    public AudioManager audioManager;
    private bool flag1;

    void Start()
    {
        flag1 = false;
        currentTargetIndex = 0;
        timeElapsed = 0f;

        encountMove = FindObjectOfType<EncountMove>();
        // 跳ねる目標地点を計算
        targets = new Vector3[jumpCount + 1];
        targets[0] = transform.position; // 初期位置
        for (int i = 1; i <= jumpCount; i++)
        {
            targets[i] = targets[i - 1] + new Vector3(jumpDistance, 0, 0); // X方向に90ずつ
        }
    }

    void Update()
    {
        if (currentTargetIndex < jumpCount&&encountMove.enemywork==true) // 次の目標地点がある場合
        {
            // 時間経過に応じて進行割合を計算
            timeElapsed += Time.deltaTime * speed;
            float t = Mathf.Clamp01(timeElapsed); // 0～1に制限

            // 現在の目標地点に向けた水平位置を補間
            Vector3 horizontalPosition = Vector3.Lerp(targets[currentTargetIndex], targets[currentTargetIndex + 1], t);

            // 垂直方向の放物線計算
            float arcHeight = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // 新しい位置を設定
            transform.position = horizontalPosition + new Vector3(0, arcHeight, 0);

            // 次の目標地点に到達したらリセット
            if (t >= 1f)
            {
                timeElapsed = 0f;
                currentTargetIndex++;
                audioManager.PlaySound("スライム跳ねる");
            }
        }
        if(currentTargetIndex==3&&flag1==false)
        {
            flag1 = true;
            audioManager.PlaySound("エンカウント");
            BikriImag.SetActive(false);
            StartCoroutine(lodeScene.FadeOut(targetSceneName));
        }
    }
}