using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "UnitSo", menuName = "ScriptableObjects/UnitSo", order = 1)]
    public class UnitSo : ScriptableObject
    {
        [Header("Stats Settings")] 
        public int attack;
        public int defense;
        public int speed;
        public int health;
        public int cost;
        public int team;

        [Header("Description")] 
        public string description;
        public string nameString;

        [Header("Sprites")] 
        public Sprite sprite;
        public Sprite attackSprite;
        public Sprite icon;

        [Header("Audio")] 
        public AudioClip[] attackSounds;
        public AudioClip[] takingDamageSounds;

        [Header("Prefab")] public GameObject prefab;
    }
}