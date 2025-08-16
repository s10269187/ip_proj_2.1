using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Simulates a ghost typing effect by displaying text one character at a time in a UI text field.
/// </summary>
public class GhostTyping : MonoBehaviour
{
    /// <summary>
    /// The TextMeshProUGUI component where the text will be displayed.
    /// </summary>
    [Header("UI Settings")]
    public TextMeshProUGUI textComponent;

    /// <summary>
    /// The full message to be typed out.
    /// </summary>
    [TextArea]
    public string fullText;

    /// <summary>
    /// Delay in seconds between each character typed.
    /// </summary>
    public float typingSpeed = 0.005f;

    /// <summary>
    /// Reference to the currently running typing coroutine.
    /// </summary>
    private Coroutine typingCoroutine;

    /// <summary>
    /// Starts the ghost typing effect. Stops any previous typing coroutine if active.
    /// </summary>
    public void StartTyping()
    {
        if (typingCoroutine != null)
        {
            CoroutineRunner.Instance.StopCoroutine(typingCoroutine);
        }

        typingCoroutine = CoroutineRunner.Instance.RunCoroutine(TypeText());
    }

    /// <summary>
    /// Coroutine that types out the full text one character at a time.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator TypeText()
    {
        if (textComponent == null)
            yield break;

        textComponent.text = "";

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
