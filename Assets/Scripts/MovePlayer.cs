using System;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float jump;
    [SerializeField] private float rayCastDistance = 0.2f;
    [SerializeField] private Transform positionRaycast;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    
    private RaycastHit2D _raycastHit2D;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }

        if (!isGrounded && Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rigidbody2D.AddForce(Vector2.down * jump, ForceMode2D.Impulse);
        }

        _animator.SetBool("isJumping", !isGrounded);
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
            Destroy(gameObject);
            GameController.Instance.StopGame();
        }
    }
}