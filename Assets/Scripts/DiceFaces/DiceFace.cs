using UnityEngine;

public enum Target
{
    Enemies,
    Allies,
    None
}

public abstract class DiceFace : MonoBehaviour
{
    public Sprite sprite;
    public Target target;
    public abstract void Action(Unit caster, Unit target, int value, GameState gameState);
}