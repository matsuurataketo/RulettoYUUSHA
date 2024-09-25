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

    private void Start()
    {

        rotatePerRoulette = 360 / (float)(choices.Count);
        float rouletteRatesEnd = 0;
        float rouletteRatesStart = 0;
        float ChildrenrouletteRatesStart = 0;
        float ChildrenrouletteRatesEnd = 0;


        for (int i = 0; i < choices.Count; i++)
        {
            var obj = Instantiate(rouletteImage, imageParentTransform);
            var RUI = Instantiate(rouletteUIImage, imageParentTransform);
            

            obj.color = rouletteColors[(choices.Count - 1 - i)];
            obj.fillAmount = ratePerRoulette;
            ratePerRoulette -= rouletteRates[i];

            rController.rotatePerRouletteStartAngle.Add(rouletteRatesStart);
            Debug.Log("スタート" + rouletteRatesStart);
            rouletteRatesStart += rouletteRates[rouletteRates.Count - i - 1] * 360;

            rouletteRatesEnd += rouletteRates[rouletteRates.Count - i - 1] * 360;
            Debug.Log("終わり" + rouletteRatesEnd);
            rController.rotatePerRouletteEndAngle.Add(rouletteRatesEnd);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];
            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];

            ChildrenrouletteRatesStart = rouletteRates[i] * 360;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2)+ ChildrenrouletteRatesEnd);
            RUI.transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            ChildrenrouletteRatesEnd += rouletteRates[i] * 360;


            rouletteImages.Add(obj);
            rouletteUIImages.Add(RUI);

            // 初期状態を保存
            initialRotations.Add(obj.transform.GetChild(0).transform.rotation);
            initialFillAmounts.Add(obj.fillAmount);
        }
        rController.rMaker = this;
        rController.roulette = imageParentTransform.gameObject;
    }

    // リセットメソッド
    public void ResetRoulette()
    {
        for (int i = 0; i < rouletteImages.Count; i++)
        {
            // 初期状態にリセット
            rouletteImages[i].transform.GetChild(0).transform.rotation = initialRotations[i];
            rouletteUIImages[i].transform.rotation = initialRotations[i];
            rouletteImages[i].fillAmount = initialFillAmounts[i];
        }
    }

    public void IncreaseRandomAngle()
    {

        rouletteImages[1].fillAmount -= 0.002775f;
        rController.rotatePerRouletteEndAngle[2] = rController.rotatePerRouletteEndAngle[2] - 1.025f;
        rController.rotatePerRouletteStartAngle[3] = rController.rotatePerRouletteStartAngle[3] - 1.025f;

        for (int i = 0; i < choices.Count - 2; i++)
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