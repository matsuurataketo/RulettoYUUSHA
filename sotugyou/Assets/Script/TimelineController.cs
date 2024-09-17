using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    // PlayableDirectorをアタッチ
    public PlayableDirector playableDirector;

    // タイムラインを再生するメソッド
    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play(); // タイムラインを再生
        }
    }
}