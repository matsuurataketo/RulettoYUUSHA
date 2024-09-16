using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    // PlayableDirector���A�^�b�`
    public PlayableDirector playableDirector;

    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ��^�C�����C�����Đ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTimeline();
        }
    }

    // �^�C�����C�����Đ����郁�\�b�h
    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play(); // �^�C�����C�����Đ�
        }
    }
}