using System;
using JSON;
using Manager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private LevelSo levelSo;
        [SerializeField] private SpriteRenderer circle;
        
        private int _indexMap;
        private bool _isCompleted, _isUnlocked; 
        private TextAsset _jsonFile;
        private ScenesManager _sceneManager;
        private LevelManager _levelManager;
        
        [Header("Requirements")] [SerializeField] private Level[] requirements;

        [Header("Icons")] 
        [SerializeField] private GameObject lockIcon;
        [SerializeField] private GameObject levelCompleted;
        [SerializeField] private GameObject bossIcon;
        [SerializeField] private GameObject markPointIcon;

        private void Start()
        {
            _sceneManager = GameObject.Find("ScenesManager").GetComponent<ScenesManager>();
            _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        }

        private void OnMouseOver()
        {
            circle.color = Color.yellow;
        }

        private void OnMouseExit()
        {
            circle.color = Color.white;
        }

        private void OnMouseDown()
        {
            if (!_isUnlocked) return;
            _levelManager.level = levelSo;
            _levelManager.mapIndex = _indexMap;
            _sceneManager.LoadScene("Game");
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                DisplayIcons();
            }
        }

        public void SetIndexMap(int indexMap)
        {
            _indexMap = indexMap;
            _jsonFile = Resources.Load<TextAsset>("levels");

            LevelJson levelJson = JsonUtility.FromJson<MapsJson>(_jsonFile.text).maps[_indexMap].levels[levelSo.level];

            _isCompleted = levelJson.isCompleted;
            _isUnlocked = levelJson.isUnlocked;
            
            DisplayIcons();
        }

        private void DisplayIcons()
        {
            lockIcon.SetActive(!_isUnlocked);
            levelCompleted.SetActive(_isCompleted);
        }
        
        public void CheckIfUnlocked()
        {
            foreach (Level l in requirements) if (!l._isCompleted) return;
            _isUnlocked = true;
            DisplayIcons();
        }

        public event EventHandler _completed, _unlocked;
    }
}