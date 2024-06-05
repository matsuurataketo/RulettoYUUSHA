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
    [SerializeField] private TextMeshProUGUI resultText; // ���ʂ̕\��TEXT
    public float initialRotationSpeed = 1000f; // ���[���b�g�̏�����]�X�s�[�h
    private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    private bool isSpinning = false; // ���[���b�g����]���Ă��邩�ǂ����̃t���O
    public float minDecelerationRate = 0.98f; // �ŏ�������
    public float maxDecelerationRate = 0.995f; // �ő匸����
    public float minimumSpeed = 0.1f; // �Œᑬ�x
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
            StartCoroutine(SpinRoulette());
        }
    }

    private IEnumerator SpinRoulette()
    {
        isSpinning = true;
        rouletteSpeed = initialRotationSpeed;

        // ���[���b�g�̑��x�����X�Ɍ���������
        while (rouletteSpeed > minimumSpeed)
        {
            // �����_���Ȍ�������K�p
            float decelerationRate = Random.Range(minDecelerationRate, maxDecelerationRate);
            rouletteSpeed *= decelerationRate;

            yield return null; // ���̃t���[���܂őҋ@
        }

        rouletteSpeed = 0;
        isSpinning = false;
        ShowResult(roulette.transform.eulerAngles.z);
    }

    private void ShowResult(float x)
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
                HPmanegment.UpdatePlayerDownHP(50);
                break;
            case "���Ⴍ":
                EnemyAttack();
                HPmanegment.UpdatePlayerDownHP(30);
                break;
            case "misu":
                HPmanegment.UpdatePlayerDownHP(0);
                break;

            // �Z�I�����[���b�g
            case "oisii":
                HPmanegment.UpdateEnemyUPHP(50);
                break;
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
