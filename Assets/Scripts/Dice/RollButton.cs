using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollButton : MonoBehaviour
{
    private PlayerActions _pa;
    // Start is called before the first frame update
    private void Start()
    {
        _pa = FindObjectOfType<PlayerActions>();
    }

    private void OnMouseDown()
    {
        if (_pa.NextActionGet == PlayerActions.NextAction.Rolling)
            FindObjectOfType<PlayerActions>().rollDices();
    }
}
