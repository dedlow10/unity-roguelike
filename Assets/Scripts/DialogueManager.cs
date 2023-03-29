using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    public static DialogueManager instance { get; private set; }

    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] GameObject dialoguePanel;

    public bool isTextShowing = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {

            instance = this;
        }
    }

    public void ShowText(string text)
    {
        isTextShowing = true;
        StartCoroutine(DelayText(text, .5f));
    }

    public void ShowTextImmediate(string text)
    {
        isTextShowing = true;
        textDisplay.text = text;
        dialoguePanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space hit");
            if (Time.timeScale > 0)
            {
                StartCoroutine(ReturnToGame(.2f));
            }
            else
            {
                dialoguePanel.SetActive(false);
                isTextShowing = false;
            }
        }
    }


    IEnumerator ReturnToGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dialoguePanel.SetActive(false);
        isTextShowing = false;
    }

    IEnumerator DelayText(string text, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textDisplay.text = text;
        dialoguePanel.SetActive(true);
    }
}
