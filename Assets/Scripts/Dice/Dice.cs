using System;
using System.Collections;
using Dice.DiceFaces;
using Manager;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dice
{
    public class Dice : MonoBehaviour
    {
        [SerializeField] private DiceSo die;

        public bool isRolling;

        // Array of dice sides sprites to load from Resources folder
        private SpriteRenderer _hover;
        private PlayerActions _pa;

        // Reference to sprite renderer to change sprites
        private SpriteRenderer _rend;
        private int _result;

        public DiceSo.DiceType DiceType => die.diceType;

        public DiceFace[] DiceSides { get; private set; }

        // Use this for initialization
        private void Start()
        {
            // Assign Renderer component
            _pa = FindObjectOfType<PlayerActions>();
            _rend = GetComponent<SpriteRenderer>();
            _hover = transform.GetChild(0).GetComponent<SpriteRenderer>();

            // Load dice sides sprites to array from DiceSides subfolder of Resources folder
            DiceSides = die.diceFaces;
        }

        private void OnMouseDown()
        {
            _hover.enabled = false;
        }


        private void OnMouseEnter()
        {
            if (_pa.NextActionGet == PlayerActions.NextAction.Rolling ||
                _pa.NextActionGet == PlayerActions.NextAction.AttackReroll)
                _hover.enabled = true;
        }


        private void OnMouseExit()
        {
            _hover.enabled = false;
        }

        public event EventHandler<RollEventArgs> OnDoneRoll;


        public void SingleRoll()
        {
            isRolling = true;
            StartCoroutine(SingleRollTheDice());
        }


        // If you left click over the dice then RollTheDice coroutine is started
        public void MultiRollCall(DiceManager diceManager)
        {
            isRolling = true;
            StartCoroutine(MultiRollTheDice(diceManager));
        }


        public IEnumerator RollTheDice()
        {
            for (int i = 0; i <= 14; i++)
            {
                // Pick up random value from 0 to 5 (All inclusive)
                _result = Random.Range(0, DiceSides.Length);

                // Set sprite to upper face of dice from array according to random value
                _rend.sprite = DiceSides[_result].sprite;

                // Pause before next itteration
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ClearListeners()
        {
            OnDoneRoll = null;
        }


        private IEnumerator SingleRollTheDice()
        {
            yield return RollTheDice();
            _rend.sprite = DiceSides[_result].sprite;
            OnDoneRoll?.Invoke(this, new RollEventArgs(_result + 1));
        }

        // Coroutine that rolls the dice
        private IEnumerator MultiRollTheDice(DiceManager diceManager)
        {
            yield return RollTheDice();

            _rend.sprite = DiceSides[_result].sprite;

            yield return new WaitForSeconds(1f);
            isRolling = false;
            diceManager.CallRollDicesListeners(new DiceManager.DiceArgs(_result + 1, this));
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
}