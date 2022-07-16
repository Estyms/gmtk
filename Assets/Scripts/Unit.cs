using System;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitSo unitSo;
    protected bool _canAttack;
    private int _currentHealth;
    private TextMeshProUGUI _healthText;
    private bool _isDead;

    private void Awake()
    {
        _currentHealth = unitSo.health;
        _isDead = false;
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
        GetComponentInChildren<SpriteRenderer>().sprite = unitSo.sprite;
        _healthText.text = _currentHealth.ToString();
    }

    public event EventHandler OnAttack;
    public event EventHandler<OnDieArgs> OnDie;

    // function attack(Unit target) that deals damage to target
    public virtual void Attack(Unit target, GameState gameState)
    {
        if (!_canAttack) return;
        _canAttack = false;
        // Find Dice and call Roll()
        Dice dice = FindObjectOfType<Dice>();
        dice.RemoveListeners();

        dice.OnDiceRoll += (sender, args) =>
        {
            if (args.Value > 0)
            {
                target.TakeDamage(unitSo.attack);
                Debug.Log("Attacked " + target.name);
            }
            else
            {
                Debug.Log("Attack missed");
            }

            // Invoke OnAttack()
            OnAttack?.Invoke(this, EventArgs.Empty);
            gameState.EndTurn();
        };

        // dice.Roll();
        GameObject.Find("GameManager").GetComponent<DiceManager>().RollDices();
        Debug.Log("DONE");
    }

    // function takeDamage(int damage) that reduces current health by damage minus armor and calls Die if health is 0 or less
    private void TakeDamage(int damage)
    {
        _currentHealth -= damage - unitSo.defense;
        Debug.Log("Took " + (damage - unitSo.defense) + " damage");
        _healthText.text = _currentHealth.ToString();
        if (_currentHealth <= 0) Die();
    }

    // function Die that prints message "Unit died" and destroys game object
    private void Die()
    {
        if (GetType() == typeof(Ally))
        {
            Ally ally = (Ally)this;
            // ally.Effect.enabled = false;
        }

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

    public void SetCanAttack(bool canAttack)
    {
        _canAttack = canAttack;
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