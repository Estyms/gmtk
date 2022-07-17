using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private GameState _gameState;
    private NextAction _nextAction;
    private DiceManager _diceManager;
    private Dictionary<Dice, int> _diceValues;

    public enum NextAction
    {
        Rolling,
        AttackReroll,
        Attack,
        Waiting
    }

    private void Awake()
    {
        _gameState = GetComponent<GameState>();
        _diceManager = GetComponent<DiceManager>();
        _nextAction = NextAction.Rolling;
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_gameState.State)
        {
            case GameState.StateEnum.Fight:
            {
                // if click on a unit in ally team then select it
                if (_gameState.IsMyTurn && Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

                    if (!hit.collider) return;

                    KeyValuePair<Dice, int> actionDice;
                    int value;
                    Unit attackTarget;
                    switch (_nextAction)
                    {
                        // Attack
                        case NextAction.Attack:
                            attackTarget = hit.collider.GetComponent<Unit>();
                            if (!attackTarget) break;

                            actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                            value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;
                            attackTarget = hit.transform.GetComponent<Unit>();
                            _gameState.AllyAttack(attackTarget, _diceValues);
                            _nextAction = NextAction.Rolling;
                            break;

                        // Attack or Re-roll one
                        case NextAction.AttackReroll:

                            var diceTarget = hit.transform.GetComponent<Dice>();
                            if (diceTarget && !diceTarget.isRolling)
                            {
                                Debug.Log(diceTarget.name);
                                _nextAction = NextAction.Waiting;
                                diceTarget.OnDoneRoll += (_, args) =>
                                {
                                    var kv = _diceValues.First(kvp => kvp.Key.DiceType == diceTarget.DiceType);
                                    _diceValues.Remove(diceTarget);
                                    _diceValues.Add(diceTarget, args.Value);
                                    _nextAction = NextAction.Attack;
                                    diceTarget.ClearListeners();
                                };
                                diceTarget.SingleRoll();
                                break;
                            }

                            // Attack
                            attackTarget = hit.transform.GetComponent<Unit>();
                            if (attackTarget)
                            {
                                actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                                value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;
                                Debug.Log(actionDice.Key.DiceSides[actionDice.Value - 1]);
                                _gameState.AllyAttack(attackTarget, _diceValues);
                                _nextAction = NextAction.Rolling;
                            }

                            break;


                        // Rolling
                        case NextAction.Rolling:
                            var rollingDiceTarget = hit.transform.GetComponent<Dice>();
                            if (rollingDiceTarget)
                            {
                                _nextAction = NextAction.Waiting;
                                _diceManager.OnDoneRollDices += (_, args) =>
                                {
                                    Debug.Log("Done Rolling");
                                    _diceManager.ClearListeners();
                                    _diceValues = args.Values;
                                    _nextAction = NextAction.AttackReroll;
                                };
                                _diceManager.RollDices();
                            }

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
}