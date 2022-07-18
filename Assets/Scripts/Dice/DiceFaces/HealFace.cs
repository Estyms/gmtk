using UnityEngine;

public class HealFace : DiceFace
{
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Action(Unit caster, Unit target, int value, GameState gameState)
    {
        caster.Heal(value);
        Debug.Log("Healed " + value);
        caster.InvokeAttackDone(false);
    }
}