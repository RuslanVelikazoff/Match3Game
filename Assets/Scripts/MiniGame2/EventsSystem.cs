using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventsSystem : MonoBehaviour
{
    [SerializeField] private PlayerMiniGame player;
    [SerializeField] private SpawnerConfig spawnerConfig;
    [SerializeField] private SpawnerConfig[] configDifficult;
    [SerializeField] private Transform spawnPos;
    float spawnRate;
    float spawnDelay;
    [SerializeField] private Button menuButton;
    [SerializeField] private Transform loseMenu;


    private void Awake()
    {
        Time.timeScale = 1;
        SetDifficult();
        player.SetVariables(spawnerConfig.GetPlayerFireRate(), spawnerConfig.GetPlayerDamage(), spawnerConfig.GetPlayerHealth());
        spawnRate = spawnerConfig.GetSpawnRate();
        spawnDelay = spawnRate;
    }

    private void Start()
    {
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }

    private void OnEnable()
    {
        player.OnScoreIncrease += DecreaseSpawnRate;
        player.OnLose += () => LoseGame();
    }

    private void OnDisable()
    {
        player.OnScoreIncrease -= DecreaseSpawnRate;
        player.OnLose -= () => LoseGame();
    }

    private void OnDestroy()
    {
        player.OnScoreIncrease -= DecreaseSpawnRate;
        player.OnLose -= () => LoseGame();
    }

    private void FixedUpdate()
    {
        if (spawnDelay < spawnRate)
            spawnDelay += Time.fixedDeltaTime;
        SpawnEnemyies();
    }

    private void LoseGame()
    {
        Time.timeScale = 0f;
        loseMenu.gameObject.SetActive(true);
    }

    private void SetDifficult()
    {
        int difficult = 2;
        switch (difficult)
        {
            case 1:
                spawnerConfig = configDifficult[0];
                break;
            case 2:
                spawnerConfig = configDifficult[1];
                break;
            case 3:
                spawnerConfig = configDifficult[2];
                break;
        }
    }

    private void SpawnEnemyies()
    {
        if(spawnDelay >= spawnRate)
        {
            spawnDelay = 0;
            int bonusChance = Random.Range(0, 5);
            if (bonusChance == 4)
            {
                GameObject bonus = Instantiate(spawnerConfig.GetBonuses()[Random.Range(0, spawnerConfig.GetBonuses().Count)]);
                bonus.transform.position = spawnPos.position + new Vector3(Random.Range(-1.65f, 1.65f), 0);
                switch(bonus.GetComponent<Bonus>().type)
                {
                    case BonusType.FireRate:
                        bonus.GetComponent<Bonus>().OnBonus += player.IncreaseFireRate;
                        break;
                    case BonusType.Health:
                        bonus.GetComponent<Bonus>().OnBonus += player.HealPlayer;
                        break;
                }
            }
            else
            {
                GameObject enemy = Instantiate(spawnerConfig.GetEnemies()[Random.Range(0, spawnerConfig.GetEnemies().Count)]);
                enemy.transform.position = spawnPos.position + new Vector3(Random.Range(-1.65f, 1.65f), 0);
                enemy.GetComponent<Enemy>().OnDie += player.OnEnemyDie;
                enemy.GetComponent<Enemy>().OnDamage += player.OnTakeDamage;
            }
        }
    }

    private void DecreaseSpawnRate()
    {
        spawnRate = spawnRate >= 0.7 ? spawnRate -= 0.3f : spawnRate;
    }
}
