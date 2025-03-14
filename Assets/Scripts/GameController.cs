using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // -------------------- SINGLETON --------------------
    public static GameController Instance { get; private set; }

    // -------------------- NIVELES --------------------
    [Header("Configuración de Niveles")]
    public bool level1 { get; set; } = true;
    public bool level2 { get; set; }
    public bool level3 { get; set; }

    // -------------------- VELOCIDAD Y DIFICULTAD --------------------
    [Header("Velocidad y Dificultad")]
    [SerializeField] private float baseSpeed = 0.1f;
    private float speed;
    public float SpeedEnemyAndFloor { get; private set; } = 4f;

    [SerializeField] private float incrementSpeed = 0.2f;
    [SerializeField] private float incrementSpeedEnemys = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxSpeedEnemy = 10f;

    private int lastThreshold = 0; // Último umbral alcanzado
    [SerializeField] private int pointsThreshold = 10;

    // -------------------- SISTEMA DE PUNTOS --------------------
    [Header("Puntuación")]
    public int Points { get; private set; }
    [SerializeField] private TMP_Text PointsTMP;

    // -------------------- CONTROL DEL JUEGO --------------------
    [Header("Estado del Juego")]
    private bool isRunning = true;
    private bool isStart = true;
    private bool playerDead;

    // -------------------- UI Y SONIDO --------------------
    [Header("Interfaz y Sonido")]
    [SerializeField] private Renderer background;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelGameStart;
    [SerializeField] private AudioSource audioSource;

    // -------------------- SPAWN DE ENEMIGOS --------------------
    [Header("Spawn de Enemigos")]
    [SerializeField] private GameObject[] listEnemy;
    [SerializeField] private GameObject positionEnemy;
    private float spawnRate = 3f;

    // -------------------- SISTEMA DE JEFES --------------------
    [Header("Jefes")]
    [SerializeField] private GameObject[] bossList; 
    private bool isBossActive = false; 
    private GameObject currentBoss; 
    [SerializeField] private int bossSpawnThreshold = 5; 

    // -------------------- MÉTODOS PRINCIPALES --------------------
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        speed = baseSpeed;
        StartCoroutine(InstantiateEnemy());
    }

    private void Update()
    {
        if (isRunning && background)
            background.material.mainTextureOffset += new Vector2(speed, 0) * Time.deltaTime;

        if (playerDead && Input.anyKeyDown)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (isStart && Input.anyKeyDown)
        {
            panelGameStart.SetActive(false);
            isStart = false;
            StartGame();
        }

        if (Points >= bossSpawnThreshold && !isBossActive)
            StartBossFight();
    }

    // -------------------- CONTROL DE JEFES --------------------
    private void StartBossFight()
    {
        isBossActive = true;
        StopAllCoroutines();
        ClearAllEnemies(); 

        if (bossList.Length > 0)
        {
            int randomIndex = Random.Range(0, bossList.Length);
            currentBoss = Instantiate(bossList[randomIndex], positionEnemy.transform.position, Quaternion.identity);
        }
    }

    public void BossDefeated()
    {
        isBossActive = false;
        bossSpawnThreshold += 10; 
        StartCoroutine(ResumeEnemySpawnAfterDelay(1f));
    }

    private IEnumerator ResumeEnemySpawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(InstantiateEnemy());
    }

    private void ClearAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);
    }

    // -------------------- SISTEMA DE PUNTOS --------------------
    public void AddPoints(int points)
    {
        Points += points;
        PointsTMP.text = $"Puntuación: {Points}";

        if (Points >= lastThreshold + 5) 
        {
            lastThreshold += 5;
            UpdateSpeed();
        }
    }

    // -------------------- VELOCIDAD Y DIFICULTAD --------------------
    private void UpdateSpeed()
    {
        if (speed < maxSpeed)
            speed += incrementSpeed;

        if (SpeedEnemyAndFloor < maxSpeedEnemy)
            SpeedEnemyAndFloor += incrementSpeedEnemys;

        UpdateSpawnRate();
    }

    private void UpdateSpawnRate()
    {
        spawnRate = Mathf.Max(0.5f, 3f - (SpeedEnemyAndFloor / 5f));
    }

    // -------------------- SPAWN DE ENEMIGOS --------------------
    private IEnumerator InstantiateEnemy()
    {
        while (isRunning && !isStart)
        {
            if (listEnemy.Length != 0)
            {
                int randomIndex = Random.Range(0, listEnemy.Length);
                Instantiate(listEnemy[randomIndex], positionEnemy.transform);
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    // -------------------- CONTROL DEL JUEGO --------------------
    private void StartGame()
    {
        isRunning = true;
        audioSource.Play();
        StartCoroutine(InstantiateEnemy());
    }

    public void StopGame()
    {
        isRunning = false;
        speed = 0;

        Scroll[] scrolls = FindObjectsOfType<Scroll>();
        foreach (var scroll in scrolls)
            scroll.stopScroll();

        panelGameOver.SetActive(true);
        playerDead = true;
        audioSource.Stop();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // -------------------- CONTROL DE SONIDO --------------------
    public void AddSpeedSoundBackground(float speed)
    {
        audioSource.pitch = speed;
    }
}
