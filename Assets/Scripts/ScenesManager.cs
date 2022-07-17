using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // don't destroy on load
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    // load scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}