using UnityEngine;
using UnityEngine.UI;

public class QuestStartUI : MonoBehaviour
{
    public RectTransform questImage; // �N�G�X�g�X�^�[�g�摜��RectTransform
    public Vector3 startPosition;   // �����ʒu
    public Vector3 endPosition;     // �I���ʒu�i�����j
    public Vector2 startScale = new Vector2(0.5f, 0.5f); // �����T�C�Y
    public Vector2 endScale = new Vector2(1f, 1f);       // �I���T�C�Y
    public float animationDuration = 2f; // �A�j���[�V�����̒���
    public float displayTime = 2f;       // �����ɗ��܂鎞��
    public AudioManager audioManager;

    private float elapsedTime = 0f;

    void Start()
    {
        // �����ʒu�ƃT�C�Y��ݒ�
        questImage.localPosition = startPosition;
        questImage.localScale = startScale;

        // �A�j���[�V�������J�n
        StartCoroutine(ShowQuestStart());
    }

    private System.Collections.IEnumerator ShowQuestStart()
    {
        // 1. �傫�����Ē����Ɉړ�
        yield return StartCoroutine(AnimateMoveAndScale(startPosition, endPosition, startScale, endScale, animationDuration,"������"));

        // 2. 2�b�ԑҋ@
        yield return new WaitForSeconds(displayTime);

        // 3. ���̈ʒu�ƃT�C�Y�ɖ߂�
        yield return StartCoroutine(AnimateMoveAndScale(endPosition, startPosition, endScale, startScale, animationDuration, "���߂�"));

        // ������ɑ��̏�����ǉ�����Ȃ炱���ɏ���
        Debug.Log("Quest Start animation complete!");
    }

    private System.Collections.IEnumerator AnimateMoveAndScale(Vector3 fromPosition, Vector3 toPosition, Vector2 fromScale, Vector2 toScale, float duration,string audiostring)
    {
        elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // �ʒu�ƃT�C�Y����
            questImage.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
            questImage.localScale = Vector3.Lerp(fromScale, toScale, t);

            yield return null;
        }
        audioManager.PlaySound(audiostring);
        // �ŏI�ʒu�ƃT�C�Y���m��
        questImage.localPosition = toPosition;
        questImage.localScale = toScale;
    }
}