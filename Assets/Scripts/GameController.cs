using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; } // Singleton

    [SerializeField] private float baseSpeed = 0.1f;
    private float speed;
    public float SpeedEnemyAndFloor { get; private set; } = 4f;
    private int _points;
    private int lastThreshold = 0; // Último umbral alcanzado
    [SerializeField] private float incrementSpeed = 0.2f;
    [SerializeField] private float incrementSpeedEnemys = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private int pointsThreshold = 10;
    
    [SerializeField] private Renderer background;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelGameStart;

    [SerializeField] private GameObject[] listEnemy;
    [SerializeField] private GameObject positionEnemy;

    [SerializeField] private TMP_Text _PointsTMP;
    
    private bool isRunning = true;
    private bool isStart = true;
    private bool playerDead;
    private float spawnRate = 3f;

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
        speed = baseSpeed;
        StartCoroutine(InstantiateEnemy());
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

    private void UpdateSpeed()
    {
        if (speed < maxSpeed)
        {
            speed += incrementSpeed;
        }

        if (SpeedEnemyAndFloor < maxSpeed)
        {
            SpeedEnemyAndFloor += incrementSpeedEnemys;
        }

        UpdateSpawnRate();
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
        StartCoroutine(InstantiateEnemy());
    }

    public void AddPoints(int points)
    {
        _points += points;
        _PointsTMP.text = $"Puntuación: {_points}";

        if (_points >= lastThreshold + 5) // Cada vez que superamos el siguiente múltiplo de 5
        {
            lastThreshold += 5; // Actualizamos el umbral
            UpdateSpeed(); // Incrementamos la velocidad
        }
    }
}
