using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI DialogText;
    [SerializeField] private GameObject button;
    [TextArea(5, 5)]
    [SerializeField] private string msgText;
    private float msgSpeed = 0.1f;
    void Start()
    {
        DialogText.text = "";
        button.SetActive(false);
        StartCoroutine(TypeDisplay());
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StopAllCoroutines();
            DialogText.text = msgText;
            button.SetActive(true);
        }
    }
    IEnumerator TypeDisplay()
    {
        foreach (char item in msgText.ToCharArray())
        {
            DialogText.text += item;
            yield return new WaitForSeconds(msgSpeed);
        }
        button.SetActive(true);
    }
}
