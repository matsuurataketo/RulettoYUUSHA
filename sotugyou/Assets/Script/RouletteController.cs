using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RouletteController : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public RouletteMaker rMaker;

    private string result;//���[���b�g�̌��ʂ̊i�[�ϐ�
    [SerializeField] private TextMeshProUGUI resultText;//���ʂ̕\��TEXT
    public float rotationSpeed = 5.0f;//���[���b�g�̉�]�X�s�[�h
    private float lastScrollWheelInputTime; // �Ō�Ƀ}�E�X�X�N���[���z�C�[���̓��͂�����������
    public float stopThreshold = 1.0f; // ���[���b�g����~�����Ƃ݂Ȃ�臒l�i�b
    private bool ScrollWheel = false;//�ŏ��̃}�E�X�z�C�[������ϐ�
    [SerializeField] private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    public float RouletteSpeed => rouletteSpeed; // �v���p�e�B����ĊO������A�N�Z�X�ł���悤�ɂ���
    private Quaternion previousRotation;//���[���b�g�̂���]�̕ϐ�
    private int frameCount = 0;//���n�߂Ă���̃t���[���J�E���^�[
    private int comparisonInterval = 60; // ��r�Ԋu

    Slider _slider; //HP�o�[
    [SerializeField] GameObject shieldRoulettoObject;//�������߂̃V�[���Ŏg�p
    [SerializeField] GameObject WponsRoulrtto;//�������߂̃V�[���Ŏg�p
    [SerializeField] GameObject[] RoulettoORButton;//�X�L�����[���b�g
    UIManager UIManager;
    ScrollSelect ScrollSelect;
    HPmanegment HPmanegment;

    [SerializeField] GameObject enemyroulette;
    [SerializeField] EnemyRoulette enemyScript;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;



    private void Start()
    {
        roulettoGimick = FindObjectOfType<RoulettoGimick>();
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        //_slider = GameObject.Find("EnemyHP").GetComponent<Slider>();
        //_slider.value = 1f;
    }

    private void Update()
    {
        if (enemyScript != null && !enemyScript.IsSpinning&&!rulettogimickflag)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // �}�E�X�z�C�[�����񂳂ꂽ�ꍇ
                ScrollWheel = true; // �t���O�����낵�āA�ȍ~�̏��������s�\�ɂ���

            rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ���[���b�g�̑��x���X�V����
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);
        }

        float tolerance = 0.1f; // Adjust this value based on the precision you need

        frameCount++;

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            float angleDifference = Quaternion.Angle(roulette.transform.rotation, previousRotation);
            if (angleDifference < tolerance && ScrollWheel == true)
            {
                ScrollWheel = false;
                Debug.Log("��]�͓����ł��B");
                ShowResult(roulette.transform.eulerAngles.z);
            }
            else
            {
                previousRotation = roulette.transform.rotation;
            }
        }
    }

    private void ShowResult(float x)
    {
        //resultText.text = "Wepon:";
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
            case "���傤":
                StartCoroutine(PlayRouletteGame(result, 1f, "\n�U��:"));
                break;
            case "���Ⴍ":
                StartCoroutine(PlayRouletteGame(result, 0.5f, "\n�U��:"));
                break;
            case "�݂�":
                StartCoroutine(PlayRouletteGame(result, 0f, "\n�U��:"));
                break;
            case "��������":
                StartCoroutine(PlayRouletteGame(result, 50f, "\n��:"));
                break;
            case "�ɂ���":
                StartCoroutine(PlayRouletteGame(result, 30f, "\n��:"));
                break;
            case "���炢":
                StartCoroutine(PlayRouletteGame(result, 10f, "\n��:"));
                break;
            default:
                break;
        }
    }

    private void SkillRouletto(string HitText)
    {
        resultText.text = "";
        resultText.text = resultText.text + HitText + result + "�I���I";

        //���[���b�g��image���\���ɂ��Ă�
        {
            // �e�I�u�W�F�N�g���擾����
            GameObject parentObject = RoulettoORButton[2];

            // �e�I�u�W�F�N�g�̂��ׂĂ̎q�������[�v�Ŏ擾���Ĕ�A�N�e�B�u�ɂ���
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        RoulettoORButton[3].SetActive(false);
    }

    private IEnumerator PlayRouletteGame(string result, float hpChange, string message)
    {
        SkillRouletto(message);
        //�~�j�Q�[���X�^�[�g
        rulettogimickflag = true;
        roulettoGimick.StartRouletteGame();
        // 10�b�ԑҋ@
        yield return new WaitForSeconds(10);
        if (result == "���傤" || result == "���Ⴍ" || result == "�݂�")
        {
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "��������" || result == "�ɂ���" || result == "���炢")
        {
            HPmanegment.UpdatePlayerUPHP(hpChange);
        }
        rulettogimickflag = false;

        //�G�̃��[���b�g�J�n
        enemyroulette.SetActive(true);
        enemyScript.StartRoulette();
        // ���[���b�g����~����܂őҋ@
        yield return new WaitUntil(() => !enemyScript.IsSpinning);
        

        // ���[���b�g����~������̏���
        enemyroulette.SetActive(false);
        RoulettoORButton[0].SetActive(true);
        RoulettoORButton[1].SetActive(true);

        UIManager.StartCountDown();
        ScrollSelect.currentTime = 0f;

        RoulettoORButton[2].SetActive(false);
        RoulettoORButton[4].SetActive(false);
    }
}