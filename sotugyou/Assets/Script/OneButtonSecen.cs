using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OneButtonSecen : MonoBehaviour
{
    public Button[] buttons;  // UI��̃{�^����Inspector�Őݒ�
    private int currentButtonIndex = 0;
    public float StartButtonDely = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[currentButtonIndex].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        StartButtonDely++;
        // �X�y�[�X�L�[�Ō��ݑI������Ă���{�^��������
        if (Input.GetKeyDown(KeyCode.Space) && StartButtonDely > 30)
        {
            StartButtonDely = 0;
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }
}
