using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Manager
{
    public class DiceManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Dice.Dice[] dices;
        public bool Rolling { get; private set; }
        public event EventHandler<DiceArgs> OnRollDices;
        public event EventHandler<DoneDiceArgs> OnDoneRollDices;

        public void CallRollDicesListeners(DiceArgs diceArgs)
        {
            OnRollDices?.Invoke(this, diceArgs);
        }

        public void ClearListeners()
        {
            OnDoneRollDices = null;
            OnRollDices = null;
        }

        public void RollDices()
        {
            audioSource.Play();
            Rolling = true;
            var dict = new ConcurrentDictionary<Dice.Dice, int>();

            OnRollDices += (sender, args) =>
            {
                dict.TryAdd(args.Dice, args.Value);
                if (dict.Count != dices.Length) return;
                DoneDiceArgs newArg = new(dict.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value));
                OnDoneRollDices?.Invoke(this, newArg);
                Rolling = false;
            };

            foreach (Dice.Dice dice in dices) dice.MultiRollCall(this);
        }

        public class DiceArgs : EventArgs
        {
            public DiceArgs(int value, Dice.Dice dice)
            {
                Value = value;
                Dice = dice;
            }

            public int Value { get; }

            public Dice.Dice Dice { get; }
        }

        public class DoneDiceArgs : EventArgs
        {
            public DoneDiceArgs(Dictionary<Dice.Dice, int> dict)
            {
                Values = dict;
            }

            public Dictionary<Dice.Dice, int> Values { get; }
        }
    }
}