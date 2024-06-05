using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WeponRouletto : MonoBehaviour
{
    [HideInInspector] public GameObject roulette;
    [HideInInspector] public float rotatePerRoulette;
    [HideInInspector] public WeponRouletteMaker rMaker;

    public static string result;//���[���b�g�̌��ʂ̊i�[�ϐ�
    [SerializeField] private TextMeshProUGUI resultText;//���ʂ̕\��TEXT
    public float rotationSpeed = 5.0f;//���[���b�g�̉�]�X�s�[�h
    private float lastScrollWheelInputTime; // �Ō�Ƀ}�E�X�X�N���[���z�C�[���̓��͂�����������
    public float stopThreshold = 1.0f; // ���[���b�g����~�����Ƃ݂Ȃ�臒l�i�b
    private bool ScrollWheel = false;//�ŏ��̃}�E�X�z�C�[������ϐ�
    [SerializeField] private float rouletteSpeed; // ���[���b�g�̑��x��ێ�����ϐ�
    public float RouletteSpeed => rouletteSpeed; // �v���p�e�B����ĊO������A�N�Z�X�ł���悤�ɂ���
    private Quaternion previousRotation;//���[���b�g�̂���]�̕ϐ�
    private int frameCount = 0;//���n�߂Ă���̃t���[���J�E���^�[
    private int comparisonInterval = 210; // ��r�Ԋu
    [SerializeField] GameObject WponsRoulrtto;//�������߂̃V�[���Ŏg�p

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)     // �}�E�X�z�C�[�����񂳂ꂽ�ꍇ
            ScrollWheel = true; // �t���O�����낵�āA�ȍ~�̏��������s�\�ɂ���

        rouletteSpeed = Input.GetAxis("Mouse ScrollWheel") * rotationSpeed; // ���[���b�g�̑��x���X�V����
        roulette.transform.Rotate(Vector3.forward, rouletteSpeed, Space.World);

        if (frameCount % comparisonInterval == 0 && ScrollWheel == true)
        {
            if (Quaternion.Angle(roulette.transform.rotation, previousRotation) == 0f && ScrollWheel == true)
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
            //����I�����[���b�g
            case "ken":
                //���킪���̂Ƃ�
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                Weit();
                SceneManager.LoadScene("MaingameScene");
                break;
            case "yari":
                //���킪���̂Ƃ�
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                Weit();
                SceneManager.LoadScene("MaingameScene");
                break;
            case "kobusi":
                //���킪���̂Ƃ�
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                break;
            case "tue":
                //���킪��̂Ƃ�
                ShowWeponRouletto(true, false, "\nWponsRoulrtto:");
                break;

            default:
                break;
        }
    }

    private void ShowWeponRouletto(bool activ, bool notactiv, string HitText)
    {
        resultText.text = resultText.text + HitText + result + "�I���I";
    }
    IEnumerator Weit()
    {
        yield return new WaitForSeconds(5.0f);
        //�x�点��������
    }
}
