using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoulettoGimick : MonoBehaviour
{
    private enum Direction { Right, Left }

    public float rotationSpeed = 100f; // ��]���x
    private List<Direction> directions = new List<Direction>();
    private int currentStep = 0;
    private float timeLimit = 10f;
    private float timer = 0f;
    public bool gameActive = false;
    private float totalRotation = 0f;

    public void InitializeGame()
    {
        directions.Clear();
        currentStep = 0;
        totalRotation = 0f;
        timer = 0f;
    }

    public void StartRouletteGame()
    {
        InitializeGame();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (gameActive == false)
        {
            // �����_����5��̕����𒊑I
            for (int i = 0; i < 5; i++)
            {
                directions.Add((Direction)Random.Range(0, 2));
            }
            // �f�o�b�O�p�ɒ��I���ʂ����O�ɕ\��
            foreach (var dir in directions)
            {
                Debug.Log(dir);
            }
            gameActive = true;
            timer = 0f;
        }
        yield return new WaitForSeconds(timeLimit);

        // �������Ԃ��߂����ꍇ�̏���
        if (currentStep < directions.Count)
        {
            Debug.Log("���Ԑ؂�I");
            gameActive = false;
        }
    }

    void Update()
    {
        if (gameActive)
        {
            timer += Time.deltaTime;

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                float step = rotationSpeed * Time.deltaTime * Mathf.Abs(scrollInput);

                if (scrollInput < 0 && directions[currentStep] == Direction.Right)
                {
                    // �E��]
                    transform.Rotate(0, 0, -step);
                    totalRotation += step;
                }
                else if (scrollInput > 0 && directions[currentStep] == Direction.Left)
                {
                    // ����]
                    transform.Rotate(0, 0, step);
                    totalRotation += step;
                }

                // 1��]�i360�x�j�������ǂ������`�F�b�N
                if (totalRotation >= 360f)
                {
                    totalRotation = 0f;
                    currentStep++;
                    Debug.Log("�����������Ɉ��]�����I");

                    // ���̃X�e�b�v�ɐi��
                    if (currentStep >= directions.Count)
                    {
                        // ���ׂẴX�e�b�v���N���A�����ꍇ
                        BothRotationsCompleted();
                    }
                }
            }
        }
    }

    private void BothRotationsCompleted()
    {
        Debug.Log("�N���A�I");
        gameActive = false;
        // �����ɃN���A���̏�����ǉ�
        // ��F SomeFunction();
    }
}