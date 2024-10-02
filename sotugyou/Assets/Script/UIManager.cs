using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField,Header("�J�E���g�_�E���̉摜")] Image countDownImage;
    [SerializeField,Header("�J�E���g�_�E���e�L�X�g")] TextMeshProUGUI countDownText;
    int countDownCount;
    float countDownElapsedTime;
    float countDownDuration = 5.0f;
    // Start is called before the first frame update
    public AudioSource bgmSource;  // BGM�p��AudioSource

    public void StartCountDown()
    {
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        bgmSource.loop = true;     // ���[�v�Đ�
        bgmSource.Play();          // �Đ��J�n
        countDownCount = 0;
        countDownElapsedTime = 0;


        //�e�L�X�g�̍X�V�B
        countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration));

        //�����A���דI�ɂ�GameObject�ւ̎Q�Ƃ͕ʂɕێ����Ă��������X�������Ǝv���������B
        countDownImage.gameObject.SetActive(true);
        countDownText.gameObject.SetActive(true);


        while (true)
        {
            countDownElapsedTime += Time.deltaTime;

            //�~�`�X���C�_�[�̍X�V�BfillAmount��0�`1.0f�̊ԂŎw�肷��B�o�ߎ��Ԃ̏����_�ȉ��̒l�����Ă���B
            countDownImage.fillAmount = countDownElapsedTime % 1.0f;

            if (countDownCount < Mathf.FloorToInt(countDownElapsedTime))
            {
                //1�b���݂ŃJ�E���g�B
                countDownCount++;
                //�e�L�X�g�̍X�V�B
                countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration - countDownCount));
            }

            if (countDownDuration <= countDownElapsedTime)
            {
                //�J�E���g�_�E���I���B

                countDownImage.gameObject.SetActive(false);
                countDownText.gameObject.SetActive(false);
                bgmSource.Stop();
                yield break;
            }

            yield return null;
        }
    }
}
