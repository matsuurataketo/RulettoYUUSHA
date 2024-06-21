using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoulettoGimick : MonoBehaviour
{
    private enum Direction { Right, Left }

    public float rotationSpeed = 100f; // 回転速度
    private List<Direction> directions = new List<Direction>();
    private int currentStep = 0;
    public float timeLimit = 5f;
    private float timer = 0f;
    public bool gameActive = false;
    private float totalRotation = 0f;
    [SerializeField] private Image imagePrefab; // インスタンス化するImageのプレハブ
    [SerializeField] private Transform imageParent; // Imageを格納する親オブジェクト
    [SerializeField] private Sprite rightImage; // 右方向の画像
    [SerializeField] private Sprite leftImage;  // 左方向の画像

    private List<Image> instantiatedImages = new List<Image>(); // インスタンス化された画像のリスト

    public void InitializeGame()
    {
        directions.Clear();
        currentStep = 0;
        totalRotation = 0f;
        timer = 0f;

        // 画像の初期化と削除
        foreach (var img in instantiatedImages)
        {
            Destroy(img.gameObject);
        }
        instantiatedImages.Clear();
    }

    public void StartRouletteGame()
    {
        InitializeGame();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (!gameActive)
        {
            float initialPositionX = -300f; // 初期位置
            float offsetX = 150f; // オフセット

            // ランダムに5回の方向を抽選
            for (int i = 0; i < 5; i++)
            {
                directions.Add((Direction)Random.Range(0, 2));

                // 画像をインスタンス化
                Image newImage = Instantiate(imagePrefab, imageParent);
                newImage.sprite = directions[i] == Direction.Right ? rightImage : leftImage;
                instantiatedImages.Add(newImage);

                // 位置を設定
                RectTransform rectTransform = newImage.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(initialPositionX + i * offsetX, 0);
            }

            // デバッグ用に抽選結果をログに表示
            foreach (var dir in directions)
            {
                Debug.Log(dir);
            }
            gameActive = true;
            timer = 0f;
        }
        yield return new WaitForSeconds(timeLimit);

        // 制限時間を過ぎた場合の処理
        if (currentStep < directions.Count)
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

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                float step = rotationSpeed * Time.deltaTime * Mathf.Abs(scrollInput);

                if (scrollInput < 0 && directions[currentStep] == Direction.Right)
                {
                    // 右回転
                    transform.Rotate(0, 0, -step);
                    totalRotation += step;
                }
                else if (scrollInput > 0 && directions[currentStep] == Direction.Left)
                {
                    // 左回転
                    transform.Rotate(0, 0, step);
                    totalRotation += step;
                }

                // 1回転（360度）したかどうかをチェック
                if (totalRotation >= 360f)
                {
                    totalRotation = 0f;
                    currentStep++;
                    Debug.Log("正しい方向に一回転完了！");

                    // 次のステップに進む
                    if (currentStep >= directions.Count)
                    {
                        // すべてのステップをクリアした場合
                        BothRotationsCompleted();
                    }
                }
            }
        }
    }

    private void BothRotationsCompleted()
    {
        Debug.Log("クリア！");
        gameActive = false;
        // ここにクリア時の処理を追加
        // 例： SomeFunction();
    }
}