using UnityEngine;

public class DeleteGameObject : MonoBehaviour
{
    [SerializeField] private float limitPosition;
    void Update()
    {
        if (transform.position.x <= limitPosition)
        {
            Destroy(gameObject);
        }
    }
}
