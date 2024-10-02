using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMaker : MonoBehaviour
{
    // インスタンス化する座標
    [SerializeField, Header("ルーレットを表示座標")] private Transform imageParentTransform;

    // 複製したtextの文字書き換え用
    [Header("ルーレットの種類")] public List<string> choices;

    // 複製したimageの書き換え用
    [Header("ルーレット内の差し替え用画像")] public List<Sprite> Images;

    // ルーレットの色書き換え用
    [SerializeField, Header("ルーレットの色")] private List<Color> rouletteColors;

    // ルーレットの割合をインスペクターから設定できるリスト
    [Header("ルーレットの割合（各要素の割合を設定）")]
    public List<float> rouletteRates;

    // ルーレットのプレファブ画像
    [SerializeField, Header("ルーレットのプレファブ画像")] private Image rouletteImage;

    [SerializeField, Header("ルーレット内のプレファブ画像（差し替え前）")] private GameObject rouletteUIImage;

    // RouletteControllerアタッチされているオブジェクト
    [SerializeField, Header("RouletteControllerがアタッチされているオブジェクト")] private RouletteController rController;

    [Header("ルーレットの分割数")]
    private float ratePerRoulette = 1;
    public float rotatePerRoulette;
    private List<Image> rouletteImages = new List<Image>();
    private List<GameObject> rouletteUIImages = new List<GameObject>();

    // 初期状態を保存するリスト
    private List<Quaternion> initialRotations = new List<Quaternion>();
    private List<float> initialFillAmounts = new List<float>();

    private List<GameObject> instantiatedObjects = new List<GameObject>(); // インスタンス化したオブジェクトのリスト

    private void Start()
    {

    }
    public void RuleetSet()
    {
        // 前のターンのオブジェクトを削除
        ClearPreviousTurnObjects();

        rotatePerRoulette = 360 / (float)(choices.Count);
        float rouletteRatesEnd = 0;
        float rouletteRatesStart = 0;
        float ChildrenrouletteRatesStart = 0;
        float ChildrenrouletteRatesEnd = 0;

        // ratePerRouletteを初期化
        float ratePerRoulette = 1f; // 必要に応じて初期化値を調整

        for (int i = 0; i < choices.Count; i++)
        {
            var obj = Instantiate(rouletteImage, imageParentTransform);  // 新しいオブジェクトをインスタンス化
            var RUI = Instantiate(rouletteUIImage, imageParentTransform); // 新しいオブジェクトをインスタンス化

            // 色の設定 (Alpha値を1にする)
            Color color = rouletteColors[(choices.Count - 1 - i)];
            obj.color = new Color(color.r, color.g, color.b, 1f); // 不透明に設定

            obj.fillAmount = ratePerRoulette;
            ratePerRoulette -= rouletteRates[i];

            rController.rotatePerRouletteStartAngle.Add(rouletteRatesStart);
            rouletteRatesStart += rouletteRates[rouletteRates.Count - i - 1] * 360;

            rouletteRatesEnd += rouletteRates[rouletteRates.Count - i - 1] * 360;
            rController.rotatePerRouletteEndAngle.Add(rouletteRatesEnd);

            // スプライトの設定が正しいか確認
            Image imageComponent = RUI.GetComponentInChildren<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = Images[(Images.Count - 1 - i)];
                imageComponent.enabled = true; // Image コンポーネントが有効か確認
            }
            else
            {
                Debug.LogWarning("RUIのImageコンポーネントが見つかりませんでした。");
            }

            // 透明度をチェック
            if (obj.color.a < 1f)
            {
                Debug.LogWarning($"objの透明度が不正です: {obj.color.a}");
            }

            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];
            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];

            ChildrenrouletteRatesStart = rouletteRates[i] * 360;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            RUI.transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            ChildrenrouletteRatesEnd += rouletteRates[i] * 360;

            rouletteImages.Add(obj);
            rouletteUIImages.Add(RUI);

            // 新しく生成したオブジェクトをリストに保存
            instantiatedObjects.Add(obj.gameObject);
            instantiatedObjects.Add(RUI.gameObject);
        }
        rController.rMaker = this;
        rController.roulette = imageParentTransform.gameObject;
    }

    private void ClearPreviousTurnObjects()
    {
        imageParentTransform.transform.rotation = Quaternion.Euler(0, 0, 0);
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj); // ゲームオブジェクトを削除
        }
        instantiatedObjects.Clear(); // リストをクリア
        rouletteImages.Clear();
        rController.rotatePerRouletteStartAngle.Clear();
        rController.rotatePerRouletteEndAngle.Clear();
        rouletteUIImages.Clear();
    }

    public void IncreaseRandomAngle()
    {
        if (rouletteImages.Count > 1 && rouletteImages[1] != null) // nullチェックを追加
        {
            rouletteImages[1].fillAmount -= 0.002775f;
            rController.rotatePerRouletteEndAngle[2] -= 1.025f;
            rController.rotatePerRouletteStartAngle[3] -= 1.025f;
        }

        for (int i = 0; i < choices.Count - 2; i++)
        {
            if (rouletteImages[i] != null && rouletteUIImages[i] != null) // nullチェックを追加
            {
                Vector3 savedRotation = rouletteImages[i].transform.GetChild(0).transform.eulerAngles;
                Vector3 UIsaveRotation = rouletteUIImages[i].transform.eulerAngles;
                savedRotation.z += 0.9f;
                UIsaveRotation.z += 0.9f;
                rouletteImages[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(savedRotation);
                rouletteUIImages[i].transform.rotation = Quaternion.Euler(UIsaveRotation);
            }
        }
    }
}