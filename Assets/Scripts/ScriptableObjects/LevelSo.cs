using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelSo", menuName = "ScriptableObjects/Level", order = 1)]
    public class LevelSo : ScriptableObject
    {
        public int level;
        public Sprite background;
        
        [Header("Enemies")] 
        public UnitSo[] enemies;
        public int waves;

        [Header("Rewards")] 
        public int gold;
        public UnitSo unit;
    }
}