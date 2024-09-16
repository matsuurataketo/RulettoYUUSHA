using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    // PlayableDirectorをアタッチ
    public PlayableDirector playableDirector;

    void Update()
    {
        // スペースキーが押されたらタイムラインを再生
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTimeline();
        }
    }

    // タイムラインを再生するメソッド
    public void PlayTimeline()
    {
        if (playableDirector != null)
        {
            playableDirector.Play(); // タイムラインを再生
        }
    }
}