using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Needed for scene switching

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textspeed;

    private int index;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click to advance dialogue
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines(); // Skip typing effect
                dialogueText.text = lines[index]; // Show full line
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textspeed);
        }
    }

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
            gameObject.SetActive(false); // Hide dialogue box
            SceneManager.LoadScene("Endscene"); // Load next scene
        }
    }
}
