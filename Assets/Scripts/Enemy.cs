using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Unit
{
    private GameState _gameState;
    private Unit _target;
    [SerializeField] private SpriteRenderer hoverCircle;

    private void Start()
    {
        // Find Manager and get PlayerActions
        _gameState = GameObject.Find("GameManager").GetComponent<GameState>();
        hoverCircle.enabled = false;
    }

    private void Update()
    {
        if (!_gameState.IsMyTurn && !
                FindObjectOfType<Dice>().isRolling)
        {
            _gameState.EnemyAttack(this);
        }
    }

    public SpriteRenderer HoverCircle
    {
        get => hoverCircle;
        set => hoverCircle = value;
    }

    public void OnMouseEnter()
    {
        hoverCircle.enabled = true;
    }

    public void OnMouseExit()
    {
        hoverCircle.enabled = false;
    }
}