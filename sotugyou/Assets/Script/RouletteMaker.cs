using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMaker : MonoBehaviour
{
    //�C���X�^���X��������W
    [SerializeField, Header("���[���b�g��\�����W")] private Transform imageParentTransform;

    //��������text�̕������������p
    [Header("���[���b�g�̎��")] public List<string> choices;

    //��������image�̏��������p
    [Header("���[���b�g���̍����ւ��p�摜")] public List<Sprite> Images;

    //���[���b�g�̐F���������p
    [SerializeField, Header("���[���b�g�̐F")] private List<Color> rouletteColors;

    //���[���b�g�̃v���t�@�u�摜
    [SerializeField, Header("���[���b�g�̃v���t�@�u�摜")] private Image rouletteImage;

    [SerializeField, Header("���[���b�g���̃v���t�@�u�摜�i�����ւ��O�j")] private GameObject rouletteUIImage;

    //RouletteController�A�b�^�b�`����Ă���I�u�W�F�N�g
    [SerializeField, Header("RouletteController���A�^�b�`����Ă���I�u�W�F�N�g")] private RouletteController rController;

    [Header("���[���b�g�̕�����")]
    public float ratePerRoulette;
    public float rotatePerRoulette;
    private List<Image> rouletteImages = new List<Image>();
    private List<GameObject> rouletteUIImages = new List<GameObject>();

    private Quaternion initialRotation;
    private void Start()
    {
        //�摜�̕����������߂Ă���
        ratePerRoulette = 1 / (float)choices.Count;
        rotatePerRoulette = 360 / (float)(choices.Count);

        for (int i = 0; i < choices.Count; i++)
        {
            //�C���X�^���X��
            var obj = Instantiate(rouletteImage, imageParentTransform);
            var RUI = Instantiate(rouletteUIImage, imageParentTransform);

            //�摜�̐F����������
            obj.color = rouletteColors[(choices.Count - 1 - i)];
            //���x�܂ŕ\�������邩
            obj.fillAmount = ratePerRoulette * (choices.Count - i);
            //�������������v���t�@�u�̎q����T���o�����������Ă���
            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];

            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];
            //�q�I�u�W�F�N�g�̉�]�������Ă��܂�
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseRandomAngle();
        }
    }
    private void IncreaseRandomAngle()
    {
        rouletteImages[1].fillAmount -= 0.00555f;
        rController.rotatePerRouletteStartAngle[2] = rController.rotatePerRouletteStartAngle[2] - 2.05f;
        rController.rotatePerRouletteEndAngle[1] = rController.rotatePerRouletteEndAngle[1] - 2.05f;
    }
}