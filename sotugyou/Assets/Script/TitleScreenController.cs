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
            NavigateToButton(0); // 1�̃{�^���Ɉړ�
        }
        else if (scroll < 0f) // �������ɃX�N���[��
        {
            NavigateToButton(1); // 0�̃{�^���Ɉړ�
        }

        // �X�y�[�X�L�[�Ō��ݑI������Ă���{�^��������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }

    void NavigateToButton(int buttonIndex)
    {
        // �w�肳�ꂽ�{�^���Ɉړ�
        if (buttonIndex >= 0 && buttonIndex < buttons.Length)
        {
            currentButtonIndex = buttonIndex;
            EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
        }
    }
}