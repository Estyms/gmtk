using Dice.DiceFaces;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "DiceSo", menuName = "ScriptableObjects/DiceSo", order = 1)]
    public class DiceSo : ScriptableObject
    {
        public enum DiceType
        {
            Number,
            Action
        }

        public DiceType diceType;
        public DiceFace[] diceFaces;
    }
}