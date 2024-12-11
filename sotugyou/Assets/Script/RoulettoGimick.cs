using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoulettoGimick : MonoBehaviour
{
    private enum Direction { Right, Left }

    [Header("ルーレットのスピード")] public float rotationSpeed = 100f;
    private Direction currentDirection;
    public float CrearNum = 0;

    [Header("ミニゲームの時間")] public float timeLimit = 5f;
    private float timer = 0f;
    [Header("ゲーム中か否か")] public bool gameActive = false;
    private float totalRotation = 0f;

    [SerializeField, Header("インスタンス化するImageのプレハブ")] private Image imagePrefab;
    [SerializeField, Header("Imageを格納する親オブジェクト")] private Transform imageParent;
    [SerializeField, Header("右方向の画像")] private Sprite rightImage;
    [SerializeField, Header("左方向の画像")] private Sprite leftImage;
    [SerializeField, Header("デバイス画像")] GameObject dviceimag;

    private Image displayedImage; // 表示される画像
    AudioManager audioManager;
    MinigameEfect MinigameEfect;

    private void Start()
    {
        MinigameEfect = FindObjectOfType<MinigameEfect>();
    }

    public void InitializeGame()
    {
        totalRotation = 0f;
        timer = 0f;

        if (displayedImage != null)
        {
            Destroy(displayedImage.gameObject);
        }

        // 新しい画像を1つ生成
        displayedImage = Instantiate(imagePrefab, imageParent);
    }

    public void StartRouletteGame()
    {
        dviceimag.transform.rotation = Quaternion.Euler(0, 0, 0);
        audioManager = FindObjectOfType<AudioManager>();
        InitializeGame();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (!gameActive)
        {
            CrearNum = 0;
            UpdateDirection(); // 最初の方向を設定
            gameActive = true;
            timer = 0f;
        }

        yield return new WaitForSeconds(timeLimit);

        // 制限時間を過ぎた場合の処理
        if (gameActive)
        {
            Debug.Log("時間切れ！");
            gameActive = false;
        }
    }

    void Update()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;

            // 制限時間に達したらゲーム終了
            if (timer >= timeLimit)
            {
                Debug.Log("時間切れ！");
                gameActive = false;
                return;
            }

            // マウスホイール入力の処理
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                float step = rotationSpeed * Time.deltaTime * Mathf.Abs(scrollInput);

                if (scrollInput < 0 && currentDirection == Direction.Right)
                {
                    // 右回転
                    transform.Rotate(0, 0, -step);
                    totalRotation += step;
                }
                else if (scrollInput > 0 && currentDirection == Direction.Left)
                {
                    // 左回転
                    transform.Rotate(0, 0, step);
                    totalRotation += step;
                }

                // 1回転（360度）したかどうかをチェック
                if (totalRotation >= 90f)
                {
                    totalRotation = 0f;
                    CrearNum++;
                    audioManager.PlaySound("正解");
                    Debug.Log("正しい方向に一回転完了！");
                    MinigameEfect.PlayParticle();

                    // 次の方向を抽選して更新
                    UpdateDirection();
                    dviceimag.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    private void UpdateDirection()
    {
        // 新しい方向を抽選
        currentDirection = (Direction)Random.Range(0, 2);

        // 表示を更新
        displayedImage.sprite = currentDirection == Direction.Right ? rightImage : leftImage;

        Debug.Log($"次の方向: {currentDirection}");
    }
}