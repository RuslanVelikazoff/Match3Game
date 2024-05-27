using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float posY = 12f;
    [SerializeField] private float posX = 16f;
    
    [SerializeField] private Sprite yellowSprite;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Sprite blueSprite;

    private int type;
    private int score;

    private void Start()
    {
        type = Random.Range(1, 11);
        type = type < 8 ? 0 : type < 10 ? 1 : 2;
        GetComponent<SpriteRenderer>().sprite = type == 0 ? yellowSprite : type == 1 ? blackSprite : blueSprite;
        score = type == 0 ? 1 : type == 1 ? 0 : 10;
        Vector3 pos = transform.position;
        pos.x = Random.Range(-posX, posX);
        pos.y = posY;
        transform.position = pos;
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(type == 0 ||  type == 2)
            {
                AudioManager.instance.Play("WinMiniGame1");
                MiniGame1Manager.Instance.UpdateScore(score);
            }
            else
            {
                AudioManager.instance.Play("LoseMiniGame1");
                MiniGame1Manager.Instance.GameOver();
            }
            Destroy(gameObject);
        }

        if(collision.CompareTag("Finish"))
        {
            if(type == 0)
            {
                AudioManager.instance.Play("LoseMiniGame1");
                MiniGame1Manager.Instance.GameOver();
            }
            Destroy(gameObject);
        }
    }
}
