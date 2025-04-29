using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float jumpForce = 2.0f;
    public float speed = 1.0f;

    private bool grounded = true;
    private bool jump;
    private bool fly;
    public static bool moving;
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    private SpriteRenderer _spriteRenderer;
    private float moveDirection;
    private int test = 0;

    private float vertical;
    [SerializeField] public GameObject resumeScreen;
    public static bool resume = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        moving = true;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = new Vector2(speed * moveDirection, _rigidbody2D.linearVelocity.y);

        if (jump)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
            jump = false;
            fly = true;
        }

        if (Enemy.isEnemyDeath)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
            Enemy.isEnemyDeath = false;
        }

        if (Climb.isClimbing)
        {
            _rigidbody2D.gravityScale = 0f;
            float verticalInput = 0;
            if (GameInputManager.GetKey("Up")) verticalInput = 1;
            if (GameInputManager.GetKey("Down")) verticalInput = -1;

            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, verticalInput * 1.5f * speed);
        }
        else
        {
            _rigidbody2D.gravityScale = 0.9f;
        }

        if (Death.isDeath)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.linearVelocity = new Vector2(0, jumpForce / 3);
            CharacterControl.moving = false;
        }
        else
        {
            CharacterControl.moving = true;
        }
    }

    private void Update()
    {
        if (moving && !resume)
        {
            bool leftPressed = GameInputManager.GetKey("Left");
            bool rightPressed = GameInputManager.GetKey("Right");
            bool horizontalInput = leftPressed || rightPressed;

            if (grounded && horizontalInput)
            {
                if (leftPressed)
                {
                    moveDirection = -1.0f;
                    _spriteRenderer.flipX = true;
                    anim.SetFloat("speed", speed);
                }
                else if (rightPressed)
                {
                    moveDirection = 1.0f;
                    _spriteRenderer.flipX = false;
                    anim.SetFloat("speed", speed);
                }
            }
            else if (grounded)
            {
                moveDirection = 0.0f;
                anim.SetFloat("speed", 0.0f);
            }

            if (fly)
            {
                if (leftPressed)
                {
                    moveDirection = -1.0f;
                    _spriteRenderer.flipX = true;
                }
                else if (rightPressed)
                {
                    moveDirection = 1.0f;
                    _spriteRenderer.flipX = false;
                }
            }

            if (grounded && GameInputManager.GetKey("Up"))
            {
                if (Climb.isLadder)
                {
                    Climb.isClimbing = true;
                    anim.SetTrigger("climb");
                }
                else
                {
                    jump = true;
                    anim.SetTrigger("jump");
                }
                grounded = false;
                anim.SetBool("grounded", false);
            }

            if (Climb.isLadder && !grounded && (GameInputManager.GetKey("Left") || GameInputManager.GetKey("Down")))
            {
                grounded = true;
                anim.SetBool("grounded", true);
                Climb.isClimbing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!resume)
            {
                resume = true;
                Time.timeScale = 0;
                resumeScreen.SetActive(true);
            }
            else
            {
                resume = false;
                Time.timeScale = 1;
                resumeScreen.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            fly = false;
            anim.SetBool("grounded", true);
        }
    }
}