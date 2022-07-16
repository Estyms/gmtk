using System;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitSo unitSo;
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
    public event EventHandler OnDie;

    // function attack(Unit target) that deals damage to target
    public void Attack(Unit target)
    {
        // Find Dice and call Roll()
        Dice dice = FindObjectOfType<Dice>();
        if (dice.Roll() > 3)
        {
            target.TakeDamage(unitSo.attack);
            Debug.Log("Attacked " + target.name);
        }
        else
        {
            Debug.Log("Attack missed");
        }

        OnAttack?.Invoke(this, EventArgs.Empty);
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
        if (GetType() == typeof(Ally)) ((Ally)this).Effect.enabled = false;

        Debug.Log("Unit died");
        gameObject.SetActive(false);
        _isDead = true;
        OnDie?.Invoke(this, EventArgs.Empty);
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
}