using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BonusType
{
    FireRate,
    Health
}

public class Bonus : ObstacleMiniGame
{
    public Action OnBonus;
    public BonusType type;

    private void Update()
    {
        base.Move();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            AudioManager.instance.Play("Bonus");
            OnBonus?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bullet bullet))
        {
            TakeDamage(bullet.Damage());
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DeathArea")
        {
            Destroy(gameObject);
        }
    }
}
