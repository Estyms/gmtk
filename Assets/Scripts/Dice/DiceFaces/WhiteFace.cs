using System;
using Manager;

namespace Dice.DiceFaces
{
    public class WhiteFace : DiceFace
    {
        public override void Action(Unit.Unit caster, Unit.Unit target, int value, GameState gameState)
        {
            caster.ShowFight(target);
            caster.OnAttackDone += (_, _) => { caster.HideFight(target); };

            if (value == 6)
            {
                target.TakeDamage(caster.GetAttack() * 3);
            }
            else
            {
                double damages = Math.Ceiling((float)caster.GetAttack() / 3 * value);
                target.TakeDamage((int)damages);
            }


            caster.InvokeAttackDone(true);
        }
    }
}