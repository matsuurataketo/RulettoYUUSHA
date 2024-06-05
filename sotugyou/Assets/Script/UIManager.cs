using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image countDownImage;
    [SerializeField] TextMeshProUGUI countDownText;
    int countDownCount;
    float countDownElapsedTime;
    float countDownDuration = 5.0f;
    // Start is called before the first frame update

    public void StartCountDown()
    {
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown()
    {
        countDownCount = 0;
        countDownElapsedTime = 0;


        //テキストの更新。
        countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration));

        //多分、負荷的にはGameObjectへの参照は別に保持していた方が宜しいかと思うが割愛。
        countDownImage.gameObject.SetActive(true);
        countDownText.gameObject.SetActive(true);


        while (true)
        {
            countDownElapsedTime += Time.deltaTime;

            //円形スライダーの更新。fillAmountは0～1.0fの間で指定する。経過時間の小数点以下の値を入れている。
            countDownImage.fillAmount = countDownElapsedTime % 1.0f;

            if (countDownCount < Mathf.FloorToInt(countDownElapsedTime))
            {
                //1秒刻みでカウント。
                countDownCount++;
                //テキストの更新。
                countDownText.text = System.String.Format("{0}", Mathf.FloorToInt(countDownDuration - countDownCount));
            }

            if (countDownDuration <= countDownElapsedTime)
            {
                //カウントダウン終了。

                countDownImage.gameObject.SetActive(false);
                countDownText.gameObject.SetActive(false);

                yield break;
            }

            yield return null;
        }
    }
}
