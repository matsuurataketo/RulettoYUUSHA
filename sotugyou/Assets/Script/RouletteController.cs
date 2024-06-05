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
    private bool ScrollWheel=false;//�ŏ��̃}�E�X�z�C�[������ϐ�
    [SerializeField] private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    public float RouletteSpeed => rouletteSpeed; // �v���p�e�B����ĊO������A�N�Z�X�ł���悤�ɂ���
    private Quaternion previousRotation;//���[���b�g�̂���]�̕ϐ�
    private int frameCount = 0;//���n�߂Ă���̃t���[���J�E���^�[
    private int comparisonInterval = 210; // ��r�Ԋu

    Slider _slider; //HP�o�[
    [SerializeField]GameObject shieldRoulettoObject;//�������߂̃V�[���Ŏg�p
    [SerializeField]GameObject WponsRoulrtto;//�������߂̃V�[���Ŏg�p
    [SerializeField] GameObject[] RoulettoORButton;//�X�L�����[���b�g
    UIManager UIManager;
    ScrollSelect ScrollSelect;
    HPmanegment HPmanegment;

    [SerializeField] GameObject enemyroulette;
    [SerializeField] EnemyRoulette enemyScript;



    private void Start()
    {
        HPmanegment = GameObject.Find("HPManegment").GetComponent<HPmanegment>();
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        ScrollSelect = GameObject.Find("ScrollSelect").GetComponent<ScrollSelect>();
        //_slider = GameObject.Find("EnemyHP").GetComponent<Slider>();
        //_slider.value = 1f;
    }

    private void Update()
    {
        if (enemyScript != null && !enemyScript.IsSpinning)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // �}�E�X�z�C�[�����񂳂ꂽ�ꍇ
                ScrollWheel = true; // �t���O�����낵�āA�ȍ~�̏��������s�\�ɂ���

            rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ���[���b�g�̑��x���X�V����
            roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);
        }
            
        if (frameCount % comparisonInterval == 0 && ScrollWheel==true)
        {
            if (Quaternion.Angle(roulette.transform.rotation, previousRotation) == 0f&&ScrollWheel==true)
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
        
        frameCount++;
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
            //�Z�I�����[���b�g
            case "���傤":
                //�Ƃ�
                //�����ɍU���A�N�V������ǉ�����
                HPmanegment.UpdateEnemyDownHP(1f);
                SkillRouletto("\n�U��:");

                break;
            case "���Ⴍ":
                //�Ƃ�
                HPmanegment.UpdateEnemyDownHP(0.5f);
                SkillRouletto("\n�U��:");
                break;
            case "�݂�":
                //�Ƃ�
                HPmanegment.UpdateEnemyDownHP(0f);
                SkillRouletto("\n�U��:");
                break;

            //�Z�I�����[���b�g
            case "��������":
                //�Ƃ�
                HPmanegment.UpdatePlayerUPHP(50);
                SkillRouletto("\n��:");
                break;
            case "�ɂ���":
                //�Ƃ�
                HPmanegment.UpdatePlayerUPHP(30);
                SkillRouletto("\n��:");
                break;
            case "���炢":
                //�Ƃ�
                HPmanegment.UpdatePlayerUPHP(10);
                SkillRouletto("\n��:");
                break;

            case "kougeki":
                //_slider.value = 0f;
                break;
            case "kaihuku":
                //_slider.value = 1f;
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
        enemyroulette.SetActive(true);
        enemyScript.StartRoulette();

        StartCoroutine(WaitForEnemyRouletteToStop());
    }

    private IEnumerator WaitForEnemyRouletteToStop()
    {
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