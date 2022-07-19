using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnitSo", menuName = "ScriptableObjects/UnitSo", order = 1)]
    public class UnitSo : ScriptableObject
    {
        public int attack;
        public int defense;
        public string description;
        public Sprite sprite;
        public Sprite attackSprite;
        public Sprite icon;
        public AudioClip attackSound;

        // variables for the unit life, attack, defense, speed, range, and cost
        public int health;

        // variables for the unit's name, description, and image
        public string nameString;
        public int speed;
        public int team;
        public int cost;
        public GameObject prefab;
    }
}