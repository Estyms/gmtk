using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
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
}