using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    [SerializeField] private Light pointLight;
    
    private Transform _player;
    
    private void Start() {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update() {
        float distance = Vector3.Distance(_player.position, transform.position);
        distance = Mathf.Clamp(distance, 0, 80);
        pointLight.intensity = Mathf.Lerp(5, 200, distance / 80);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            var player = other.gameObject.GetComponent<Player>();
            if (player) player.RaiseFlashlight(this);
            Debug.Log("Flashlight True");
        }
    }
}
