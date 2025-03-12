using System;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(parent);
        }
    }
}
