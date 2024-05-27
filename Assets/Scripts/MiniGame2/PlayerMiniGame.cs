using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMiniGame : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private int health;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootDelay;
    [SerializeField] private Text healthText;
    public int score;
    private int startScore;
    [SerializeField] private Text scoreText;
    private int maxHealth;
    public Action OnScoreIncrease;
    public Action OnLose;

    private void Awake()
    {
        score = 0;
        startScore = score;
        scoreText.text = $"score: {score}";
    }

    private void Start()
    {
        maxHealth = health + 2;
        healthText.text = $"x{health}";
        shootDelay = fireRate;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void FixedUpdate()
    {
        if (shootDelay < fireRate)
            shootDelay += Time.fixedDeltaTime;
        PlayerShoot();
    }

    public void SetVariables(float FireRate, float Damage, int Health)
    {
        fireRate = FireRate;
        damage = Damage;
        health = Health;
    }

    private void PlayerMovement()
    {
        if (Time.timeScale != 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(Mathf.Clamp(mousePosition.x, -1.79f, 1.79f), transform.position.y);
        }
    }

    private void PlayerShoot()
    {
        if (Input.GetMouseButton(0) && shootDelay >= fireRate)
        {
            shootDelay = 0;
            GameObject go = Instantiate(projectile);
            AudioManager.instance.Play("Pop1");
            go.transform.position = transform.position;
            go.GetComponent<Bullet>().SetDamage(damage);
        }
    }

    public void SetConfiguration(float FireRate, float Damage, int Health)
    {
        fireRate = FireRate;
        damage = Damage;
        health = Health;
    }

    public void HealPlayer()
    {
        if (health < maxHealth)
        {
            health++;
            healthText.text = $"x{health}";
        }
    }

    public void IncreaseFireRate()
    {
        fireRate = fireRate > 0.5f ? fireRate -= 0.1f : fireRate;
    }

    public void OnEnemyDie()
    {
        score++;
        scoreText.text = $"score: {score}";
        if (startScore + 3 == score)
        {
            startScore = score;
            OnScoreIncrease?.Invoke();
        }
    }

    public void OnTakeDamage()
    {
        health--;
        healthText.text = $"x{health}";
        if (health <= 0)
        {
            AudioManager.instance.Play("LoseMiniGame1");
            GameLose();
        }
    }

    public void GameLose()
    {
        OnLose?.Invoke();
    }

    public int CalculateScore(int Score)
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (PlayerPrefs.GetInt("BestScore") > score)
            {
                score = PlayerPrefs.GetInt("BestScore");
                return score;
            }
            else
            {
                PlayerPrefs.SetInt("BestScore", score);
                return score;
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
            return score;
        }
    }
}
