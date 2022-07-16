using UnityEngine;

public class DiceManager : MonoBehaviour
{
    [SerializeField] private Dice[] dices;

    public void RollDices()
    {
        foreach (Dice dice in dices)
        {
            dice.Roll();
        }
    }
}