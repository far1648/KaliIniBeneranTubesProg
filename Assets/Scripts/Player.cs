using UnityEngine;
using System;
using Unity.Mathematics;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    Rigidbody2D Rb;
    private bool _hasRB = false;
    
    Vector2 movementInput;
    [SerializeField] public int starCount = 0;

    // HP section
    [SerializeField] private int maxHP = 4;
    [SerializeField] public int HP;
    [SerializeField] private float iFrameSeconds = 1.5f;
    [SerializeField] private bool Dead = false;

    private bool charTranslucent = false; // for the blinking effect on iFrame
    private float blinkInterval = .1f;
    private bool isIFrame = false;
    private float currentIFrameSeconds = 0f;
    
    // Animations
    [SerializeField] private GameObject animatedSprite;

    private bool hasAnimator = false;
    private Animator _animator;
    [SerializeField] float flashDuration = 0.15f;
    private SpriteRenderer _sr;
    private Coroutine flashRoutine;
    
    void Start()
    {
        if (TryGetComponent(out Rigidbody2D x))
        {
            Rb = x;
            _hasRB = true;
        }

        if (animatedSprite.TryGetComponent(out Animator y))
        {
            _animator = y;
            hasAnimator = true;
        }
        else if (TryGetComponent(out Animator z))
        {
            _animator = z;
            hasAnimator = true;
        }

        HP = maxHP;

        _sr = animatedSprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // HP Systems
        if (!Dead)
        {
            if (isIFrame)
            {
                IFrameVoid();
            }
            
            // Animations
            if (hasAnimator)
            {
                if (charTranslucent)
                {
                    animatedSprite.GetComponent<SpriteRenderer>().enabled = false;
                }
                else
                {
                    animatedSprite.GetComponent<SpriteRenderer>().enabled = true;
                }
                Animate();
            }
            
            // Player movement
            if (_hasRB)
            {
                Move();
            }
            
            // When HP <= 0, dead.
            if (HP <= 0)
            {
                Die();
            }
            
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

    private void Die()
    {
        maxHP = 0;
        movementInput = Vector2.zero;
        Move();
        Animate();
        if (flashRoutine!=null) StopCoroutine(flashRoutine);
        animatedSprite.GetComponent<SpriteRenderer>().color = Color.red;
        
        Dead = true;
    }

    public void TakeDamage(int dmg=1)
    {
        if (!isIFrame && !Dead)
        {
            HP -= dmg;
            isIFrame = true;
            Flash(Color.red);
        }
    }

    private void IFrameVoid()
    {
        // Invincibility Frames after damage
        if (currentIFrameSeconds >= iFrameSeconds)
        {
            isIFrame = false;
            currentIFrameSeconds = 0;
            charTranslucent = false;
        }
        else
        {
            currentIFrameSeconds += Time.deltaTime;
            if ((int) Mathf.Round(currentIFrameSeconds*100) % (int) Mathf.Round(blinkInterval*100) == 0)
            {
                charTranslucent = !charTranslucent;
            }
        }
    }

    public void Heal(int healing=1)
    {
        int before = HP;
        HP = Mathf.Min(HP + healing, maxHP);
        if (HP > before) Flash(new Color(0.4f, 1f, 0.4f));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Star")
        {
            starCount++;
            Heal();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage();
        }
    }

    void Flash(Color color)
    {
        if (_sr == null || Dead) return;
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine(color));
    }

    IEnumerator FlashRoutine(Color color)
    {
        _sr.color = color;
        yield return new WaitForSeconds(flashDuration);
        _sr.color = Color.white;
        flashRoutine = null;
    }
}