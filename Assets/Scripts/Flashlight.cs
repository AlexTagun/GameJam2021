using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour {
    [SerializeField] private Light pointLight;
    
    private Transform _player;
    private Transform _tranform;
    private Vector3 flashlightPosition;
    
    private void Start() {
        _player = GameObject.FindWithTag("Player").transform;
        _tranform = transform;
        flashlightPosition = transform.position;
    }

    private void Update() {

        float distance = Vector3.Distance(_player.position, transform.position);

        if (distance > 80)
        {
            var direction = flashlightPosition - _player.position;
            _tranform.position = _player.position + direction.normalized * 100;
        }
        else _tranform.position = flashlightPosition;

        distance = Vector3.Distance(_player.position, transform.position);
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
