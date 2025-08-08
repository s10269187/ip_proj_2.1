using UnityEngine;
using TMPro;
using System.Collections;

public class GhostTyping : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI textComponent;
    [TextArea] public string fullText;
    public float typingSpeed = 0.005f;

    private Coroutine typingCoroutine;

    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
