using Manager;

namespace Dice.DiceFaces
{
    public class BlackFace : DiceFace
    {
        public override void Action(Unit.Unit caster, Unit.Unit _, int value, GameState gameState)
        {
            caster.TakeDamage(value);
            if (caster.enabled)
                caster.InvokeAttackDone(false);
            else
                gameState.EndTurn();
        }
    }
}