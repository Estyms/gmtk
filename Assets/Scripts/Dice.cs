using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] private DiceSo die;

    public bool isRolling;

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] _diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer _rend;
    private int _result;

    // Use this for initialization
    private void Start()
    {
        // Assign Renderer component
        _rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        _diceSides = die.diceFaces;
    }

    public event EventHandler<RollEventArgs> OnDiceRoll;

    // If you left click over the dice then RollTheDice coroutine is started
    public void Roll()
    {
        isRolling = true;
        StartCoroutine("RollTheDice");
    }

    public void RemoveListeners()
    {
        OnDiceRoll = null;
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 14; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            _result = Random.Range(0, _diceSides.Length);

            // Set sprite to upper face of dice from array according to random value
            _rend.sprite = _diceSides[_result];

            // Pause before next itteration
            yield return new WaitForSeconds(0.1f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        _rend.sprite = _diceSides[_result];

        // Show final dice value in Console
        Debug.Log(_result + 1);

        OnDiceRoll?.Invoke(this, new RollEventArgs(_result + 1));
        yield return new WaitForSeconds(1f);
        isRolling = false;
    }

    public class RollEventArgs : EventArgs
    {
        public RollEventArgs(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}