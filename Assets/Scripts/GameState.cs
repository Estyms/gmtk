using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;


public class GameState : MonoBehaviour
{
    public enum StateEnum
    {
        Fight,
        Win = 1,
        Lose = 2,
        Loading = 3,
    }

    public class EnemiesGeneratedArgs : EventArgs
    {
        private Unit[] enemies;

        public EnemiesGeneratedArgs(Unit[] enemies)
        {
            this.enemies = enemies;
        }

        public Unit[] Enemies => enemies;
    }

    public event EventHandler onEndTurn;
    public event EventHandler<EnemiesGeneratedArgs> onEnemiesGenerated;

    // Properties
    [SerializeField] private Unit[] unitsAlly;
    private Unit[] _unitsEnemy = {};
    [SerializeField] private FightLoader fightLoader;
    [SerializeField] private SpeedManager speedManager;
    private StateEnum _state = StateEnum.Fight;
    private Ally _selectedUnit;
    private bool _isMyTurn = true;
    [SerializeField] private TextMeshProUGUI _turnText;

    public void Awake()
    {
        _state = StateEnum.Loading;
        onEnemiesGenerated = null;
        onEnemiesGenerated += (_, args) =>
        {
            _unitsEnemy = args.Enemies;
            _state = StateEnum.Fight;
            SetTurn(true);
            speedManager.InitFight(this);
        };
        fightLoader.NextFight(this);
        foreach (var unit in _unitsEnemy)
        {
            Debug.Log(unit.name);
        }
    }

    public void SetTurn(bool isMyTurn)
    {
        _isMyTurn = isMyTurn;
        _turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
    }

    public void EndFight()
    {
        if (_state == StateEnum.Fight) return;
        switch (_state)
        {
            case StateEnum.Win:
                _state = StateEnum.Loading;
                onEnemiesGenerated = null;
                onEnemiesGenerated += (_, args) =>
                {
                    _unitsEnemy = args.Enemies;
                    _state = StateEnum.Fight;
                    SetTurn(true);
                    speedManager.InitFight(this);
                };
                fightLoader.NextFight(this);

                break;
        }
    }

    private void NextState()
    {
        _state = (unitsAlly.All(ally => ally.IsDead())) ? StateEnum.Lose
            : _unitsEnemy.All(enemy => enemy.IsDead()) ? StateEnum.Win
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

    public void EnemiesGenerated(EnemiesGeneratedArgs args)
    {
        onEnemiesGenerated?.Invoke(this, args);
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

    public Unit[] UnitsEnemy => _unitsEnemy;
    public bool IsMyTurn => _isMyTurn;

    public StateEnum State => _state;
}