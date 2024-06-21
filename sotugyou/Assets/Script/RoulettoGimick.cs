using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoulettoGimick : MonoBehaviour
{
    private enum Direction { Right, Left }

    public float rotationSpeed = 100f; // ��]���x
    private List<Direction> directions = new List<Direction>();
    private int currentStep = 0;
    public float timeLimit = 5f;
    private float timer = 0f;
    public bool gameActive = false;
    private float totalRotation = 0f;
    [SerializeField] private Image imagePrefab; // �C���X�^���X������Image�̃v���n�u
    [SerializeField] private Transform imageParent; // Image���i�[����e�I�u�W�F�N�g
    [SerializeField] private Sprite rightImage; // �E�����̉摜
    [SerializeField] private Sprite leftImage;  // �������̉摜

    private List<Image> instantiatedImages = new List<Image>(); // �C���X�^���X�����ꂽ�摜�̃��X�g

    public void InitializeGame()
    {
        directions.Clear();
        currentStep = 0;
        totalRotation = 0f;
        timer = 0f;

        // �摜�̏������ƍ폜
        foreach (var img in instantiatedImages)
        {
            Destroy(img.gameObject);
        }
        instantiatedImages.Clear();
    }

    public void StartRouletteGame()
    {
        InitializeGame();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (!gameActive)
        {
            float initialPositionX = -300f; // �����ʒu
            float offsetX = 150f; // �I�t�Z�b�g

            // �����_����5��̕����𒊑I
            for (int i = 0; i < 5; i++)
            {
                directions.Add((Direction)Random.Range(0, 2));

                // �摜���C���X�^���X��
                Image newImage = Instantiate(imagePrefab, imageParent);
                newImage.sprite = directions[i] == Direction.Right ? rightImage : leftImage;
                instantiatedImages.Add(newImage);

                // �ʒu��ݒ�
                RectTransform rectTransform = newImage.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(initialPositionX + i * offsetX, 0);
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