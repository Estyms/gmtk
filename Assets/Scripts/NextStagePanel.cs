using Manager;
using UnityEngine;

public class NextStagePanel : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) gameState.NextFight();
    }
}