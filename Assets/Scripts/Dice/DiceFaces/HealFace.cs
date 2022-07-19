using Manager;
using UnityEngine;

namespace Dice.DiceFaces
{
    public class HealFace : DiceFace
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void Action(Unit.Unit caster, Unit.Unit target, int value, GameState gameState)
        {
            caster.Heal(value);
            Debug.Log("Healed " + value);
            caster.InvokeAttackDone(false);
        }
    }
}