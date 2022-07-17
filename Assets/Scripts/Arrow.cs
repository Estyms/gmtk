using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private UnitSelector unitSelector;
    [SerializeField] private bool nextUnit;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (nextUnit)
            unitSelector.SelectNextUnit();
        else
            unitSelector.SelectPreviousUnit();
    }

    private void OnMouseEnter()
    {
        _spriteRenderer.color = color;
    }

    private void OnMouseExit()
    {
        _spriteRenderer.color = Color.white;
    }
}