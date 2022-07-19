using System;
using ScriptableObjects;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    [SerializeField] private UnitSelector unitSelector;
    [SerializeField] private SpriteRenderer unitPortrait;
    [SerializeField] private int portraitIndex;
    private UnitSo _ally;
    private TeamListSo _teamList;

    private void Awake()
    {
        // get ActualTeam from resources
        _teamList = Resources.Load<TeamListSo>("ActualTeam");
        _ally = _teamList.teamList[portraitIndex];
        Debug.Log(_ally);
        Display();
    }

    private void Start()
    {
        unitSelector.OnUnitSelected += UnitSelector_OnUnitSelected;
    }

    private void OnMouseDown()
    {
        _ally = null;
        Display();
        _teamList.teamList[portraitIndex] = _ally;
    }

    private void UnitSelector_OnUnitSelected(object sender, EventArgs e)
    {
        _ally = _teamList.teamList[portraitIndex];
        Display();
    }

    private void Display()
    {
        unitPortrait.sprite = _ally != null ? _ally.icon : null;
    }
}