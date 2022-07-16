using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] private RectTransform orderPanel;
    [SerializeField] private Image imageunit;
    private Unit[] _units;

    private void Awake()
    {
        // _units = UnitsAlly and UnitsEnemy from component PlayerActions
        _units = GetComponent<GameState>().unitsAlly.Concat(GetComponent<GameState>().unitsEnemy).ToArray();
    }

    private void Start()
    {
        Order();
        Display();
    }

    private void Order()
    {
        // Sort units by speed
        Array.Sort(_units, (a, b) => a.GetSpeed().CompareTo(b.GetSpeed()));
    }

    private void Display()
    {
        // for each unit in _units instantiate a new imageUnit and set it's sprite to the unit's sprite and set it's parent to orderPanel
        foreach (Unit unit in _units)
        {
            Image image = Instantiate(imageunit, orderPanel);
            image.sprite = unit.GetIcon();
        }
    }
}