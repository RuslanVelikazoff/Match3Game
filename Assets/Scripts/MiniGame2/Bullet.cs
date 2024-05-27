using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float damage;
    [SerializeField] private float speed;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void Update()
    {
        transform.position += new Vector3(0,speed * Time.deltaTime);
    }
    public void SetDamage(float Damage)
    {
        damage = Damage;
    }

    public float Damage()
    {
        return damage;
    }
}
