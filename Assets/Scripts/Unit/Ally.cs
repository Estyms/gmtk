using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    [SerializeField] private SpriteRenderer selectCircle;
    [SerializeField] private SpriteRenderer hoverCircle;

    protected new void Start()
    {
        base.Start();
        hoverCircle.enabled = false;
        selectCircle.enabled = false;
    }

    public void SetSelected(bool selected)
    {
        selectCircle.enabled = selected;
    }
    
    // override Attack()
    public override void Action(Unit target, GameState gameState, Dictionary<Dice, int> diceValues)
    {
        // call base Attack()
        base.Action(target, gameState, diceValues);
        SetSelected(false);
    }

    public override void Heal(int value)
    {
        base.Heal(value);
        SetSelected(false);
    }
}