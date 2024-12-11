using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoulettoGimick : MonoBehaviour
{
    private enum Direction { Right, Left }

    [Header("���[���b�g�̃X�s�[�h")] public float rotationSpeed = 100f;
    private Direction currentDirection;
    public float CrearNum = 0;

    [Header("�~�j�Q�[���̎���")] public float timeLimit = 5f;
    private float timer = 0f;
    [Header("�Q�[�������ۂ�")] public bool gameActive = false;
    private float totalRotation = 0f;

    [SerializeField, Header("�C���X�^���X������Image�̃v���n�u")] private Image imagePrefab;
    [SerializeField, Header("Image���i�[����e�I�u�W�F�N�g")] private Transform imageParent;
    [SerializeField, Header("�E�����̉摜")] private Sprite rightImage;
    [SerializeField, Header("�������̉摜")] private Sprite leftImage;
    [SerializeField, Header("�f�o�C�X�摜")] GameObject dviceimag;

    private Image displayedImage; // �\�������摜
    AudioManager audioManager;
    MinigameEfect MinigameEfect;

    private void Start()
    {
        MinigameEfect = FindObjectOfType<MinigameEfect>();
    }

    public void InitializeGame()
    {
        totalRotation = 0f;
        timer = 0f;

        if (displayedImage != null)
        {
            Destroy(displayedImage.gameObject);
        }

        // �V�����摜��1����
        displayedImage = Instantiate(imagePrefab, imageParent);
    }

    public void StartRouletteGame()
    {
        dviceimag.transform.rotation = Quaternion.Euler(0, 0, 0);
        audioManager = FindObjectOfType<AudioManager>();
        InitializeGame();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        if (!gameActive)
        {
            CrearNum = 0;
            UpdateDirection(); // �ŏ��̕�����ݒ�
            gameActive = true;
            timer = 0f;
        }

        yield return new WaitForSeconds(timeLimit);

        // �������Ԃ��߂����ꍇ�̏���
        if (gameActive)
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

            // �������ԂɒB������Q�[���I��
            if (timer >= timeLimit)
            {
                Debug.Log("���Ԑ؂�I");
                gameActive = false;
                return;
            }

            // �}�E�X�z�C�[�����͂̏���
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                float step = rotationSpeed * Time.deltaTime * Mathf.Abs(scrollInput);

                if (scrollInput < 0 && currentDirection == Direction.Right)
                {
                    // �E��]
                    transform.Rotate(0, 0, -step);
                    totalRotation += step;
                }
                else if (scrollInput > 0 && currentDirection == Direction.Left)
                {
                    // ����]
                    transform.Rotate(0, 0, step);
                    totalRotation += step;
                }

                // 1��]�i360�x�j�������ǂ������`�F�b�N
                if (totalRotation >= 90f)
                {
                    totalRotation = 0f;
                    CrearNum++;
                    audioManager.PlaySound("����");
                    Debug.Log("�����������Ɉ��]�����I");
                    MinigameEfect.PlayParticle();

                    // ���̕����𒊑I���čX�V
                    UpdateDirection();
                    dviceimag.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    private void UpdateDirection()
    {
        // �V���������𒊑I
        currentDirection = (Direction)Random.Range(0, 2);

        // �\�����X�V
        displayedImage.sprite = currentDirection == Direction.Right ? rightImage : leftImage;

        Debug.Log($"���̕���: {currentDirection}");
    }
}