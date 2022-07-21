using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator _animator;
    private ChestState _currentState;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentState = ChestState.idle;
    }

    private void OnMouseDown()
    {
        ChangeState(ChestState.Open);
    }

    private void OnMouseExit()
    {
        ChangeState(ChestState.idle);
    }

    // on mouse hover
    private void OnMouseOver()
    {
        ChangeState(ChestState.Idle2);
    }

    // function change state of animator
    private void ChangeState(ChestState state)
    {
        if (_currentState == state || _currentState == ChestState.Open) return;

        Debug.Log("Actual state : " + _currentState + "Change state to : " + state);
        _animator.Play(state.ToString());
        _currentState = state;
    }

    // ENUM with idle and idle2 states
    private enum ChestState
    {
        idle,
        Idle2,
        Open
    }
}