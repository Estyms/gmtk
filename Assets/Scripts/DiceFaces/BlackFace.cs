using UnityEngine;

public class BlackFace : DiceFace
{
    public override void Action(Unit caster, Unit _, int value, GameState gameState)
    {
        caster.TakeDamage(value);
        if (caster.enabled)
        {
            caster.InvokeAttackDone(false);
        }
        else
        {
            gameState.EndTurn();
        }
        
    }
}