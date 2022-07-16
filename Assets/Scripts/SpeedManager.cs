using System;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private RectTransform orderPanel;
    [SerializeField] private Image imageUnit;
    private Image[] _imageUnits = {}; 

    private Unit[] _units;
    // subscribe to the event OnAttack

    public void InitFight(GameState gameState)
    {
        // _units = UnitsAlly and UnitsEnemy from component PlayerActions
        ClearArray();
        _units = gameState.UnitsAlly.Where(x=>!x.IsDead()).ToArray().Concat(gameState.UnitsEnemy).ToArray();
        foreach (var unit in _units)
        {
            Debug.Log("SPEEDMANAGER " + unit.name);
        }
        // subscribe to the event OnAttack
        foreach (Unit unit in _units)
        {
            unit.OnAttack += OnAttack;
            unit.OnDie += OnDie;
        }

        Order();
    }

    private void ClearArray()
    {
        foreach (Transform child in orderPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDie(object sender, Unit.OnDieArgs e)
    {
        _units = _units.Where(x => x != e.Deadguy).ToArray();
        ClearArray();
        Display();
    }

    private void OnAttack(object sender, EventArgs e)
    {
        // push the array of units to the end of the queue
        ClearArray();
        Shift();
        Display();
    }

    private void Order()
    {
        // Sort units by speed
        Array.Sort(_units, (a, b) => a.GetSpeed().CompareTo(b.GetSpeed()));
        Array.Reverse(_units);
        // for each unit in _units instantiate a new imageUnit and set it's sprite to the unit's sprite and set it's parent to orderPanel
        foreach (Unit unit in _units)
        {
            Image image = Instantiate(imageUnit, orderPanel);
            image.sprite = unit.GetIcon();
            _imageUnits = _imageUnits.Concat(new []{image}).ToArray();
        }
    }

    private void Display()
    {
        foreach (Unit unit in _units)
        {
            Image image = Instantiate(imageUnit, orderPanel);
            image.sprite = unit.GetIcon();
            _imageUnits = _imageUnits.Concat(new []{image}).ToArray();
        }
    }

    private void Shift()
    {
        var first = _units[0];
        var newArray = new Unit[_units.Length-1];
        Array.Copy(_units,1, newArray, 0, _units.Length - 1);
        _units = newArray.Concat(new[] { first }).ToArray();
    }
}