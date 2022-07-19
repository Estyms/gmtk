using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameState : MonoBehaviour
    {
        public enum StateEnum
        {
            Fight,
            Win = 1,
            Lose = 2,
            Loading = 3
        }

        [SerializeField] private TeamSpawner teamSpawner;
        [SerializeField] private SpeedManager speedManager;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private FightLoader fightLoader;
        [SerializeField] private GameObject nextStagePanel;
        [SerializeField] private GameObject backStagePanel;

        // Properties

        // Setters
        public Ally SelectedUnit { get; set; }

        // Getters
        public Unit.Unit[] UnitsAlly { get; private set; }

        public Unit.Unit[] UnitsEnemy { get; private set; } = { };

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

        private void Start()
        {
            UnitsAlly = teamSpawner.Units;
        }

        // get delted by MOI
        public event EventHandler onEndTurn;
        public event EventHandler<EnemiesGeneratedArgs> onEnemiesGenerated;

        public void SetTurn(bool isMyTurn)
        {
            IsMyTurn = isMyTurn;
            turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
        }

        public void EndFight()
        {
            if (State == StateEnum.Fight) return;
            switch (State)
            {
                case StateEnum.Win:
                    nextStagePanel.SetActive(true);
                    backStagePanel.SetActive(true);
                    break;
            }
        }

        public void NextFight()
        {
            nextStagePanel.SetActive(false);
            backStagePanel.SetActive(false);

            State = StateEnum.Loading;
            onEnemiesGenerated = null;
            onEnemiesGenerated += (_, args) =>
            {
                UnitsEnemy = args.Enemies;
                State = StateEnum.Fight;
                speedManager.InitFight(this);
            };
            fightLoader.NextFight(this);
        }


        private void NextState()
        {
            State = UnitsAlly.All(ally => ally.IsDead()) ? StateEnum.Lose
                : UnitsEnemy.All(enemy => enemy.IsDead()) ? StateEnum.Win
                : StateEnum.Fight;
        }


        public void AllyAttack(Unit.Unit enemy, Dictionary<Dice.Dice, int> diceValues)
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
            SelectedUnit.Action(enemy, this, diceValues);
        }

        public void EnemyAttack(Unit.Unit enemy)
        {
            Unit.Unit target = null;
            // Return if fight is done
            if (State != StateEnum.Fight) return;

            // Search a target
            while (target == null)
            {
                int random = Random.Range(0, UnitsAlly.Length);
                if (!UnitsAlly[random].IsDead()) target = UnitsAlly[random];
            }

            DiceManager diceManager = GameObject.Find("GameManager").GetComponent<DiceManager>();

            // Setup Event
            onEndTurn = null;
            onEndTurn += (sender, args) =>
            {
                NextState();
                // Give back the hand to the player
                // SetTurn(true);
                diceManager.ClearListeners();
                Debug.Log("ENEMY END TURN " + State);
            };


            diceManager.ClearListeners();
            diceManager.OnDoneRollDices += (_, args) => enemy.Action(target, this, args.Values);

            // Attack the target
            diceManager.RollDices();
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
            public EnemiesGeneratedArgs(Unit.Unit[] enemies)
            {
                Enemies = enemies;
            }

            public Unit.Unit[] Enemies { get; }
        }
    }
}