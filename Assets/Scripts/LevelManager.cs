using JSON;
using Manager;
using ScriptableObjects;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelSo level;
    public int mapIndex;
    private TextAsset _jsonFile;
    private ScenesManager _scenesManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _scenesManager = GameObject.Find("ScenesManager").GetComponent<ScenesManager>();
    }

    public void LevelCompleted()
    {
        _jsonFile = Resources.Load<TextAsset>("levels");
        LevelJson levelJson = JsonUtility.FromJson<MapsJson>(_jsonFile.text).maps[mapIndex].levels[level.level];

        levelJson.isCompleted = true;
        
        _scenesManager.LoadScene("Map");
    }
}
