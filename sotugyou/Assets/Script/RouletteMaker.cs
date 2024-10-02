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

    private List<GameObject> instantiatedObjects = new List<GameObject>(); // �C���X�^���X�������I�u�W�F�N�g�̃��X�g

    private void Start()
    {

    }
    public void RuleetSet()
    {
        // �O�̃^�[���̃I�u�W�F�N�g���폜
        ClearPreviousTurnObjects();

        rotatePerRoulette = 360 / (float)(choices.Count);
        float rouletteRatesEnd = 0;
        float rouletteRatesStart = 0;
        float ChildrenrouletteRatesStart = 0;
        float ChildrenrouletteRatesEnd = 0;

        // ratePerRoulette��������
        float ratePerRoulette = 1f; // �K�v�ɉ����ď������l�𒲐�

        for (int i = 0; i < choices.Count; i++)
        {
            var obj = Instantiate(rouletteImage, imageParentTransform);  // �V�����I�u�W�F�N�g���C���X�^���X��
            var RUI = Instantiate(rouletteUIImage, imageParentTransform); // �V�����I�u�W�F�N�g���C���X�^���X��

            // �F�̐ݒ� (Alpha�l��1�ɂ���)
            Color color = rouletteColors[(choices.Count - 1 - i)];
            obj.color = new Color(color.r, color.g, color.b, 1f); // �s�����ɐݒ�

            obj.fillAmount = ratePerRoulette;
            ratePerRoulette -= rouletteRates[i];

            rController.rotatePerRouletteStartAngle.Add(rouletteRatesStart);
            rouletteRatesStart += rouletteRates[rouletteRates.Count - i - 1] * 360;

            rouletteRatesEnd += rouletteRates[rouletteRates.Count - i - 1] * 360;
            rController.rotatePerRouletteEndAngle.Add(rouletteRatesEnd);

            // �X�v���C�g�̐ݒ肪���������m�F
            Image imageComponent = RUI.GetComponentInChildren<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = Images[(Images.Count - 1 - i)];
                imageComponent.enabled = true; // Image �R���|�[�l���g���L�����m�F
            }
            else
            {
                Debug.LogWarning("RUI��Image�R���|�[�l���g��������܂���ł����B");
            }

            // �����x���`�F�b�N
            if (obj.color.a < 1f)
            {
                Debug.LogWarning($"obj�̓����x���s���ł�: {obj.color.a}");
            }

            obj.GetComponentInChildren<TextMeshProUGUI>().text = choices[(choices.Count - 1 - i)];
            RUI.GetComponentInChildren<Image>().sprite = Images[(Images.Count - 1 - i)];

            ChildrenrouletteRatesStart = rouletteRates[i] * 360;
            obj.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            RUI.transform.rotation = Quaternion.Euler(0, 0, (ChildrenrouletteRatesStart / 2) + ChildrenrouletteRatesEnd);
            ChildrenrouletteRatesEnd += rouletteRates[i] * 360;

            rouletteImages.Add(obj);
            rouletteUIImages.Add(RUI);

            // �V�������������I�u�W�F�N�g�����X�g�ɕۑ�
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
            Destroy(obj); // �Q�[���I�u�W�F�N�g���폜
        }
        instantiatedObjects.Clear(); // ���X�g���N���A
        rouletteImages.Clear();
        rController.rotatePerRouletteStartAngle.Clear();
        rController.rotatePerRouletteEndAngle.Clear();
        rouletteUIImages.Clear();
    }

    public void IncreaseRandomAngle()
    {
        if (rouletteImages.Count > 1 && rouletteImages[1] != null) // null�`�F�b�N��ǉ�
        {
            rouletteImages[1].fillAmount -= 0.002775f;
            rController.rotatePerRouletteEndAngle[2] -= 1.025f;
            rController.rotatePerRouletteStartAngle[3] -= 1.025f;
        }

        for (int i = 0; i < choices.Count - 2; i++)
        {
            if (rouletteImages[i] != null && rouletteUIImages[i] != null) // null�`�F�b�N��ǉ�
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