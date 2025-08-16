/// <summary>
/// Displays a sequence of dialogue lines with a typing effect.
/// Advances on mouse click and transitions to a new scene when complete.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 15/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    /// <summary>
    /// The UI text component used to display dialogue.
    /// </summary>
    public TextMeshProUGUI dialogueText;

    /// <summary>
    /// Array of dialogue lines to display.
    /// </summary>
    public string[] lines;

    /// <summary>
    /// Typing speed in seconds per character.
    /// </summary>
    public float textspeed;

    /// <summary>
    /// Index of the current dialogue line.
    /// </summary>
    private int index;

    /// <summary>
    /// Initializes the dialogue system by clearing the text and starting the dialogue.
    /// </summary>
    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    /// <summary>
    /// Handles mouse input to advance or complete the current dialogue line.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    /// <summary>
    /// Starts the dialogue sequence from the first line.
    /// </summary>
    public void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    /// <summary>
    /// Coroutine that types out the current line character by character.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    IEnumerator TypeLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textspeed);
        }
    }

    /// <summary>
    /// Advances to the next line of dialogue or ends the sequence if finished.
    /// </summary>
    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene("Endscene");
        }
    }
}
