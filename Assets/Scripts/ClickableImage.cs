using UnityEngine;
using UnityEngine.UI;

public class ClickableImage : MonoBehaviour
{
    [SerializeField] private ScenesManager scenesManager;
    [SerializeField] private string sceneName;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    // OnMouseOver
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) scenesManager.LoadScene(sceneName);
    }
}