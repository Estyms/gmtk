using System;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private UnitSo unitSo;
    private SpriteRenderer _backGround;
    private int _currentHealth;
    private Transform[] _fightPositions;
    private TextMeshProUGUI _healthText;
    private bool _isDead;
    private Vector3[] _oldFightPositions;
    private Sprite _sprite;
    protected bool CanAttack;

    private void Awake()
    {
        _currentHealth = unitSo.health;
        _isDead = false;
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
        GetComponentInChildren<SpriteRenderer>().sprite = unitSo.sprite;
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

    // function attack(Unit target) that deals damage to target
    public virtual void Attack(Unit target, GameState gameState)
    {
        if (!CanAttack) return;
        GetComponentInChildren<SpriteRenderer>().sprite = unitSo.attackSprite;
        // set background layer order to 1
        _backGround.sortingOrder = 1;
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
        target.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;

        int unitPos = GetTeam() - 1;
        int targetPos = target.GetTeam() - 1;

        _oldFightPositions[unitPos] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _oldFightPositions[targetPos] = new Vector3(target.transform.position.x, target.transform.position.y,
            target.transform.position.z);

        transform.position = _fightPositions[unitPos].position;
        target.transform.position = _fightPositions[targetPos].position;

        CanAttack = false;
        // Find Dice and call Roll()
        DiceManager diceManager = GameObject.Find("GameManager").GetComponent<DiceManager>();
        diceManager.ClearListeners();
        diceManager.OnDoneRollDices += (_, args) =>
        {
            var number = 0;
            foreach (var (dice,pValue) in args.Values)
            {
                if (dice.DiceType == DiceSo.DiceType.Number) number = pValue;
            }
            Debug.Log(number);
            if (number > 3)
            {
                target.TakeDamage(unitSo.attack);
                Debug.Log("Attacked " + target.name);
            }
            else
            {
                Debug.Log("Attack missed");
            }

            GetComponentInChildren<SpriteRenderer>().sprite = unitSo.sprite;
            _backGround.sortingOrder = -1;
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
            target.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;


            transform.position = _oldFightPositions[unitPos];
            target.transform.position = _oldFightPositions[targetPos];

            // Invoke OnAttack()
            OnAttack?.Invoke(this, EventArgs.Empty);
            gameState.EndTurn();
        };

        // dice.Roll();
        diceManager.RollDices();
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