using UnityEngine;

public class Bomb : MonoBehaviour
{
    private static readonly float ForceX = 0.0f;
    private static readonly float ForceY = 0.0f;
    private static readonly float VelocityX = -3.0f;
    private static readonly float VelocityY = 0.0f;
    public int damage = 50;
    private Rigidbody2D _rigidBody;
    private Vector2 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;

        float randomOffsetVelocityX = Random.Range(-2, 2);

        _rigidBody.velocity = new Vector2(VelocityX + randomOffsetVelocityX, VelocityY);
        _rigidBody.AddForce(new Vector2(ForceX, ForceY), ForceMode2D.Force);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -4.5) Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            var managerObject = GameObject.Find("ResourceManager").gameObject;
            var manager = managerObject.GetComponent<ResourceManager>();
            Debug.LogError("Enemy Collision");
            Debug.Log(collision.gameObject);
            collision.gameObject.SendMessage("DamageEnemy", damage);
            manager.UpdateProjectileDamage(this.GetType().Name,damage);
            Die();
        }
    }
    
    private void Die()
    {
        // Debug.Log("Arrow - Die()");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
