using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    Rigidbody2D Rb;
    Vector2 movementInput;
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Rb.MovePosition(Rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
    }
}
