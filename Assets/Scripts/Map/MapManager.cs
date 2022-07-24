using System;
using JSON;
using ScriptableObjects;
using UnityEngine;

namespace Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Level[] levels;
        [SerializeField] private Transform[] levelPositions;
        [SerializeField] private int indexMap;

        private TextAsset _jsonFile;

        private void Awake()
        {
            // check if is enough levelPositions to draw all levels
            if (levelPositions.Length < levels.Length)
            {
                Debug.LogError("Not enough levelPositions to draw all levels");
                return;
            }
            
            _jsonFile = Resources.Load<TextAsset>("levels");

            MapsJson mapsInJson = JsonUtility.FromJson<MapsJson>(_jsonFile.text);
            foreach (LevelJson level in mapsInJson.maps[indexMap].levels)
            {
                int index = level.level - 1;
                
                // Instantiate level on levelPositions
                Level l = levels[index];
                l.transform.position = levelPositions[index].position;
                l.SetIndexMap(indexMap);
                l._unlocked += On_unlocked;
                l._completed += On_completed;
            }
        }

        private void On_completed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void On_unlocked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // Draw gizmo for levelPositions
        private void OnDrawGizmos()
        {
            if (levelPositions == null) return;
            foreach (Transform t in levelPositions)
                Gizmos.DrawSphere(t.position, 0.5f);
        }
    }
}