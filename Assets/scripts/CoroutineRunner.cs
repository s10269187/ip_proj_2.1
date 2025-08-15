using UnityEngine; 
using System.Collections; 

// Singleton class to run coroutines globally
public class CoroutineRunner : MonoBehaviour
{
    // Static reference to the singleton instance
    private static CoroutineRunner _instance;
    // Public accessor for the singleton
    public static CoroutineRunner Instance
    {
        get
        {
            // If instance doesn't exist
            if (_instance == null)
            {
                // Create new GameObject
                // Attach this script
                // Persist across scenes
                GameObject runner = new GameObject("CoroutineRunner");
                _instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            // Return the instance
            return _instance;
        }
    }
    // Method to start a coroutine
    // Start and return coroutine
    public Coroutine RunCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
