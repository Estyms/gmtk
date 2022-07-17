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
        Attack
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
                            actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                            value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;
                            attackTarget = hit.transform.GetComponent<Unit>();
                            switch (actionDice.Key.DiceSides[actionDice.Value - 1].target)
                            {
                                case Target.Enemies:
                                    if (attackTarget?.GetTeam() == 2)
                                    {
                                        _gameState.AllyAttack(attackTarget, _diceValues);
                                        _nextAction = NextAction.Rolling;
                                    }

                                    break;

                                default:
                                    actionDice.Key.DiceSides[actionDice.Value].Action(
                                        _gameState.SelectedUnit,
                                        _gameState.SelectedUnit,
                                        value,
                                        _gameState);
                                    _nextAction = NextAction.Rolling;
                                    break;
                            }


                            break;

                        // Attack or Re-roll one
                        case NextAction.AttackReroll:

                            var diceTarget = hit.transform.GetComponent<Dice>();
                            if (diceTarget?.DiceType != null)
                            {
                                var dice = hit.transform.GetComponent<Dice>();
                                Debug.Log("Reroll " + dice.DiceType);
                                _nextAction = NextAction.Attack;
                            }

                            // Attack
                            attackTarget = hit.transform.GetComponent<Unit>();
                            actionDice = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Action);
                            value = _diceValues.First(kvp => kvp.Key.DiceType == DiceSo.DiceType.Number).Value;
                            Debug.Log(actionDice.Key.DiceSides[actionDice.Value - 1]);
                            switch (actionDice.Key.DiceSides[actionDice.Value - 1].target)
                            {
                                case Target.Enemies:
                                    if (attackTarget?.GetTeam() == 2)
                                    {
                                        _gameState.AllyAttack(attackTarget, _diceValues);
                                        _nextAction = NextAction.Rolling;
                                    }

                                    break;

                                default:
                                    actionDice.Key.DiceSides[actionDice.Value - 1].Action(
                                        _gameState.SelectedUnit,
                                        _gameState.SelectedUnit,
                                        value,
                                        _gameState);
                                    _nextAction = NextAction.Rolling;
                                    break;
                            }

                            break;


                        // Rolling
                        case NextAction.Rolling:
                            var rollingDiceTarget = hit.transform.GetComponent<Dice>();
                            if (rollingDiceTarget?.DiceType != null)
                            {
                                _diceManager.ClearListeners();
                                _diceManager.OnDoneRollDices += (_, args) =>
                                {
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