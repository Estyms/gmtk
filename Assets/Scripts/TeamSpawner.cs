using ScriptableObjects;
using UnityEngine;

public class TeamSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    private UnitSo[] _team;
    private Unit.Unit[] _units;

    private void Awake()
    {
        _units = new Unit.Unit[spawnPoints.Length];
        _team = Resources.Load<TeamListSo>("ActualTeam").teamList;
    }

    public void Start()
    {
        int index = 0;
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject g = Instantiate(_team[index].prefab, spawnPoint.position, spawnPoint.rotation);
            _units[index] = g.GetComponent<Unit.Unit>();
            index++;
        }
    }

    public Unit.Unit[] Units => _units;
}