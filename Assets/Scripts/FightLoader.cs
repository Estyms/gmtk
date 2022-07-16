using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FightLoader : MonoBehaviour
{

    public Animator transition;
    private Enemy[] _enemies;
    [SerializeField] private Transform[] spawns;
    
    
    private void Awake()
    {
        _enemies = Resources.LoadAll<Enemy>("Enemies/");
        foreach (var enemy in _enemies)
        {
            Debug.Log(enemy.name);
        }
    }

    public void NextFight(GameState gameState)
    {
        foreach (var enemy in gameState.UnitsEnemy)
        {
            Destroy(enemy);   
        }
        StartCoroutine(LoadNextFight(gameState));
    }
    public Unit[] GenerateEnemies()
    {
        Unit[] enemies = { };
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            var mob = _enemies[Random.Range(0, _enemies.Length)];
            mob = Instantiate(mob, spawns[i]);
            enemies = enemies.Concat(new []{mob}).ToArray();
        }

        return enemies;
    } 

    IEnumerator LoadNextFight(GameState gameState)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        var args = new GameState.EnemiesGeneratedArgs(GenerateEnemies());
        gameState.EnemiesGenerated(args);
        
    }
}
