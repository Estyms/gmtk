using UnityEngine;

public class Enemy : Unit
{
    private PlayerActions _playerActions;
    private Unit _target;

    private void Start()
    {
        // Find Manager and get PlayerActions
        _playerActions = GameObject.Find("Manager").GetComponent<PlayerActions>();
    }

    private void Update()
    {
        if (!_playerActions.GetTurn() && !FindObjectOfType<Dice>().isRolling)
        {
            while (_target == null)
            {
                int random = Random.Range(0, _playerActions.unitsAlly.Length);
                if (!_playerActions.unitsAlly[random].IsDead()) _target = _playerActions.unitsAlly[random];
            }

            Attack(_target);
            _playerActions.SetTurn(true);
        }
    }
}