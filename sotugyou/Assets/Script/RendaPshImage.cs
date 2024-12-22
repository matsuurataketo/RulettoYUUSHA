using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RendaPshImage : MonoBehaviour
{
    public Image image1; // �ŏ���Image
    public Image image2; // �؂�ւ����Image
    public float switchDuration = 0.1f; // �؂�ւ��Ԋu

    void Update()
    {
        // �X�y�[�X�L�[����������A�j���[�V�����J�n
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SwitchImages());
        }
    }

    private System.Collections.IEnumerator SwitchImages()
    {
        image1.enabled = false; // Image1���I��/�I�t
        image2.enabled = true; // Image2���I��/�I�t
        yield return new WaitForSeconds(switchDuration); // �w�莞�ԑ҂�

        // �Ō�Ɍ��̏�Ԃɖ߂�
        image1.enabled = true;
        image2.enabled = false;
    }
}
