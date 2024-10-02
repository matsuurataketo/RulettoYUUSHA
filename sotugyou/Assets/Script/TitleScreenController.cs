using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
        public Button[] buttons;  // UI��̃{�^����Inspector�Őݒ�
    private int currentButtonIndex = 0;

    void Start()
    {
        // �ŏ��̃{�^����I��
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    void Update()
    {
        // �}�E�X�z�C�[�������m���ă{�^���Ԃ��ړ�
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // ������ɃX�N���[��
        {
            NavigateToPreviousButton();
        }
        else if (scroll < 0f) // �������ɃX�N���[��
        {
            NavigateToNextButton();
        }

        // �X�y�[�X�L�[�Ō��ݑI������Ă���{�^��������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }

    void NavigateToNextButton()
    {
        // ���̃{�^���Ɉړ�
        currentButtonIndex = (currentButtonIndex + 1) % buttons.Length;
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    void NavigateToPreviousButton()
    {
        // �O�̃{�^���Ɉړ�
        currentButtonIndex--;
        if (currentButtonIndex < 0)
        {
            currentButtonIndex = buttons.Length - 1;
        }
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }
}