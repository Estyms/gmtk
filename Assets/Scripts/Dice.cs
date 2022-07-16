using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

    public bool isRolling = false;
    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private SpriteRenderer rend;
    private int _result;

	// Use this for initialization
	private void Start () {

        // Assign Renderer component
        rend = GetComponent<SpriteRenderer>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}
	
    // If you left click over the dice then RollTheDice coroutine is started
    public int Roll()
    {
        isRolling = true;
        _result = Random.Range(0, 6);
        StartCoroutine("RollTheDice");
        return _result + 1;
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDice()
    {
        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 14; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            int randomDiceSide = Random.Range(0, 6);

            // Set sprite to upper face of dice from array according to random value
            rend.sprite = diceSides[randomDiceSide];

            // Pause before next itteration
            yield return new WaitForSeconds(0.1f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        rend.sprite = diceSides[_result];

        // Show final dice value in Console
        Debug.Log(_result);
        isRolling = false;
    }
}
