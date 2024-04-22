using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Load the scene with the given name
        SceneManager.LoadScene(sceneName);
    }
}
