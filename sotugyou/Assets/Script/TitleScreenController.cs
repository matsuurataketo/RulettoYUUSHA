using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{
    public Button startButton;  // �X�^�[�g�{�^����Inspector�Őݒ�

    void Start()
    {
        // �V�[�����J�n���ꂽ��X�^�[�g�{�^���Ƀt�H�[�J�X�����킹��
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ��X�^�[�g�{�^�������s
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startButton.onClick.Invoke();  // �X�^�[�g�{�^���̃N���b�N�C�x���g���Ăяo��
        }
    }
}