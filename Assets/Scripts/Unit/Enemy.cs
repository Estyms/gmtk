using Manager;
using UnityEngine;

namespace Unit
{
    public class Enemy : Unit
    {
        [SerializeField] private SpriteRenderer hoverCircle;
        private GameState _gameState;
        private Unit _target;

        public SpriteRenderer HoverCircle
        {
            get => hoverCircle;
            set => hoverCircle = value;
        }

        protected new void Start()
        {
            base.Start();
            // Find Manager and get PlayerActions
            _gameState = GameObject.Find("GameManager").GetComponent<GameState>();
            hoverCircle.enabled = false;
        }

        private void Update()
        {
            if (!_gameState.IsMyTurn && !FindObjectOfType<DiceManager>().Rolling && CanAttack)
                _gameState.EnemyAttack(this);
        }

        public void OnMouseEnter()
        {
            hoverCircle.enabled = true;
        }

        public void OnMouseExit()
        {
            hoverCircle.enabled = false;
        }
    }
}