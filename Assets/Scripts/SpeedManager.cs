using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private RectTransform orderPanel;
    [SerializeField] private Image imageUnit;

    private Unit[] _units;
    // suscribe to the event OnAttack

    private void Start()
    {
        // _units = UnitsAlly and UnitsEnemy from component PlayerActions
        _units = GetComponent<GameState>().unitsAlly.Concat(GetComponent<GameState>().unitsEnemy).ToArray();
        // subscribe to the event OnAttack
        foreach (Unit unit in _units)
        {
            unit.OnAttack += OnAttack;
            unit.OnDie += OnDie;
        }

        Order();
    }

    private void OnDie(object sender, EventArgs e)
    {
        // orderPanel.GetChild(0)
    }

    private void OnAttack(object sender, EventArgs e)
    {
        // push the array of units to the end of the queue
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
        }
    }

    private void Display()
    {
        orderPanel.GetChild(0).SetAsLastSibling();
    }
}