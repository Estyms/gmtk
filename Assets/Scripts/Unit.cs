using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitSo unitSo;
    private SpriteRenderer _backGround;
    private SpriteRenderer _spriteRenderer;
    private int _currentHealth;
    private Transform[] _fightPositions;
    private TextMeshProUGUI _healthText;
    private bool _isDead;
    private Vector3[] _oldFightPositions;
    protected bool CanAttack;

    private void Awake()
    {
        _currentHealth = unitSo.health;
        _isDead = false;
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = unitSo.sprite;
        _healthText.text = _currentHealth.ToString();
    }

    protected virtual void Start()
    {
        _backGround = GameObject.Find("backGround").GetComponent<SpriteRenderer>();
        _fightPositions = new Transform[2];
        _oldFightPositions = new Vector3[2];
        _fightPositions[0] = GameObject.Find("AllyFightPosition").transform;
        _fightPositions[1] = GameObject.Find("EnemyFightPosition").transform;
    }

    public event EventHandler OnAttack;
    public event EventHandler<OnDieArgs> OnDie;

    public event EventHandler OnAttackDone;

    public void HideFight(Unit target)
    {
        int unitPos = GetTeam() - 1;
        int targetPos = target.GetTeam() - 1;

        _spriteRenderer.sprite = unitSo.sprite;
        _backGround.sortingOrder = -1;
        _spriteRenderer.sortingOrder = 0;
        target._spriteRenderer.sortingOrder = 0;

        transform.position = _oldFightPositions[unitPos];
        target.transform.position = _oldFightPositions[targetPos];
    }

    public void ShowFight(Unit target)
    {
        _spriteRenderer.sprite = unitSo.attackSprite;
        // set background layer order to 1
        _backGround.sortingOrder = 1;
        _spriteRenderer.sortingOrder = 3;
        target._spriteRenderer.sortingOrder = 2;

        int unitPos = GetTeam() - 1;
        int targetPos = target.GetTeam() - 1;

        var transform1 = transform;
        var position = transform1.position;
        _oldFightPositions[unitPos] = new Vector3(position.x, position.y, position.z);
        var transform2 = target.transform;
        var position1 = transform2.position;
        _oldFightPositions[targetPos] = new Vector3(position1.x, position1.y,
            position1.z);

        position = _fightPositions[unitPos].position;
        transform1.position = position;
        position1 = _fightPositions[targetPos].position;
        transform2.position = position1;
    }

    // function attack(Unit target) that deals damage to target
    public virtual void Action(Unit target, GameState gameState, Dictionary<Dice, int> diceValues)
    {
        if (!CanAttack) return;
        CanAttack = false;
        // Find Dice and call Roll()
        var actionDice = diceValues.First(kvp=>kvp.Key.DiceType == DiceSo.DiceType.Action);
        var numberDice = diceValues.First(kvp=>kvp.Key.DiceType == DiceSo.DiceType.Number);
        
        OnAttackDone += (_, _) =>
        {
            // Invoke OnAttack()
            OnAttack?.Invoke(this, EventArgs.Empty);
            gameState.EndTurn();
        };
        
        actionDice.Key.DiceSides[actionDice.Value-1].Action(this, target, numberDice.Value, gameState);
    }

    public void InvokeAttackDone(bool x)
    {
        StartCoroutine(AttackDone(x));
    }
    
    private IEnumerator AttackDone(bool attacked)
    {
        if (attacked)
        {
            yield return new WaitForSeconds(1f);
        }
        OnAttackDone?.Invoke(this, EventArgs.Empty);
        OnAttackDone = null;
    }

    // function takeDamage(int damage) that reduces current health by damage minus armor and calls Die if health is 0 or less
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage - unitSo.defense;
        Debug.Log("Took " + (damage - unitSo.defense) + " damage");
        _healthText.text = _currentHealth.ToString();
        if (_currentHealth <= 0) Die();
    }

    // function Die that prints message "Unit died" and destroys game object
    private void Die()
    {
        _isDead = true;
        Debug.Log("Unit died");
        OnDie?.Invoke(this, new OnDieArgs(this));
        gameObject.SetActive(false);
    }

    public void ResetHandlers()
    {
        OnAttack = null;
        OnDie = null;
    }

    public virtual void Heal(GameState gameState, int health)
    {
        CanAttack = false;
        _currentHealth = Math.Min(health + _currentHealth, unitSo.health);
        Debug.Log("Got Healed " + health  + " HP");
        _healthText.text = _currentHealth.ToString();
        OnAttack?.Invoke(this, EventArgs.Empty);
        gameState.EndTurn();
    }

    public int GetTeam()
    {
        return unitSo.team;
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public int GetSpeed()
    {
        return unitSo.speed;
    }

    public Sprite GetSprite()
    {
        return unitSo.sprite;
    }

    public Sprite GetIcon()
    {
        return unitSo.icon;
    }

    public string GetName()
    {
        return unitSo.nameString;
    }
    
    public int GetAttack()
    {
        return unitSo.attack;
    }

    public void SetCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }
    

    public class OnDieArgs : EventArgs
    {
        public OnDieArgs(Unit deadguy)
        {
            Deadguy = deadguy;
        }

        public Unit Deadguy { get; }
    }
}