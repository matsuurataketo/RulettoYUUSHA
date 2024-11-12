using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class RouletteController : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public List<float> rotatePerRouletteStartAngle;
    [HideInInspector] public List<float> rotatePerRouletteEndAngle;
    [HideInInspector] public RouletteMaker rMaker;

    private string result;//���[���b�g�̌��ʂ̊i�[�ϐ�
    [SerializeField,Header("���[���b�g�̌��ʕ\���e�L�X�g")] private TextMeshProUGUI resultText;//���ʂ̕\��TEXT
    [Header("���[���b�g�̉�]�X�s�[�h")] public float rotationSpeed = 7.0f;//���[���b�g�̉�]�X�s�[�h
    private float lastScrollWheelInputTime; // �Ō�Ƀ}�E�X�X�N���[���z�C�[���̓��͂�����������
    public bool ScrollWheel = false;//�ŏ��̃}�E�X�z�C�[������ϐ�
    [SerializeField, Header("���[���b�g�̃��A���ȉ�]���x")] private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    public float RouletteSpeed => rouletteSpeed; // �v���p�e�B����ĊO������A�N�Z�X�ł���悤�ɂ���
    private Quaternion previousRotation;//���[���b�g�̂���]�̕ϐ�
    private int frameCount = 0;//���n�߂Ă���̃t���[���J�E���^�[
    private int comparisonInterval = 60; // ��r�Ԋu

    Slider _slider; //HP�o�[
    [SerializeField, Header("�G���[���b�g�̒�~���")] GameObject EnemyroulettoYazirusi;//
    [SerializeField, Header("�������[���b�g�̒�~���")] GameObject RoulettoYazirusi;//
    [SerializeField , Header("�~�j�Q�[��Script�I�u�W�F�N�g")] GameObject RoulettoGame;
    [SerializeField, Header("�~�j�Q�[���Ɏg���摜�v���t�@�u")] GameObject RightLeftImage;
    [SerializeField, Header("SetActiv�̐؂�ւ����s���I�u�W�F�N�g")] GameObject[] RoulettoORButton;//�X�L�����[���b�g

    UIManager UIManager;
    ScrollSelect ScrollSelect;      
    HPmanegment HPmanegment;
    CountDownTimer countDownTimer;
    ActivScene activScene;

    [SerializeField, Header("�G�̃��[���b�g�I�u�W�F�N�g")] GameObject enemyroulette;
    [SerializeField, Header("EnemyRoulette���A�^�b�`����Ă���I�u�W�F�N�g")] EnemyRoulette enemyScript;
    [SerializeField,Header("�^�C�����C�����A�b�^�b�`")]TimelineManager timelinemanager;

    private RoulettoGimick roulettoGimick;
    private bool rulettogimickflag = false;
    bool scrollSoundPlayed = false; // SE���Đ����ꂽ���ǂ����������t���O
    AudioManager audioManager;
    UIListController Uilistcontroller;
    bool scrollWheelEnabled = true;  // �}�E�X�z�C�[���̓��͂�L��/�����ɂ���t���O
    bool LedyImage = false;
    bool LedyButton = false;
    float UpRouletteRates;
    float DownRouletteRates;


    private void Start()
    {
        roulettoGimick = FindObjectOfType<RoulettoGimick>();
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        countDownTimer=GameObject.Find("MiniGameTimer").GetComponent<CountDownTimer>();
        activScene = GameObject.Find("ActiveScene").GetComponent<ActivScene>();
        audioManager = FindObjectOfType<AudioManager>();
        Uilistcontroller = FindObjectOfType<UIListController>();
    }

    private void Update()
    {
        if (LedyImage == false)
        {
            UpRouletteRates = rMaker.rouletteRates[0];
            DownRouletteRates = rMaker.rouletteRates[1];
            Uilistcontroller.ToggleSpecificImage(2);
            for (int i = 0; i < Uilistcontroller.textElements.Count; i++)
                Uilistcontroller.ToggleSpecificText(i);
            Uilistcontroller.ToggleSpecificImage(3);
            Uilistcontroller.KougekiRoulettoText(0, rMaker.choices[3] + "�E�E" + rMaker.rouletteRates[0] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(1, rMaker.choices[2] + "�E�E" + rMaker.rouletteRates[1] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(2, rMaker.choices[1] + "�E�E" + rMaker.rouletteRates[2] * 100 + "%");
            Uilistcontroller.KougekiRoulettoText(3, rMaker.choices[0] + "�E�E" + rMaker.rouletteRates[3] * 100 + "%");
            LedyImage = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && LedyButton == false)
        {
            Uilistcontroller.ToggleSpecificImage(2);
            LedyButton = true;
        }

        if (enemyScript != null && !enemyScript.IsSpinning&&!rulettogimickflag&& scrollWheelEnabled && LedyButton)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // �}�E�X�z�C�[�����񂳂ꂽ�ꍇ
            {
                if (!scrollSoundPlayed) // SE���܂��Đ�����Ă��Ȃ��ꍇ
                {
                    audioManager.PlaySound("���[���b�gSE");
                    scrollSoundPlayed = true;
                }
                   
                ScrollWheel = true; // �t���O�����낵�āA�ȍ~�̏��������s�\�ɂ���
            }
            rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ���[���b�g�̑��x���X�V����
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);
        }

        float tolerance = 0.1f; // Adjust this value based on the precision you need

        frameCount++;

        if (rMaker.randomGame == 0&& scrollWheelEnabled)
        {
            //Debug.Log("�{�^���A��");
            if (Input.GetKeyDown(KeyCode.Space) && ScrollWheel == true)
            {
                audioManager.PlaySound("�A�ŉ�");
                rMaker.IncreaseRandomAngle();
                UpRouletteRates += 0.0025f;//�����ύX
                DownRouletteRates -= 0.0025f;//�����ύX
                Uilistcontroller.KougekiRoulettoText(0, rMaker.choices[3] + "�E�E" + (UpRouletteRates * 100).ToString("F2") + "%");
                Uilistcontroller.KougekiRoulettoText(1, rMaker.choices[2] + "�E�E" + (DownRouletteRates * 100).ToString("F2") + "%");
                Debug.Log("�g�����Ă܂�");
            }
        }
        else if (rMaker.randomGame == 1&& scrollWheelEnabled)
        {
            //Debug.Log("�{�^���ꌂ");
            rotationSpeed = 21f;
            if (Input.GetKey(KeyCode.Space) && ScrollWheel == true)
            {
                Debug.Log("�~�߂Ă��܂�");
                rotationSpeed = 0f;
                scrollWheelEnabled = false;
            }
        }

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            
            float angleDifference = Quaternion.Angle(roulette.transform.rotation, previousRotation);
            if (angleDifference < tolerance && ScrollWheel == true)
            {
                ScrollWheel = false;
                scrollSoundPlayed = false;
                scrollWheelEnabled = false;
                audioManager.StopSound("���[���b�gSE");
                Debug.Log("��]�͓����ł��B");
                ShowResult(roulette.transform.eulerAngles.z);
                rotationSpeed = 5.0f;
            }
            else
            {
                previousRotation = roulette.transform.rotation;
            }
        }
    }

    private void ShowResult(float x)
    {
        for (int i = 0; i < rMaker.choices.Count; i++)
        {
            if (rotatePerRouletteStartAngle[i] <= x && x < rotatePerRouletteEndAngle[i] ||
                -(360 - rotatePerRouletteStartAngle[i]) >= x && x >= -(360 - rotatePerRouletteEndAngle[i]))
            {
                result = rMaker.choices[i];
                Debug.Log(result);

            }
        }

        switch (result)
        {
            case "���Z":
                StartCoroutine(PlayRouletteGame(result, 35, "\n�U��:"));
                Debug.Log(result);
                break;
            case "��Z":
                StartCoroutine(PlayRouletteGame(result, 15, "\n�U��:"));
                Debug.Log("���Ⴍ");
                break;
            case "���Z":
                StartCoroutine(PlayRouletteGame(result, 25, "\n�U��:"));
                Debug.Log("��");
                break;
            case "�m��":
                StartCoroutine(PlayRouletteGame(result, 100f, "\n�U��:"));
                Debug.Log("�m��");
                break;
            case "��������":
                StartCoroutine(PlayRouletteGame(result, 40f, "\n��:"));
                Debug.Log("��������");
                break;
            case "�ɂ���":
                StartCoroutine(PlayRouletteGame(result, 20f, "\n��:"));
                Debug.Log("�ɂ���");
                break;
            case "�ɏ��":
                StartCoroutine(PlayRouletteGame(result, 80f, "\n��:"));
                Debug.Log("�ɏ��");
                break;
            case "���炢":
                StartCoroutine(PlayRouletteGame(result, 10f, "\n��:"));
                Debug.Log("���炢");
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
            RoulettoYazirusi.SetActive(false);

            if (rMaker.randomGame == 1)
                Uilistcontroller.ToggleSpecificImage(0);  // Image���X�g�̍ŏ��̗v�f��\��/��\���ɂ���
            if (rMaker.randomGame == 0)
                Uilistcontroller.ToggleSpecificImage(1);  // Image���X�g�̍ŏ��̗v�f��\��/��\���ɂ���
            for (int i = 0; i < Uilistcontroller.textElements.Count; i++)//Text���X�g���\��/�\��
                Uilistcontroller.ToggleSpecificText(i);
            Uilistcontroller.ToggleSpecificImage(3);
        }

    }

    private IEnumerator PlayRouletteGame(string result, float hpChange, string message)
    {
        yield return new WaitForSeconds(1f);
        SkillRouletto(message);
        //�~�j�Q�[���X�^�[�g
        countDownTimer.StartCountDown();
        RoulettoGame.SetActive(true);
        RightLeftImage.SetActive(true);
        rulettogimickflag = true;
        roulettoGimick.StartRouletteGame();

        // 10�b�ԑҋ@
        yield return new WaitForSeconds(5); 

        RoulettoGame.SetActive(false);
        RightLeftImage.SetActive(false);

        if (result == "���Z" || result == "��Z" || result == "���Z" || result == "�m��")
        {
            // �^�C�����C�����Đ�
            timelinemanager.PlayTimeline(2);

            // �^�C�����C���̍Đ����I������܂őҋ@
            yield return new WaitUntil(() => timelinemanager.playableDirectors[2].state != PlayState.Playing);

            // HP�����������鏈��
            HPmanegment.UpdateEnemyDownHP(hpChange);
        }
        else if (result == "��������" || result == "�ɂ���" || result == "���炢" || result == "�ɏ��")
        {
            HPmanegment.UpdatePlayerUPHP(hpChange);
        }

        //�G�̃��[���b�g�J�n
        activScene.StartEnemyEffect();
        yield return new WaitUntil(() => activScene.HasCompleted);
        enemyroulette.SetActive(true);
        EnemyroulettoYazirusi.SetActive(true);
        enemyScript.StartRoulette();

        // ���[���b�g����~����܂őҋ@
        yield return new WaitUntil(() => !enemyScript.IsSpinning);

        // ���[���b�g����~������̏���
        enemyroulette.SetActive(false);
        EnemyroulettoYazirusi.SetActive(false);

        // �^�C�����C�����Đ�
        timelinemanager.PlayTimeline(1);
        // �^�C�����C���̍Đ����I������܂őҋ@
        yield return new WaitUntil(() => timelinemanager.playableDirectors[1].state != PlayState.Playing);

        enemyScript.ShowResult(enemyScript.roulette.transform.eulerAngles.z);

        RoulettoORButton[0].SetActive(true);
        RoulettoORButton[1].SetActive(true);

        UIManager.StartCountDown();
        activScene.StartPlayerEffect();
        ScrollSelect.currentTime = 0f;

        RoulettoORButton[2].SetActive(false);
        RoulettoYazirusi.SetActive(false);
        RoulettoORButton[3].SetActive(false);
        rulettogimickflag = false;
        rotationSpeed = 5.0f;
        scrollWheelEnabled = true;
        LedyButton = false;
        LedyImage = false;
    }
}