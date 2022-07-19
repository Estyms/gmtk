using Manager;
using UnityEngine;

namespace Dice.DiceFaces
{
    public enum Target
    {
        Enemies,
        Allies,
        None
    }

    public enum Rarity
    {
        None,
        Critical,
        Baaaad
    }

    public abstract class DiceFace : MonoBehaviour
    {
        public Sprite sprite;
        public Target target;
        public Rarity rarity;
        public abstract void Action(Unit.Unit caster, Unit.Unit target, int value, GameState gameState);
    }
}