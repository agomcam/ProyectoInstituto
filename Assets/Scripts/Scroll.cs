using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float speed;

    
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

    }
}
