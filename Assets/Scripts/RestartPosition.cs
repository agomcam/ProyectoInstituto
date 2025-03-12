using UnityEngine;

public class RestartPosition : MonoBehaviour
{
    [SerializeField] private float restartPosition;
    [SerializeField] private float limitPosition;
    void Update()
    {
        if (transform.position.x <= limitPosition)
        {
            transform.position = new Vector3(restartPosition, 0, 0);
        }
    }
}
