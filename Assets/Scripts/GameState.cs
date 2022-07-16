using System.Linq;
using TMPro;
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
        // Attack the clicked enemy
        _selectedUnit.Attack(enemy);
        
        // Compute the next state
        NextState();
        
        // Give hand to the enemy
        SetTurn(false);
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
        
        // Attack the target
        enemy.Attack(target);
        
        // Compute next state
        NextState();
        
        // Give back the hand to the player
        SetTurn(true);
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