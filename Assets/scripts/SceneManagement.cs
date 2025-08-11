using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void OnStartClick()
    {
    SceneManager.LoadScene("GameScene"); // Replace "GameScene" with your scene name
    }
    public void OnInstructionButton ()
    {
        SceneManager.LoadScene(2);
    }
    
}