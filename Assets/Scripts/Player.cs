﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour {

    public float NoiseLevel = 1;
    public float VisibilityLevel { get; private set; }

    public void SetVisibilityLevel(bool isOn)
    {
        VisibilityLevel = (isOn) ? visibilityLevelOn : visibilityLevelOff;
    }

    public int NumberAttempts = 3;

    [SerializeField] private float visibilityLevelOn = 5;
    [SerializeField] private float visibilityLevelOff = 1;
    [SerializeField] private PlayableDirector playableDirectorFalling;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip[] PlayerVoices;

    private PlayerMovement playerMovement = null;
    public FlashlightController flashlightController = null;
    
    private ColorAdjustments _colorAdjustments;

    private bool isAlive = true;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        flashlightController = GetComponent<FlashlightController>();
        
        var volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out _colorAdjustments);

    }

    public void DiedFromTraps()
    {
        NumberAttempts--;
        
        LeanTween.value(gameObject, 0f, -100f, 0.01f)
            .setOnUpdate(f => { _colorAdjustments.contrast.value = f; }).setOnComplete(() => {
                LeanTween.value(gameObject, -100f, 0, 0.8f)
                    .setOnUpdate(f => { _colorAdjustments.contrast.value = f; });
            });

        if (Random.value > 0.5f) PlayVoice(PlayerVoices[Random.Range(0, PlayerVoices.Length)]);
        
        if (NumberAttempts > 0) return;
        Died();
    }

    public void Died()
    {
        if (!isAlive) return;
        isAlive = false;
        playableDirectorFalling.Play();
        playerMovement.SetCanMove(false);
       
        GameController.Instance.HandleGameOver(false);

        LeanTween.value(gameObject, 0f, -100f, 1.5f)
            .setOnUpdate(f => { _colorAdjustments.contrast.value = f; });
    }

    public void RaiseFlashlight(Flashlight flashlight)
    {
        flashlightController.SetHasFlashlight(true);
        flashlightController.TurnOnFlashlight(true);
        Destroy(flashlight.gameObject);
    }

    public void PlayVoice(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
