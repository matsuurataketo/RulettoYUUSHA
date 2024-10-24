using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyRoulette : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public EnemyMaker rMaker;

    private string result; // ���[���b�g�̌��ʂ̊i�[�ϐ�
    [SerializeField, Header("���ʕ\��TEXT")] private TextMeshProUGUI resultText; // ���ʂ̕\��TEXT
    [Header("���[���b�g�̉�]���x")] float initialRotationSpeed = 500f; // ���[���b�g�̏�����]�X�s�[�h
    private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    private bool isSpinning = false; // ���[���b�g����]���Ă��邩�ǂ����̃t���O
    public float minRotationSpeed = 1000f; // ������]���x�̍ŏ��l
    public float maxRotationSpeed = 2500f; // ������]���x�̍ő�l
    [Header("���[���b�g�̍ŏ�������")] public float minDecelerationRate = 0.1f; // �ŏ�������
    [Header("���[���b�g�̍ő匸����")] public float maxDecelerationRate = 0.5f; // �ő匸����
    [Header("���[���b�g�̍Œᑬ�x")] public float minimumSpeed = 4.5f; // �Œᑬ�x
    public bool IsSpinning => isSpinning;

    HPmanegment HPmanegment;


   

    private void Start()
    {
        if (roulette == null)
        {
            roulette = gameObject; // ���[���b�g���ݒ肳��Ă��Ȃ��ꍇ�A���g�����[���b�g�Ƃ��Đݒ�
        }
    }

    private void Update()
    {
        if (isSpinning)
        {
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartRoulette()
    {
        if (!isSpinning)
        {
            initialRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            StartCoroutine(SpinRoulette());
        }
    }

    private IEnumerator SpinRoulette()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlaySound("���[���b�gSE");
        isSpinning = true;
        rouletteSpeed = initialRotationSpeed;

        // ���[���b�g�̑��x�����X�Ɍ���������
        while (rouletteSpeed > minimumSpeed)
        {
            // �����_���Ȍ�������K�p
            float decelerationRate = Random.Range(minDecelerationRate, maxDecelerationRate);
            rouletteSpeed *= decelerationRate;
            Debug.Log(rouletteSpeed);
            yield return null; // ���̃t���[���܂őҋ@
        }


        rouletteSpeed = 0;
        audioManager.StopSound("���[���b�gSE");
        isSpinning = false;
    }

    public void ShowResult(float x)
    {
        for (int i = 1; i <= rMaker.choices.Count; i++)
        {
            if (((rotatePerRoulette * (i - 1) <= x) && x <= (rotatePerRoulette * i)) ||
                (-(360 - ((i - 1) * rotatePerRoulette)) >= x && x >= -(360 - (i * rotatePerRoulette))))
            {
                result = rMaker.choices[i - 1];
            }
        }

        switch (result)
        {
            // �Z�I�����[���b�g
            case "���傤":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(45); 
                break;
            case "���イ":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(30);
                break;
            case "���Ⴍ":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(15);
                break;
            case "�݂�":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(0);
                break;

            // �Z�I�����[���b�g
            case "nigai":
                HPmanegment.UpdateEnemyUPHP(30);
                break;
            case "karai":
                HPmanegment.UpdateEnemyUPHP(10);
                break;

            default:
                break;
        }
    }

    private void EnemyAttack()
    {
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
    }
}
