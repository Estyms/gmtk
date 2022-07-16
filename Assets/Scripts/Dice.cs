using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour {
    public class RollEventArgs : EventArgs
    {
        private readonly int _value;

        public RollEventArgs(int value)
        {
            this._value = value;
        }
        
        public int Value => _value;
    }

    public bool isRolling = false;
    // Array of dice sides sprites to load from Resources folder
    private Sprite[] _diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer _rend;
    private int _result;

    public event EventHandler<RollEventArgs> onDiceRoll;

	// Use this for initialization
	private void Start () {

        // Assign Renderer component
        _rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        _diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}
	
    // If you left click over the dice then RollTheDice coroutine is started
    public void Roll()
    {
        isRolling = true;
        StartCoroutine("RollTheDice");
    }

    public void RemoveListeners()
    {
        onDiceRoll = null;
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 14; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            _result = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            _rend.sprite = _diceSides[_result];

            // Pause before next itteration
            yield return new WaitForSeconds(0.1f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        _rend.sprite = _diceSides[_result];

        // Show final dice value in Console
        Debug.Log(_result+1);
        
        onDiceRoll?.Invoke(this, new RollEventArgs(_result+1));
        yield return new WaitForSeconds(1f);
        isRolling = false;
    }
}
