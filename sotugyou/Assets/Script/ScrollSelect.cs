using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSelect : MonoBehaviour
{
    [Header("左右のボタン")]public Button[] buttons; // 左右のボタン
    [Header("SetActivの切り替え")]public GameObject[] objectsToActivate;
    [SerializeField,Header("矢印のSetActivの切り替え")] GameObject[] RoulettoYazirusi;
    public int selectedIndex = 0; // 現在の選択インデックス
    public float selectionTime = 10f; // 選択時間
    [Header("現在の経過時間")]public bool currentFrag = true; // 現在の経過時間
    UIManager uiManager;
    public RouletteMaker KougekirMaker;
    public RouletteMaker KaihukurMaker;
    UIListController uilistcontroller;


    void Start()
    {
        uilistcontroller = FindObjectOfType<UIListController>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        // 最初のボタンを選択状態にする
        SelectButton(selectedIndex);
        //uiManager.StartCountDown();
        
    }

    void Update()
    {
        // マウススクロールの入力を受け取る
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // スクロール方向によって選択を変更する
        if (scroll > 0 && buttons[0].gameObject.activeSelf)
        {
            selectedIndex = 0;
        }
        else if (scroll < 0 && buttons[1].gameObject.activeSelf)
        {
            selectedIndex = 1;
        }

        // 選択されたボタンを更新する
        SelectButton(selectedIndex);

        // 選択が決定されるまでの時間をカウント
        //currentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)&& currentFrag)
        {
            // 選択を決定する処理をここに追加する
            buttons[selectedIndex].onClick.Invoke();
            Debug.Log("Button " + selectedIndex + " selected!");
        }
        if (!objectsToActivate[1].activeSelf)
            currentFrag = false;

        //Debug.Log(currentTime);
    }

    // 指定したインデックスのボタンを選択状態にする
    void SelectButton(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            // インデックスが一致するボタンを選択状態にする
            buttons[i].interactable = (i == index);
        }
    }
    public void ActivateObjects()
    {
        if (selectedIndex == 0)
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySound("ルーレット決定SE");
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[0].SetActive(true);

            RoulettoYazirusi[0].SetActive(true);
            //ルーレットオブジェクトの子供を全部アクティブにしている
            {
                // 親オブジェクトを取得する
                GameObject parentObject = objectsToActivate[0];

                // 親オブジェクトのすべての子供をループで取得してアクティブにする
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[4];
                parentObject.SetActive(true);
                Debug.Log("呼び出しています");
                KaihukurMaker.RuleetSet();

                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
                if (KaihukurMaker.randomGame == 0)
                    uilistcontroller.ToggleSpecificImage(1);
                if (KaihukurMaker.randomGame == 1)
                    uilistcontroller.ToggleSpecificImage(0);
            }
        }

        else
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();
            audioManager.PlaySound("ルーレット決定SE");
            objectsToActivate[1].SetActive(false);
            objectsToActivate[2].SetActive(false);
            objectsToActivate[3].SetActive(true);

            RoulettoYazirusi[0].SetActive(true);
            //ルーレットオブジェクトの子供を全部アクティブにしている
            {
                // 親オブジェクトを取得する
                GameObject parentObject = objectsToActivate[3];

                // 親オブジェクトのすべての子供をループで取得して非アクティブにする
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }

                parentObject = objectsToActivate[5];
                parentObject.SetActive(true);
                KougekirMaker.RuleetSet();

                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                }
                if (KougekirMaker.randomGame == 0)
                    uilistcontroller.ToggleSpecificImage(1);
                if (KougekirMaker.randomGame == 1)
                    uilistcontroller.ToggleSpecificImage(0);
            }
        }
       
    }
}
