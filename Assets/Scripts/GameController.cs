using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } // Singleton

    [SerializeField] private float speed = 0.1f;
    public float SpeedEnemyAndFloor { get; set; } = 2f;

    [SerializeField] private float incrementSpeed = 0.2f;
    [SerializeField] private float incrementSpeedEnemys = 0.5f;
    [SerializeField] private float timeIncrement = 1f;
    [SerializeField] private float maxSpeed = 1f;

    [SerializeField] private Renderer background;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelGameStart;

    [SerializeField] private GameObject[] listEnemy;
    [SerializeField] private GameObject positionEnemy;

    private bool isRunning = true;
    private bool isStart = true;
    private bool playerDead;

    private Coroutine enemyRoutine;
    private Coroutine speedRoutine;

    private float spawnRate = 3f; // Tiempo inicial entre spawns

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

        if (playerDead && Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (isStart && Input.anyKeyDown)
        {
            panelGameStart.SetActive(false);
            isStart = false;
            StartGame();
        }
    }

    private IEnumerator AddSpeedRoutine()
    {
        while (isRunning && !isStart && speed < maxSpeed)
        {
            yield return new WaitForSeconds(timeIncrement);
            speed += incrementSpeed;
            speed = Mathf.Min(speed, maxSpeed);
            SpeedEnemyAndFloor += incrementSpeedEnemys;

            UpdateSpawnRate();
        }
    }

    private void UpdateSpawnRate()
    {
        spawnRate = Mathf.Max(0.5f, 3f - (SpeedEnemyAndFloor / 5f));
    }

    private IEnumerator InstantiateEnemy()
    {
        while (isRunning && !isStart)
        {
            if (listEnemy.Length != 0)
            {
                int randomNumber = Random.Range(0, listEnemy.Length);
                Instantiate(listEnemy[randomNumber], positionEnemy.transform);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void StopGame()
    {
        isRunning = false;
        speed = 0;

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

        Scroll[] scrolls = FindObjectsOfType<Scroll>();
        foreach (var scroll in scrolls)
        {
            scroll.stopScroll();
        }

        panelGameOver.SetActive(true);
        playerDead = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        isRunning = true;
        enemyRoutine = StartCoroutine(InstantiateEnemy());
        speedRoutine = StartCoroutine(AddSpeedRoutine());
    }
}
