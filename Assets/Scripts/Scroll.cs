using UnityEngine;

public class Scroll : MonoBehaviour
{
 
    private float speed;

    private void Start()
    {
        if (GameController.Instance != null)
        {
            speed = GameController.Instance.SpeedEnemyAndFloor;
        }
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

    }

    public void stopScroll()
    {
        speed = 0;
    }
}
