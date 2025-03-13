using System.Collections;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } // Singleton

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float incrementSpeed = 0.2f;
    [SerializeField] private float timeIncrement = 1f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private Renderer background;

    [SerializeField] private GameObject[] listEnemy;
    [SerializeField] private GameObject positionEnemy;

    private bool isRunning = true;
    private Coroutine enemyRoutine;
    private Coroutine speedRoutine;

    private void Awake()
    {
        // Implementación Singleton: Garantiza que solo haya una instancia
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Mantener instancia entre escenas
    }

    private void Start()
    {
        enemyRoutine = StartCoroutine(InstantiateEnemy());
        speedRoutine = StartCoroutine(AddSpeedRoutine());
    }

    private void Update()
    {
        if (isRunning && background != null)
        {
            background.material.mainTextureOffset += new Vector2(speed, 0) * Time.deltaTime;
        }
    }

    private IEnumerator AddSpeedRoutine()
    {
        while (isRunning && speed < maxSpeed)
        {
            yield return new WaitForSeconds(timeIncrement);
            speed += incrementSpeed;
            speed = Mathf.Min(speed, maxSpeed);
        }
    }

    private IEnumerator InstantiateEnemy()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(3);
            int randomNumber = Random.Range(0, listEnemy.Length - 1);
            Instantiate(listEnemy[randomNumber], positionEnemy.transform);
        }
    }

    public void StopGame()
    {
        isRunning = false;
        speed = 0;

        // Detener las corrutinas activas
        if (enemyRoutine != null)
        {
            StopCoroutine(enemyRoutine);
            enemyRoutine = null;
        }

        if (speedRoutine != null)
        {
            StopCoroutine(speedRoutine);
            speedRoutine = null;
        }

        // Detener objetos Scroll en la escena
        Scroll[] scrolls = FindObjectsByType<Scroll>(FindObjectsSortMode.None);
        foreach (var scrollAux in scrolls)
        {
            scrollAux.stopScroll();
        }
    }
}
