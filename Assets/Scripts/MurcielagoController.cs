using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MurcielagoController : MonoBehaviour
{
    [Header("Ajustes de Vuelo")]
    public float speed = 3f;
    public float distance = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    private float leftLimit;
    private float rightLimit;
    private int direction = 1; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0; 

        leftLimit = transform.position.x - distance;
        rightLimit = transform.position.x + distance;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        if (transform.position.x >= rightLimit)
        {
            direction = -1;
        }
        else if (transform.position.x <= leftLimit)
        {
            direction = 1;
        }

        if (direction == 1)
        {
            spriteRenderer.flipX = false; 
        }
        else
        {
            spriteRenderer.flipX = true;  
        }
    }
    
   
}