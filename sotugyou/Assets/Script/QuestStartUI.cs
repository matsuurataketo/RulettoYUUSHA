using UnityEngine;
using UnityEngine.UI;

public class QuestStartUI : MonoBehaviour
{
    public RectTransform questImage; // クエストスタート画像のRectTransform
    public Vector3 startPosition;   // 初期位置
    public Vector3 endPosition;     // 終了位置（中央）
    public Vector2 startScale = new Vector2(0.5f, 0.5f); // 初期サイズ
    public Vector2 endScale = new Vector2(1f, 1f);       // 終了サイズ
    public float animationDuration = 2f; // アニメーションの長さ
    public float displayTime = 2f;       // 中央に留まる時間
    public AudioManager audioManager;

    private float elapsedTime = 0f;

    void Start()
    {
        // 初期位置とサイズを設定
        questImage.localPosition = startPosition;
        questImage.localScale = startScale;

        // アニメーションを開始
        StartCoroutine(ShowQuestStart());
    }

    private System.Collections.IEnumerator ShowQuestStart()
    {
        // 1. 大きくして中央に移動
        yield return StartCoroutine(AnimateMoveAndScale(startPosition, endPosition, startScale, endScale, animationDuration,"あける"));

        // 2. 2秒間待機
        yield return new WaitForSeconds(displayTime);

        // 3. 元の位置とサイズに戻す
        yield return StartCoroutine(AnimateMoveAndScale(endPosition, startPosition, endScale, startScale, animationDuration, "しめる"));

        // 完了後に他の処理を追加するならここに書く
        Debug.Log("Quest Start animation complete!");
    }

    private System.Collections.IEnumerator AnimateMoveAndScale(Vector3 fromPosition, Vector3 toPosition, Vector2 fromScale, Vector2 toScale, float duration,string audiostring)
    {
        elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 位置とサイズを補間
            questImage.localPosition = Vector3.Lerp(fromPosition, toPosition, t);
            questImage.localScale = Vector3.Lerp(fromScale, toScale, t);

            yield return null;
        }
        audioManager.PlaySound(audiostring);
        // 最終位置とサイズを確定
        questImage.localPosition = toPosition;
        questImage.localScale = toScale;
    }
}