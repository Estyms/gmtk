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
    public override void Attack(Unit target, GameState gameState)
    {
        // call base Attack()
        base.Attack(target, gameState);
        SetSelected(false);
    }
}