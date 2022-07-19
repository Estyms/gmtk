using System;
using System.Linq;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    [SerializeField] private Button selectButton, buyButton;
    [SerializeField] private TextMeshProUGUI costText;

    private int _currentUnitIndex;

    private PlayerStatsSo _playerStats;
    private SpriteRenderer _spriteRenderer;
    private UnitSo[] _team;
    private TeamListSo _teamList;
    private UnitSo[] _units;

    private void Awake()
    {
        _units = Resources.LoadAll<UnitSo>("Ally");
        _teamList = Resources.Load<TeamListSo>("ActualTeam");
        _playerStats = Resources.Load<PlayerStatsSo>("PlayerStats");
        _team = _teamList.teamList;
    }

    private void Start()
    {
        costText.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        Display();
    }

    public event EventHandler OnUnitSelected;

    public void SelectNextUnit()
    {
        Debug.Log("SelectNextUnit");
        _currentUnitIndex++;
        if (_currentUnitIndex >= _units.Length) _currentUnitIndex = 0;
        Display();
    }

    public void SelectPreviousUnit()
    {
        Debug.Log("SelectPreviousUnit");
        _currentUnitIndex--;
        if (_currentUnitIndex < 0) _currentUnitIndex = _units.Length - 1;
        Display();
    }

    public void SelectUnit()
    {
        for (int i = 0; i < 3; i++)
        {
            if (_team[i] != null) continue;
            _team[i] = _units[_currentUnitIndex];
            OnUnitSelected?.Invoke(this, EventArgs.Empty);
            Debug.Log("Unit selected");
            break;
        }
    }

    public void Buy()
    {
        _playerStats.gold -= _units[_currentUnitIndex].cost;
        _playerStats.unlockedAllies.Add(_units[_currentUnitIndex]);
        Display();
    }

    private void Display()
    {
        _spriteRenderer.sprite = _units[_currentUnitIndex].sprite;
        _spriteRenderer.color = Color.black;
        costText.text = _units[_currentUnitIndex].cost.ToString();
        costText.gameObject.SetActive(true);
        buyButton.gameObject.SetActive(true);
        selectButton.gameObject.SetActive(false);

        if (_playerStats.unlockedAllies.Any(unitSo => unitSo.name == _units[_currentUnitIndex].name))
        {
            _spriteRenderer.color = Color.white;
            selectButton.gameObject.SetActive(true);
            costText.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
        }
    }
}