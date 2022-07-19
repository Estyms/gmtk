using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerStatsSo", menuName = "ScriptableObjects/PlayerStatsSo", order = 1)]
    public class PlayerStatsSo : ScriptableObject
    {
        public TeamListSo actualTeam;
        public List<UnitSo> unlockedAllies;
        public int gold;
    }
}