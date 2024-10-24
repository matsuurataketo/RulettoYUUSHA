using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    // ������PlayableDirector�����X�g�ŊǗ�
    [SerializeField] public List<PlayableDirector> playableDirectors;

    // �^�C�����C�����Đ�����
    public void PlayTimeline(int index)
    {
        if (index >= 0 && index < playableDirectors.Count)
        {
            playableDirectors[index].Play();
        }
        else
        {
            Debug.LogWarning("Invalid Timeline index");
        }
    }

    // �^�C�����C�����~����
    public void StopTimeline(int index)
    {
        if (index >= 0 && index < playableDirectors.Count)
        {
            playableDirectors[index].Stop();
        }
        else
        {
            Debug.LogWarning("Invalid Timeline index");
        }
    }

    // �^�C�����C�����ꎞ��~����
    public void PauseTimeline(int index)
    {
        if (index >= 0 && index < playableDirectors.Count)
        {
            playableDirectors[index].Pause();
        }
        else
        {
            Debug.LogWarning("Invalid Timeline index");
        }
    }
}
