using UnityEngine;

public class WhiteFace : DiceFace
{
    public override void Action(Unit caster, Unit target, int value, GameState gameState)
    {
        caster.OnAttackDone += (_, _) =>
        {
            caster.HideFight(target);
        };
        
        caster.ShowFight(target);
        caster.InvokeAttackDone(true);
    }
}