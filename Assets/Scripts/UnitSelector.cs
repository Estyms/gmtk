using System;
using ScriptableObjects;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private UnitSo[] units;
    private int _currentUnitIndex;
    private SpriteRenderer _spriteRenderer;
    private UnitSo[] _team;
    private TeamListSo _teamList;

    private void Awake()
    {
        _teamList = Resources.Load<TeamListSo>("ActualTeam");
        _team = _teamList.teamList;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = units[_currentUnitIndex].sprite;
    }

    public event EventHandler OnUnitSelected;

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

    public void SelectUnit()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_team[i] != null) continue;
            _team[i] = units[_currentUnitIndex];
            OnUnitSelected?.Invoke(this, EventArgs.Empty);
            Debug.Log("Unit selected");
            break;
        }
    }
}