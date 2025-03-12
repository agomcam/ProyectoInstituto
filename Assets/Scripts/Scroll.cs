using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float restartPosition;
    [SerializeField] private float limitPosition;
    
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x <= limitPosition)
        {
            transform.position = new Vector3(restartPosition, 0, 0);
        }
    }
}
