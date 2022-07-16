using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnText;
    public Unit[] unitsAlly, unitsEnemy;
    private bool _isMyTurn = true;
    private Unit _selectedUnit;

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
                    _selectedUnit = hit.transform.GetComponent<Unit>();
                }
                else if (hit.transform.GetComponent<Unit>().GetTeam() == 2 && _isMyTurn && _selectedUnit != null)
                {
                    _selectedUnit.Attack(hit.transform.GetComponent<Unit>());
                    SetTurn(false);
                }
            }

            Debug.Log("selected unit is " + _selectedUnit.GetName());
        }
    }

    public void SetTurn(bool isMyTurn)
    {
        _isMyTurn = isMyTurn;
        _turnText.text = isMyTurn ? "Your turn" : "Enemy turn";
    }
    
    public bool GetTurn()
    {
        return _isMyTurn;
    }
}