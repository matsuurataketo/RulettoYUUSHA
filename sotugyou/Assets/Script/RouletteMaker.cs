using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMaker : MonoBehaviour
{
    //インスタンス化する座標
    [SerializeField, Header("ルーレットを表示座標")] private Transform imageParentTransform;

    //複製したtextの文字書き換え用
    [Header("ルーレットの種類")] public List<string> choices;

    //複製したimageの書き換え用
    [Header("ルーレット内の差し替え用画像")] public List<Sprite> Images;

    //ルーレットの色書き換え用
    [SerializeField, Header("ルーレットの色")] private List<Color> rouletteColors;

    //ルーレットのプレファブ画像
    [SerializeField, Header("ルーレットのプレファブ画像")] private Image rouletteImage;

    [SerializeField, Header("ルーレット内のプレファブ画像（差し替え前）")] private GameObject rouletteUIImage;

    //RouletteControllerアッタッチされているオブジェクト
    [SerializeField, Header("RouletteControllerがアタッチされているオブジェクト")] private RouletteController rController;

    [Header("ルーレットの分割数")]
    public float ratePerRoulette;
    public float rotatePerRoulette;
    private List<Image> rouletteImages = new List<Image>();
    private List<GameObject> rouletteUIImages = new List<GameObject>();

    private Quaternion initialRotation;
    private void Start()
    {
        //画像の分割数を求めている
        ratePerRoulette = 1 / (float)choices.Count;
        rotatePerRoulette = 360 / (float)(choices.Count);

        for (int i = 0; i < choices.Count; i++)
        {
            //インスタンス化
            var obj = Instantiate(rouletteImage, imageParentTransform);
            var RUI = Instantiate(rouletteUIImage, imageParentTransform);

            //画像の色を書き換え
            obj.color = rouletteColors[(choices.Count - 1 - i)];
            //何度まで表示させるか
            obj.fillAmount = ratePerRoulette * (choices.Count - i);
            //書き換えたいプレファブの子供を探し出し書き換えている
            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];

            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];
            //子オブジェクトの回転をさせています
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            RUI.transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            //obj.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            rouletteImages.Add(obj);
            rouletteUIImages.Add(RUI);
            rController.rotatePerRouletteStartAngle.Add(rotatePerRoulette * i);
            rController.rotatePerRouletteEndAngle.Add(rotatePerRoulette * (i + 1));
        }
        rController.rMaker = this;
        rController.roulette = imageParentTransform.gameObject;
    }
   
    public void IncreaseRandomAngle()
    {
        rouletteImages[1].fillAmount -= 0.002775f;
        rController.rotatePerRouletteStartAngle[2] = rController.rotatePerRouletteStartAngle[2] - 1.025f;
        rController.rotatePerRouletteEndAngle[1] = rController.rotatePerRouletteEndAngle[1] - 1.025f;
        for (int i = 0; i < choices.Count-1; i++)
        {
            // 現在の回転角度を1度追加して計算
            Vector3 savedRotation = rouletteImages[i].transform.GetChild(0).transform.eulerAngles;
            Vector3 UIsaveRotation = rouletteUIImages[i].transform.eulerAngles;
            savedRotation.z += 0.9f;
            UIsaveRotation.z += 0.9f;
            rouletteImages[i].transform.GetChild(0).transform.rotation = Quaternion.Euler(savedRotation);
            rouletteUIImages[i].transform.rotation = Quaternion.Euler(UIsaveRotation);
        }

    }
}