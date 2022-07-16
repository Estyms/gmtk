using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameState : MonoBehaviour
{

    public enum StateEnum
    {
        Fight,
        Win = 1,
        Lose = 2
    }
    
    public event EventHandler onEndTurn;

    // Properties
    public Unit[] unitsAlly, unitsEnemy;
    private StateEnum _state = StateEnum.Fight;
    private Ally _selectedUnit;
    private bool _isMyTurn = true;
    [SerializeField] private TextMeshProUGUI _turnText;

    public void SetTurn(bool isMyTurn)
    {
        _isMyTurn = isMyTurn;
        _turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
    }

    public void EndFight()
    {
        if (_state == StateEnum.Fight) return;
        _turnText.text = _state == StateEnum.Win ? "You Win !" : "You Lose.";
    }


    private void NextState()
    {
        _state = (unitsAlly.All(ally => ally.IsDead())) ? StateEnum.Lose 
            : unitsEnemy.All(enemy => enemy.IsDead()) ? StateEnum.Win 
            : StateEnum.Fight;
    }


    public void AllyAttack(Unit enemy)
    {
        // Setup event
        onEndTurn = null;
        onEndTurn += (sender, args) =>
        {
            NextState();
            SetTurn(false);
            Debug.Log("ALLY END TURN " + _state);
        };
        
        // Attack the clicked enemy
        _selectedUnit.Attack(enemy, this);

    }

    public void EnemyAttack(Unit enemy)
    {
        Unit target = null;
        // Return if fight is done
        if (_state != StateEnum.Fight) return;
        
        // Search a target
        while (target == null)
        {
            int random = Random.Range(0, unitsAlly.Length);
            if (!unitsAlly[random].IsDead()) target = unitsAlly[random];
        }
        
        
        
        // Setup Event
        onEndTurn = null;
        onEndTurn += (sender, args) =>
        {
            if (_selectedUnit.IsDead()) _selectedUnit = null;
            // Compute next state
            NextState();
            // Give back the hand to the player
            SetTurn(true);
            Debug.Log("ENEMY END TURN " + _state);
        };
        // Attack the target
        enemy.Attack(target, this);
    }

    public void EndTurn()
    {
        onEndTurn?.Invoke(this, EventArgs.Empty);
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

    public StateEnum State => _state;
}