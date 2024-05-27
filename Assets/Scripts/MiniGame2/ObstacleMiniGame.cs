using UnityEngine;

public class ObstacleMiniGame : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float speed;

    public virtual void Move()
    {
        transform.position += new Vector3(0, -speed * Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }
}
