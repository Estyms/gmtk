using Unit;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "TeamListSo", menuName = "ScriptableObjects/TeamListSo", order = 1)]
    public class TeamListSo : ScriptableObject
    {
        public UnitSo[] teamList;
    }
}