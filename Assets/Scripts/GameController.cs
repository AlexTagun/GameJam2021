﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject flashlightPrefab = null;
    [SerializeField] private float delayRestartAfterDeath = 7f;

    public Player Player { get; private set; }

    public Action<bool> OnGameOver;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this) Destroy(gameObject);

        Player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        if (RememberFlashlight.Instance.NeedSpawnFlashLight)
        {
            CreateFlashlight(RememberFlashlight.Instance.GetLastPosition());
        }
        if (RememberFlashlight.Instance.NeedActivateRoad)
        {
            var winPlace = RememberFlashlight.Instance.GetWinPlace();
            winPlace.Activate();
            SpawnerRoad.Instance.SetIsRoadSpawn(true);
        }
    }

    public void HandleGameOver(bool isWin)
    {
        OnGameOver?.Invoke(isWin);

        RememberFlashlight.Instance.ReactToEndGame(isWin);
        Restart();
    }

    public void Restart()
    {
        StartCoroutine(RestartScene(delayRestartAfterDeath));
    }


    private IEnumerator RestartScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




    private void CreateFlashlight(Vector3 pos)
    {
        var newFlashlight = Instantiate(flashlightPrefab, pos, Quaternion.identity);
    }


}
