using UnityEngine;

public class ClickableImage : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private ScenesManager _scenesManager;

    private void Start()
    {
        _scenesManager = FindObjectOfType<ScenesManager>();
    }

    // OnMouseOver
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) _scenesManager.LoadScene(sceneName);
    }
}