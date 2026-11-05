using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    Rigidbody2D Rb;
    private bool _hasRB = false;
    
    Vector2 movementInput;
    [SerializeField] private int starCount = 0;
    
    
    // Animations
    [SerializeField] private GameObject animatedSprite;

    private bool hasAnimator = false;
    private Animator _animator;
    
    void Start()
    {
        if (this.TryGetComponent(out Rigidbody2D x))
        {
            Rb = x;
            _hasRB = true;
        }

        if (animatedSprite.TryGetComponent(out Animator y))
        {
            _animator = y;
            hasAnimator = true;
        }
        else if (this.TryGetComponent(out Animator z))
        {
            _animator = z;
            hasAnimator = true;
        }
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (_hasRB)
        {
            Move();
        }

        Move();
        if (hasAnimator)
        {
            Animate();
        }
    }

    void Move()
    {
        Rb.linearVelocity = new Vector2(movementInput.x, movementInput.y).normalized * moveSpeed;
        Animate();
    }
    
    void Animate()
    {
        
        if (Mathf.Abs(movementInput.x) > 0)
        {
            _animator.speed = 1f;
            if (movementInput.x < 0)
            {
                _animator.Play("Left");
                return;
            } _animator.Play("Right");
        } else if (MathF.Abs(movementInput.y) > 0)
        {
            _animator.speed = 1f;
            if (movementInput.y < 0)
            {
                _animator.Play("Down");
                return;
            } _animator.Play("Up");
        }
        else
        {
            _animator.Play("Idle");
        }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Star")
        {
            starCount++;
        }
    }
}
}
