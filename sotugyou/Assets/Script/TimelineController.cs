using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    // PlayableDirector���A�^�b�`
    public PlayableDirector playableDirector;

    // �^�C�����C�����Đ����郁�\�b�h
    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play(); // �^�C�����C�����Đ�
        }
    }
}