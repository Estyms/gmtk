using System;
using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    
    [SerializeField] private GameState _gameState;


    // Update is called once per frame
    private void Update()
    {
        // if click on a unit in ally team then select it
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (hit.transform.GetComponent<Unit>().GetTeam() == 1)
                {
                    _gameState.SelectedUnit = hit.transform.GetComponent<Ally>();
                }
                else if (hit.transform.GetComponent<Unit>().GetTeam() == 2 && _gameState.IsMyTurn && _gameState.SelectedUnit != null)
                {
                    _gameState.Attack(hit.transform.GetComponent<Unit>());
                }
                Debug.Log("selected unit is " + _gameState.SelectedUnit.GetName());
            }
        }
    }
}