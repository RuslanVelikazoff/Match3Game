using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "ScriptableObjects/Spawner Configuration", order = 1)]
public class SpawnerConfig : ScriptableObject
{
    [SerializeField] private List<GameObject> EnemyPrefabs;
    [SerializeField] private List<GameObject> BonusPrefabs;
    [SerializeField] private float SpawnRate;
    [SerializeField] private float playerFireRate;
    [SerializeField] private int playerHealth;
    [SerializeField] private float playerDamage;

    public float GetPlayerDamage()
    {
        return playerDamage;
    }

    public float GetPlayerFireRate()
    {
        return playerFireRate;
    }

    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public List<GameObject> GetEnemies()
    {
        return EnemyPrefabs;
    }

    public List<GameObject> GetBonuses()
    {
        return BonusPrefabs;
    }
    public float GetSpawnRate()
    {
        return SpawnRate;
    }
}
