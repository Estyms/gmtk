using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class DiceManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private Dice[] dices;
    private bool _rolling;


    public class DiceArgs : EventArgs
    {
        private int _value;
        private Dice _dice;

        public DiceArgs(int value, Dice dice)
        {
            this._value = value;
            this._dice = dice;
        }

        public int Value => _value;
        public Dice Dice => _dice;
    }

    public class DoneDiceArgs : EventArgs
    {
        public DoneDiceArgs(Dictionary<Dice, int> dict)
        {
            Values = dict;
        }

        public Dictionary<Dice, int> Values { get; }
    }


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
        _rolling = true;
        ConcurrentDictionary<Dice, int> dict = new ConcurrentDictionary<Dice, int>();

        OnRollDices += (sender, args) =>
        {
            dict.TryAdd(args.Dice, args.Value);
            if (dict.Count != dices.Length) return;
            var newArg = new DoneDiceArgs(dict.ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value));
            OnDoneRollDices?.Invoke(this, newArg);
            _rolling = false;
        };

        foreach (var dice in dices){
            dice.MultiRollCall(this);
        }
    }

    public bool Rolling => _rolling;
}