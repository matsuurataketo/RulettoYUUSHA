using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMaker : MonoBehaviour
{
    // �C���X�^���X��������W
    [SerializeField, Header("���[���b�g��\�����W")] private Transform imageParentTransform;

    // ��������text�̕������������p
    [Header("���[���b�g�̎��")] public List<string> choices;

    // ��������image�̏��������p
    [Header("���[���b�g���̍����ւ��p�摜")] public List<Sprite> Images;

    // ���[���b�g�̐F���������p
    [SerializeField, Header("���[���b�g�̐F")] private List<Color> rouletteColors;

    // ���[���b�g�̊������C���X�y�N�^�[����ݒ�ł��郊�X�g
    [Header("���[���b�g�̊����i�e�v�f�̊�����ݒ�j")]
    public List<float> rouletteRates;

    // ���[���b�g�̃v���t�@�u�摜
    [SerializeField, Header("���[���b�g�̃v���t�@�u�摜")] private Image rouletteImage;

    [SerializeField, Header("���[���b�g���̃v���t�@�u�摜�i�����ւ��O�j")] private GameObject rouletteUIImage;

    // RouletteController�A�^�b�`����Ă���I�u�W�F�N�g
    [SerializeField, Header("RouletteController���A�^�b�`����Ă���I�u�W�F�N�g")] private RouletteController rController;

    [Header("���[���b�g�̕�����")]
    private float ratePerRoulette = 1;
    public float rotatePerRoulette;
    private List<Image> rouletteImages = new List<Image>();
    private List<GameObject> rouletteUIImages = new List<GameObject>();

    // ������Ԃ�ۑ����郊�X�g
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
            Debug.Log("�X�^�[�g" + rouletteRatesStart);
            rouletteRatesStart += rouletteRates[rouletteRates.Count - i - 1] * 360;

            rouletteRatesEnd += rouletteRates[rouletteRates.Count - i - 1] * 360;
            Debug.Log("�I���" + rouletteRatesEnd);
            rController.rotatePerRouletteEndAngle.Add(rouletteRatesEnd);

            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];
            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];

            ChildrenrouletteRatesStart = rouletteRates[i] * 360;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2)+ ChildrenrouletteRatesEnd);
            RUI.transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            ChildrenrouletteRatesEnd += rouletteRates[i] * 360;


            rouletteImages.Add(obj);
            rouletteUIImages.Add(RUI);

            // ������Ԃ�ۑ�
            initialRotations.Add(obj.transform.GetChild(0).transform.rotation);
            initialFillAmounts.Add(obj.fillAmount);
        }
        rController.rMaker = this;
        rController.roulette = imageParentTransform.gameObject;
    }

    // ���Z�b�g���\�b�h
    public void ResetRoulette()
    {
        for (int i = 0; i < rouletteImages.Count; i++)
        {
            // ������ԂɃ��Z�b�g
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