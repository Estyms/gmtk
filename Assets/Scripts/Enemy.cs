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
        _gameState = GameObject.Find("Manager").GetComponent<GameState>();
        hoverCircle.enabled = false;
    }

    private void Update()
    {
        if (!_gameState.IsMyTurn && !
                FindObjectOfType<Dice>().isRolling)
        {
            while (_target == null)
            {
                int random = Random.Range(0, _gameState.unitsAlly.Length);
                if (!_gameState.unitsAlly[random].IsDead()) _target = _gameState.unitsAlly[random];
            }

            Attack(_target);
            _gameState.SetTurn(true);
            _target = null;
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