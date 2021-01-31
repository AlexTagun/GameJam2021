using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InvisibleWall : MonoBehaviour {
    private ColorAdjustments _colorAdjustments;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private void Start() {
        var volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet(out _colorAdjustments);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            audioSource.PlayOneShot(audioClip);
            LeanTween.value(gameObject, 0f, -100f, 0.5f)
                .setOnUpdate(f => { _colorAdjustments.contrast.value = f; }).setOnComplete(() => {
                    
                    other.transform.Rotate(0, 180, 0);
                    other.transform.position = other.transform.position + other.transform.forward * 15;
                    LeanTween.value(gameObject, -100f, 0, 0.8f)
                        .setOnUpdate(f => { _colorAdjustments.contrast.value = f; }).setOnComplete(() => audioSource.Stop());
                });
        }
    }
}