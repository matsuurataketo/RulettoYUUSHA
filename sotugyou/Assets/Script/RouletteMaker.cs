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
        }
        rController.rMaker = this;
        rController.rotatePerRoulette = rotatePerRoulette;
        rController.roulette = imageParentTransform.gameObject;
    }
}