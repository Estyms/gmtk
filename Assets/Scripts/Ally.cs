using System;
using UnityEngine;

public class Ally : Unit
{
    [SerializeField] private SpriteRenderer selectCircle;
    [SerializeField] private SpriteRenderer hoverCircle;

    public void Start()
    {
        hoverCircle.enabled = false;
        selectCircle.enabled = false;
    }

    public SpriteRenderer Effect
    {
        get => selectCircle;
        set => selectCircle = value;
    }

    public SpriteRenderer HoverCircle
    {
        get => hoverCircle;
        set => hoverCircle = value;
    }

    private void OnMouseEnter()
    { 
        if (!selectCircle.enabled) hoverCircle.enabled = true;
    }

    private void OnMouseExit()
    {
        hoverCircle.enabled = false;
    }
}