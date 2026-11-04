using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] private GameObject animatedSprite;

    private bool hasAnimator = false;
    private Animator _animator;
    
    Rigidbody2D Rb;
    Vector2 movementInput;
    void Start()
    {
        if (this.TryGetComponent(out Rigidbody2D x))
        {
            Rb = x;
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
        Move();
        if (hasAnimator)
        {
            Animate();
        }
    }

    void Move()
    {
        Rb.linearVelocity = new Vector2(movementInput.x, movementInput.y).normalized * moveSpeed;
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
    }
}
