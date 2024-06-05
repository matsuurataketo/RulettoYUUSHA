using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelect : MonoBehaviour
{
    public Button[] buttons; // ���E�̃{�^��
    public GameObject[] objectsToActivate;
    private int selectedIndex = 0; // ���݂̑I���C���f�b�N�X
    private float selectionTime = 5f; // �I������
    public float currentTime = 0f; // ���݂̌o�ߎ���
    UIManager uiManager;

    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // �ŏ��̃{�^����I����Ԃɂ���
        SelectButton(selectedIndex);
        uiManager.StartCountDown();
    }

    void Update()
    {
        // �}�E�X�X�N���[���̓��͂��󂯎��
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // �X�N���[�������ɂ���đI����ύX����
        if (scroll > 0)
        {
            selectedIndex--; // ��ɃX�N���[��
            if (selectedIndex < 0)
                selectedIndex = buttons.Length - 1;
        }
        else if (scroll < 0)
        {
            selectedIndex++; // ���ɃX�N���[��
            if (selectedIndex >= buttons.Length)
                selectedIndex = 0;
        }

        // �I�����ꂽ�{�^�����X�V����
        SelectButton(selectedIndex);

        // �I�������肳���܂ł̎��Ԃ��J�E���g
        currentTime += Time.deltaTime;

        if (currentTime >= selectionTime)
        {
            // �I�������肷�鏈���������ɒǉ�����
            buttons[selectedIndex].onClick.Invoke();
            Debug.Log("Button " + selectedIndex + " selected!");
        }
        if (!objectsToActivate[1].activeSelf)
            currentTime = 0;
        Debug.Log(currentTime);
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
            currentTime = 0f;
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[0].SetActive(true);
            //���[���b�g�I�u�W�F�N�g�̎q����S���A�N�e�B�u�ɂ��Ă���
            {
                // �e�I�u�W�F�N�g���擾����
                GameObject parentObject = objectsToActivate[0];

                // �e�I�u�W�F�N�g�̂��ׂĂ̎q�������[�v�Ŏ擾���Ĕ�A�N�e�B�u�ɂ���
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[5];
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            objectsToActivate[4].SetActive(true);
        }

        else
        {
            currentTime = 0f;
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[3].SetActive(true);
            //���[���b�g�I�u�W�F�N�g�̎q����S���A�N�e�B�u�ɂ��Ă���
            {
                // �e�I�u�W�F�N�g���擾����
                GameObject parentObject = objectsToActivate[3];

                // �e�I�u�W�F�N�g�̂��ׂĂ̎q�������[�v�Ŏ擾���Ĕ�A�N�e�B�u�ɂ���
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[6];
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
            objectsToActivate[4].SetActive(true);
        }
       
    }
}
