/// <summary>
/// Singleton class that allows coroutines to be run globally from non-MonoBehaviour scripts.
/// </summary>
/// <author> Aralyn Han Zi Ning </author>
/// <date> 11/08/2025 </date>
/// <StudentID> S10267170A </StudentID>
using UnityEngine;
using System.Collections;


public class CoroutineRunner : MonoBehaviour
{
    /// <summary>
    /// Static reference to the singleton instance.
    /// </summary>
    private static CoroutineRunner _instance;

    /// <summary>
    /// Public accessor for the singleton instance.
    /// Creates the instance if it doesn't exist and ensures it persists across scenes.
    /// </summary>
    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runner = new GameObject("CoroutineRunner");
                _instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            return _instance;
        }
    }

    /// <summary>
    /// Starts a coroutine using the singleton instance.
    /// </summary>
    /// <param name="coroutine">The coroutine to run.</param>
    /// <returns>The Coroutine object.</returns>
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
