using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelect : MonoBehaviour
{
    [Header("���E�̃{�^��")]public Button[] buttons; // ���E�̃{�^��
    [Header("SetActiv�̐؂�ւ�")]public GameObject[] objectsToActivate;
    [SerializeField,Header("����SetActiv�̐؂�ւ�")] GameObject[] RoulettoYazirusi;
    public int selectedIndex = 0; // ���݂̑I���C���f�b�N�X
    public float selectionTime = 10f; // �I������
    [Header("���݂̌o�ߎ���")]public bool currentFrag = true; // ���݂̌o�ߎ���
    UIManager uiManager;
    public RouletteMaker KougekirMaker;
    public RouletteMaker KaihukurMaker;
    UIListController uilistcontroller;


    void Start()
    {
        uilistcontroller = FindObjectOfType<UIListController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // �ŏ��̃{�^����I����Ԃɂ���
        SelectButton(selectedIndex);
        //uiManager.StartCountDown();
        
    }

    void Update()
    {
        // �}�E�X�X�N���[���̓��͂��󂯎��
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // �X�N���[�������ɂ���đI����ύX����
        if (scroll > 0 && buttons[0].gameObject.activeSelf)
        {
            selectedIndex = 0;
        }
        else if (scroll < 0 && buttons[1].gameObject.activeSelf)
        {
            selectedIndex = 1;
        }

        // �I�����ꂽ�{�^�����X�V����
        SelectButton(selectedIndex);

        // �I�������肳���܂ł̎��Ԃ��J�E���g
        //currentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)&& currentFrag)
        {
            // �I�������肷�鏈���������ɒǉ�����
            buttons[selectedIndex].onClick.Invoke();
            Debug.Log("Button " + selectedIndex + " selected!");
        }
        if (!objectsToActivate[1].activeSelf)
            currentFrag = false;

        //Debug.Log(currentTime);
    }

    // �w�肵���C���f�b�N�X�̃{�^����I����Ԃɂ���
    void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // �C���f�b�N�X����v����{�^����I����Ԃɂ���
            buttons[i].interactable = (i == index);
        }
    }
    public void ActivateObjects()
    {
        if (selectedIndex == 0)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySound("���[���b�g����SE");
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[0].SetActive(true);

            RoulettoYazirusi[0].SetActive(true);
            //���[���b�g�I�u�W�F�N�g�̎q����S���A�N�e�B�u�ɂ��Ă���
            {
                // �e�I�u�W�F�N�g���擾����
                GameObject parentObject = objectsToActivate[0];

                // �e�I�u�W�F�N�g�̂��ׂĂ̎q�������[�v�Ŏ擾���ăA�N�e�B�u�ɂ���
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[4];
                parentObject.SetActive(true);
                Debug.Log("�Ăяo���Ă��܂�");
                KaihukurMaker.RuleetSet();

                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
                if (KaihukurMaker.randomGame == 0)
                    uilistcontroller.ToggleSpecificImage(1);
                if (KaihukurMaker.randomGame == 1)
                    uilistcontroller.ToggleSpecificImage(0);
            }
        }

        else
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySound("���[���b�g����SE");
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[3].SetActive(true);

            RoulettoYazirusi[0].SetActive(true);
            //���[���b�g�I�u�W�F�N�g�̎q����S���A�N�e�B�u�ɂ��Ă���
            {
                // �e�I�u�W�F�N�g���擾����
                GameObject parentObject = objectsToActivate[3];

                // �e�I�u�W�F�N�g�̂��ׂĂ̎q�������[�v�Ŏ擾���Ĕ�A�N�e�B�u�ɂ���
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[5];
                parentObject.SetActive(true);
                KougekirMaker.RuleetSet();

                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
                if (KougekirMaker.randomGame == 0)
                    uilistcontroller.ToggleSpecificImage(1);
                if (KougekirMaker.randomGame == 1)
                    uilistcontroller.ToggleSpecificImage(0);
            }
        }
       
    }
}
