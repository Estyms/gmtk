using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Properties
    public Unit[] unitsAlly, unitsEnemy;
    private Ally _selectedUnit;
    private bool _isMyTurn = true;
    [SerializeField] private TextMeshProUGUI _turnText;

    public void SetTurn(bool isMyTurn)
    {
        _isMyTurn = isMyTurn;
        _turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
    }


    public void Attack(Unit enemy)
    {
        _selectedUnit.Attack(enemy);
        SetTurn(false);
    }
    
    
    //Setters
    public Ally SelectedUnit
    {
        get => _selectedUnit;
        set
        {
            if (_selectedUnit) _selectedUnit.Effect.enabled = false;
            _selectedUnit = value;
            _selectedUnit.Effect.enabled = true;
        }
    }

    // Getters
    public Unit[] UnitsAlly => unitsAlly;

    public Unit[] UnitsEnemy => unitsEnemy;
    public bool IsMyTurn => _isMyTurn;
}