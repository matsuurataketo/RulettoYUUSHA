using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    // 複数のPlayableDirectorをリストで管理
    [SerializeField] public List<PlayableDirector> playableDirectors;

    // タイムラインを再生する
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

    // タイムラインを停止する
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

    // タイムラインを一時停止する
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
