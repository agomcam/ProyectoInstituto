using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float incrementSpeed = 0.2f;
    [SerializeField] private float timeIncrement = 1f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private Renderer _background;

    private void Start()
    {
        StartCoroutine(AddSpeedRoutine());
    }

    void Update()
    {
        _background.material.mainTextureOffset += new Vector2(speed, 0) * Time.deltaTime;

    }

    IEnumerator AddSpeedRoutine()
    {
        while (speed < maxSpeed)
        {
            yield return new WaitForSeconds(timeIncrement);
            speed += incrementSpeed;
            speed = Mathf.Min(speed, maxSpeed); 
        }
    }
   
}