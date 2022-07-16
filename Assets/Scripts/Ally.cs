using System;
using UnityEngine;

public class Ally : Unit
{
    [SerializeField] private SpriteRenderer effect;

    public void Start()
    {
        effect.enabled = false;
    }

    public SpriteRenderer Effect
    {
        get => effect;
        set => effect = value;
    }
}