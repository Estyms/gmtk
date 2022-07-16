using System;
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
        Lose = 2,
        Loading = 3
    }

    // Properties
    [SerializeField] private Unit[] unitsAlly;
    [SerializeField] private SpeedManager speedManager;
    [SerializeField] private TextMeshProUGUI _turnText;
    [SerializeField] private FightLoader fightLoader;

    // Setters
    public Ally SelectedUnit
    {
        get;
        set;
        // _selectedUnit.Effect.enabled = true;
    }

    // Getters
    public Unit[] UnitsAlly => unitsAlly;

    public Unit[] UnitsEnemy { get; private set; } = { };

    public bool IsMyTurn { get; set; } = true;

    public StateEnum State { get; private set; } = StateEnum.Fight;

    public void Awake()
    {
        State = StateEnum.Loading;
        onEnemiesGenerated = null;
        onEnemiesGenerated += (_, args) =>
        {
            UnitsEnemy = args.Enemies;
            State = StateEnum.Fight;
            SetTurn(true);
            speedManager.InitFight(this);
            
        };
        fightLoader.NextFight(this);
    }

    // get delted by MOI
    public event EventHandler onEndTurn;
    public event EventHandler<EnemiesGeneratedArgs> onEnemiesGenerated;


    //Setters
    // public Ally SelectedUnit
    // {
    //     get => _selectedUnit;
    //     set
    //     {
    //         if (_selectedUnit) _selectedUnit.Effect.enabled = false;
    //         _selectedUnit = value;
    //         _selectedUnit.Effect.enabled = true;
    //     }
    // }

    // Getters
    // public Unit[] UnitsAlly => unitsAlly;
    //
    // public Unit[] UnitsEnemy => unitsEnemy;
    // public bool IsMyTurn { get; private set; } = true;
    //
    // public StateEnum State { get; private set; } = StateEnum.Fight;
    //
    // public Ally SelectedUnit { get; set; }

    // public event EventHandler onEndTurn;

    public void SetTurn(bool isMyTurn)
    {
        IsMyTurn = isMyTurn;
        _turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
    }

    public void EndFight()
    {
        if (State == StateEnum.Fight) return;
        switch (State)
        {
            case StateEnum.Win:
                State = StateEnum.Loading;
                onEnemiesGenerated = null;
                onEnemiesGenerated += (_, args) =>
                {
                    UnitsEnemy = args.Enemies;
                    State = StateEnum.Fight;
                    // SetTurn(true);
                    speedManager.InitFight(this);
                };
                fightLoader.NextFight(this);
                break;
        }
    }


    private void NextState()
    {
        State = unitsAlly.All(ally => ally.IsDead()) ? StateEnum.Lose
            : UnitsEnemy.All(enemy => enemy.IsDead()) ? StateEnum.Win
            : StateEnum.Fight;
    }


    public void AllyAttack(Unit enemy)
    {
        // Setup event
        onEndTurn = null;
        onEndTurn += (sender, args) =>
        {
            NextState();
            // SetTurn(false);
            Debug.Log("ALLY END TURN " + State);
        };

        // Attack the clicked enemy
        SelectedUnit.Attack(enemy, this);
    }

    public void EnemyAttack(Unit enemy)
    {
        Unit target = null;
        // Return if fight is done
        if (State != StateEnum.Fight) return;

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
            // if (_selectedUnit.IsDead()) _selectedUnit = null;
            // Compute next state
            NextState();
            // Give back the hand to the player
            // SetTurn(true);
            Debug.Log("ENEMY END TURN " + State);
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

    public class EnemiesGeneratedArgs : EventArgs
    {
        public EnemiesGeneratedArgs(Unit[] enemies)
        {
            Enemies = enemies;
        }

        public Unit[] Enemies { get; }
    }
}