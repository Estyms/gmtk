using System;
using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private GameState gameState;


    // Update is called once per frame
    private void Update()
    {
        switch (gameState.State)
        {
            case GameState.StateEnum.Fight:
            {
                // if click on a unit in ally team then select it
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
                    if (hit.collider)
                    {
                        if (hit.transform.GetComponent<Unit>().GetTeam() == 1)
                        {
                            gameState.SelectedUnit = hit.transform.GetComponent<Ally>();
                        }
                        else if (hit.transform.GetComponent<Unit>().GetTeam() == 2 && gameState.IsMyTurn &&
                                 gameState.SelectedUnit != null)
                        {
                            gameState.AllyAttack(hit.transform.GetComponent<Unit>());
                        }

                        Debug.Log("selected unit is " + gameState.SelectedUnit.GetName());
                    }
                }
            } break;
            
            default:
                gameState.EndFight();
                break;
        }
    }
}