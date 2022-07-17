using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private UnitSo[] units;
    private int _currentUnitIndex;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = units[_currentUnitIndex].sprite;
    }

    public void SelectNextUnit()
    {
        Debug.Log("SelectNextUnit");
        _currentUnitIndex++;
        if (_currentUnitIndex >= units.Length) _currentUnitIndex = 0;
        _spriteRenderer.sprite = units[_currentUnitIndex].sprite;
    }

    public void SelectPreviousUnit()
    {
        Debug.Log("SelectPreviousUnit");
        _currentUnitIndex--;
        if (_currentUnitIndex < 0) _currentUnitIndex = units.Length - 1;
        _spriteRenderer.sprite = units[_currentUnitIndex].sprite;
    }
}