using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints;

    private SpriteRenderer sprite;

    private GoalManager goalManager;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        goalManager = FindObjectOfType<GoalManager>();
    }

    private void Update()
    {
        if (hitPoints <= 0)
        {
            //Цель уничтожение плитки
            if (goalManager != null)
            {
                goalManager.CompareGoal(this.gameObject.tag);
                goalManager.UpdateGoals();
            }

            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        MakeLighter();
    }

    void MakeLighter()
    {
        //Получение текущего цвета
        Color color = sprite.color;

        //Получить альфа-значение текущего цвета
        float newAlpha = color.a * .5f;

        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
