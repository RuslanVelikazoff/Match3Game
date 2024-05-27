using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ObstacleMiniGame
{
    public Action OnDamage;
    public Action OnDie;

    void Update()
    {
        base.Move();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (health <= 0)
        {
            AudioManager.instance.Play("Pop1");
            OnDie?.Invoke();
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

        if(collision.tag == "DeathArea")
        {
            OnDamage?.Invoke();
        }
    }
}
