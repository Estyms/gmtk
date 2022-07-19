using System.Collections.Generic;
using System.Linq;
using Manager;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public enum NextAction
    {
        Rolling,
        AttackReroll,
        Attack,
        Waiting
    }

    [SerializeField] private TextMeshProUGUI rerollAmountText;
    private DiceManager _diceManager;
    private Dictionary<Dice.Dice, int> _diceValues;
    private GameState _gameState;

    public NextAction NextActionGet { get; private set; }

    public int RerollAmount { get; set; } = 3;

    private void Awake()
    {
        _gameState = GetComponent<GameState>();
        _diceManager = GetComponent<DiceManager>();
        NextActionGet = NextAction.Rolling;
    }

    private void Update()
    {
        switch (_gameState.State)
        {
            case GameState.StateEnum.Fight:
            {
                if (_gameState.IsMyTurn && Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                    if (!hit.collider) return;

                    KeyValuePair<Dice.Dice, int> actionDice;
                    int value;
                    Unit.Unit attackTarget;

                    switch (NextActionGet)
                    {
                        // Attack
                        case NextAction.Attack:
                            attackTarget = hit.transform.GetComponent<Unit.Unit>();

                            if (!attackTarget) break;

                            actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                            value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;

                            _gameState.AllyAttack(attackTarget, _diceValues);
                            NextActionGet = NextAction.Rolling;
                            break;

                        // Attack or Re-roll one
                        case NextAction.AttackReroll:

                            Dice.Dice diceTarget = hit.transform.GetComponent<Dice.Dice>();
                            if (diceTarget && !diceTarget.isRolling && RerollAmount > 0)
                            {
                                Debug.Log(diceTarget.name);
                                NextActionGet = NextAction.Waiting;
                                diceTarget.OnDoneRoll += (_, args) =>
                                {
                                    var kv = _diceValues.First(kvp => kvp.Key.DiceType == diceTarget.DiceType);
                                    _diceValues.Remove(diceTarget);
                                    _diceValues.Add(diceTarget, args.Value);
                                    NextActionGet = NextAction.Attack;
                                    diceTarget.ClearListeners();
                                };
                                diceTarget.SingleRoll();
                                RerollAmount--;
                                UpdateRerollAmountText();
                                break;
                            }

                            // Attack
                            attackTarget = hit.transform.GetComponent<Unit.Unit>();
                            if (attackTarget)
                            {
                                actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                                value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;
                                Debug.Log(actionDice.Key.DiceSides[actionDice.Value - 1]);
                                _gameState.AllyAttack(attackTarget, _diceValues);
                                NextActionGet = NextAction.Rolling;
                            }

                            break;

                        // Rolling
                        case NextAction.Rolling:
                            Dice.Dice rollingDiceTarget = hit.transform.GetComponent<Dice.Dice>();
                            if (rollingDiceTarget) rollDices();

                            break;
                    }


                    // Debug.Log("selected unit is " + _gameState.SelectedUnit.GetName());
                }
            }
                break;

            default:
                _gameState.EndFight();
                break;
        }
    }

    public void rollDices()
    {
        NextActionGet = NextAction.Waiting;
        _diceManager.OnDoneRollDices += (_, args) =>
        {
            Debug.Log("Done Rolling");
            _diceManager.ClearListeners();
            _diceValues = args.Values;
            NextActionGet = NextAction.AttackReroll;
        };
        _diceManager.RollDices();
    }

    public void UpdateRerollAmountText()
    {
        rerollAmountText.text = RerollAmount + " Reroll";
    }
}