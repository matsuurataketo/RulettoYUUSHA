using UnityEngine;

public class ArcJump : MonoBehaviour
{
    public float jumpHeight = 2f;     // ���˂鍂��
    public float speed = 1f;         // �ړ����x
    public int jumpDistance = 30;    // ���˂鋗��
    public int jumpCount = 3;        // ���˂��

    private Vector3[] targets;       // �ڕW�n�_���X�g
    private int currentTargetIndex = 0;
    private float timeElapsed = 0f;
    private EncountMove encountMove;
    public LodeScene lodeScene;
    public string targetSceneName = "MaingameScene 1"; // �J�ڐ�̃V�[����
    public GameObject BikriImag;
    public AudioManager audioManager;
    private bool flag1;

    void Start()
    {
        flag1 = false;
        currentTargetIndex = 0;
        timeElapsed = 0f;

        encountMove = FindObjectOfType<EncountMove>();
        // ���˂�ڕW�n�_���v�Z
        targets = new Vector3[jumpCount + 1];
        targets[0] = transform.position; // �����ʒu
        for (int i = 1; i <= jumpCount; i++)
        {
            targets[i] = targets[i - 1] + new Vector3(jumpDistance, 0, 0); // X������90����
        }
    }

    void Update()
    {
        if (currentTargetIndex < jumpCount&&encountMove.enemywork==true) // ���̖ڕW�n�_������ꍇ
        {
            // ���Ԍo�߂ɉ����Đi�s�������v�Z
            timeElapsed += Time.deltaTime * speed;
            float t = Mathf.Clamp01(timeElapsed); // 0�`1�ɐ���

            // ���݂̖ڕW�n�_�Ɍ����������ʒu����
            Vector3 horizontalPosition = Vector3.Lerp(targets[currentTargetIndex], targets[currentTargetIndex + 1], t);

            // ���������̕������v�Z
            float arcHeight = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            // �V�����ʒu��ݒ�
            transform.position = horizontalPosition + new Vector3(0, arcHeight, 0);

            // ���̖ڕW�n�_�ɓ��B�����烊�Z�b�g
            if (t >= 1f)
            {
                timeElapsed = 0f;
                currentTargetIndex++;
                audioManager.PlaySound("�X���C�����˂�");
            }
        }
        if(currentTargetIndex==3&&flag1==false)
        {
            flag1 = true;
            audioManager.PlaySound("�G���J�E���g");
            BikriImag.SetActive(false);
            StartCoroutine(lodeScene.FadeOut(targetSceneName));
        }
    }
}