using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 2f;
    private Animator _animator;
    [SerializeField]private LayerMask _layerMask;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.left, raycastDistance, _layerMask);
    
        if (raycastHit2D.collider != null)
        {
            Debug.Log("Detectado: " + raycastHit2D.collider.gameObject.name);
            _animator.SetBool("isAtack", true);
        }
        else
        {
            _animator.SetBool("isAtack", false);

        }
    }
    

}
