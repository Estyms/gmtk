using System;
using UnityEngine;

public class AttackFace : DiceFace
{
    public override void Action(Unit caster, Unit target, int value, GameState gameState)
    {
        caster.ShowFight(target);
        caster.OnAttackDone += (_, _) =>
        {
            caster.HideFight(target);
        };
        
        if (value == 6)
        {
            target.TakeDamage(caster.GetAttack()*2);
        }
        else
        {
            var damages = Math.Ceiling(((float)caster.GetAttack() / 4)*value);
            target.TakeDamage((int)damages);
        }
        
        caster.InvokeAttackDone(true);
    }
}