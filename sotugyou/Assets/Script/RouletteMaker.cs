using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteMaker : MonoBehaviour
{
    //�C���X�^���X��������W
    [SerializeField] private Transform imageParentTransform;

    //��������text�̕������������p
    public List<string> choices;

    //��������image�̏��������p
    public List<Sprite> Images;

    //���[���b�g�̐F���������p
    [SerializeField] private List<Color> rouletteColors;

    //���[���b�g�̃v���t�@�u�摜
    [SerializeField] private Image rouletteImage;

    [SerializeField] private GameObject rouletteUIImage;

    //RouletteController�A�b�^�b�`����Ă���I�u�W�F�N�g
    [SerializeField] private RouletteController rController;

    public float ratePerRoulette;
    public float rotatePerRoulette;
    private void Start()
    {
        //�摜�̕����������߂Ă���
        ratePerRoulette = 1 / (float)choices.Count;
        rotatePerRoulette = 360 / (float)(choices.Count);


        for (int i = 0; i < choices.Count; i++)
        {
            //�C���X�^���X��
            var obj = Instantiate(rouletteImage, imageParentTransform);
            var RUI =Instantiate(rouletteUIImage,imageParentTransform);

            //�摜�̐F����������
            obj.color= rouletteColors[(choices.Count - 1 - i)];
            //���x�܂ŕ\�������邩
            obj.fillAmount = ratePerRoulette * (choices.Count - i);
            //�������������v���t�@�u�̎q����T���o�����������Ă���
            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];

            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];
            //�q�I�u�W�F�N�g�̉�]�������Ă��܂�
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            RUI.transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
            //obj.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, ((rotatePerRoulette / 2) + rotatePerRoulette * i));
        }
        rController.rMaker = this;
        rController.rotatePerRoulette = rotatePerRoulette;
        rController.roulette = imageParentTransform.gameObject;
    }
}