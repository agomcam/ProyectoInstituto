using System;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float jump;
    [SerializeField] private float rayCastDistance = 0.2f;
    [SerializeField] private Transform positionRaycast;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private AudioSource audioSourceJump;
    [SerializeField] private AudioSource audioSourceDeath;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 startTouchPosition, endTouchPosition;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        // Controles de teclado
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if (!isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            Descend();
        }
        
        // Controles táctiles
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDirection = endTouchPosition - startTouchPosition;

                if (swipeDirection.y > 50 && isGrounded) // Deslizar arriba
                {
                    Jump();
                }
                else if (swipeDirection.y < -50 && !isGrounded) // Deslizar abajo
                {
                    Descend();
                }
            }
        }

        _animator.SetBool("isJumping", !isGrounded);

        if (GameController.Instance.Points >= 10)
        {
            _animator.SetBool("level1", false);
            _animator.SetBool("level2", false);
            _animator.SetBool("level3", true);

            GameController.Instance.level1 = false;
            GameController.Instance.level2 = false;
            GameController.Instance.level3 = true;
            GameController.Instance.AddSpeedSoundBackground(1.2f);
        }
        else if (GameController.Instance.Points >= 5)
        {
            _animator.SetBool("level1", false);
            _animator.SetBool("level2", true);
            _animator.SetBool("level3", false);

            GameController.Instance.level1 = false;
            GameController.Instance.level2 = true;
            GameController.Instance.level3 = false;
            GameController.Instance.AddSpeedSoundBackground(1.1f);
        }
    }

    private void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        audioSourceJump.Play();
    }

    private void Descend()
    {
        _rigidbody2D.AddForce(Vector2.down * jump, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(positionRaycast.position, Vector2.down, rayCastDistance, groundLayer);
        Debug.DrawRay(positionRaycast.position, Vector2.down * rayCastDistance, Color.red);
        return hit.collider != null && hit.collider.CompareTag("floor");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            audioSourceDeath.Play();
            Destroy(gameObject);
            GameController.Instance.StopGame();
        }
    }
}
