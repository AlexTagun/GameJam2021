using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CricketSound : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    private Transform _player;

    private void Start() {
        _player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(5, 15));

            Vector3 randomVector = Random.onUnitSphere;
            randomVector.y = 0;

            randomVector = randomVector.normalized * 5;
            Debug.Log(randomVector);
            transform.position = _player.position + randomVector;
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
        }
    }
}