using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Renderer _background;

    void Start()
    {
    }

    void Update()
    {
        _background.material.mainTextureOffset += new Vector2(speed, 0) * Time.deltaTime;
    }
}