using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Button playButton;

    [SerializeField] private GameObject[] ObjectsToShow;
    
    private ColorAdjustments _colorAdjustments;
    private void Awake() {
        playButton.onClick.AddListener(OnPlay);
    }

    private void Start() {
        Cursor.visible = true;
        var volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out _colorAdjustments);
        if (RememberFlashlight.Instance.NeedSpawnFlashLight) {
            OnPlay();
        }
    }

    private void OnPlay() {
        Cursor.visible = false;
        for (int i = 0; i < ObjectsToShow.Length; i++) {
            ObjectsToShow[i].SetActive(true);
        }
        
        LeanTween.value(gameObject, -100f, 0, 2f)
            .setOnUpdate(f => {
                _colorAdjustments.contrast.value = f;
            });
        
        gameObject.SetActive(false);
    }
}