using System.Collections;
using System.Linq;
using Manager;
using ScriptableObjects;
using Unit;
using UnityEngine;

public class FightLoader : MonoBehaviour
{
    public Animator transition;
    [SerializeField] private Transform[] spawns;
    private Enemy[] _enemies;
    private LevelManager _levelManager;

    private void Awake()
    {
        //_enemies = Resources.LoadAll<Enemy>("Enemies/");
        //foreach (Enemy enemy in _enemies) Debug.Log(enemy.name);
    }

    public void NextFight(GameState gameState)
    {
        _levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        UnitSo[] enemyListSo = _levelManager.level.enemies;
        _enemies = new Enemy[enemyListSo.Length];
        for (int i = 0; i < enemyListSo.Length; i++)
        {
            _enemies[i] = enemyListSo[i].prefab.GetComponent<Enemy>();
        }
        
        foreach (Unit.Unit enemy in gameState.UnitsEnemy) Destroy(enemy);
        StartCoroutine(LoadNextFight(gameState));
    }

    public Unit.Unit[] GenerateEnemies()
    {
        Unit.Unit[] enemies = { };
        for (var i = 0; i < Random.Range(1, 4); i++)
        {
            Enemy mob = _enemies[Random.Range(0, _enemies.Length)];
            mob = Instantiate(mob, spawns[i]);
            enemies = enemies.Concat(new[] { mob }).ToArray();
        }

        return enemies;
    }

    private IEnumerator LoadNextFight(GameState gameState)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        GameState.EnemiesGeneratedArgs args = new(GenerateEnemies());
        gameState.EnemiesGenerated(args);
    }
}