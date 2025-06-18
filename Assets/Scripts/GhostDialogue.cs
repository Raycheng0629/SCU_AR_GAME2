using UnityEngine;
using TMPro;
using System.Collections;
using RAY;


public class GhostDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [TextArea(2, 5)]
    public string[] dialogueLines;

    private int currentLine = 0;
    private bool isPlaying = false;

    void OnMouseDown()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayDialogueCoroutine());
        }
    }

    IEnumerator PlayDialogueCoroutine()
    {
        isPlaying = true;
        dialoguePanel.SetActive(true);

        while (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
            currentLine++;
            yield return new WaitForSeconds(2f); // 每 2 秒播一行
        }

        dialoguePanel.SetActive(false);
        FindObjectOfType<UIManager>().ShowQuestionPanel();
    }
}
