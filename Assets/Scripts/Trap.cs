using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private AudioSource audioTrap = null;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            var player = other.gameObject.GetComponent<Player>();
            if (player) player.DiedFromTraps();
            audioTrap.Play();
            Debug.Log("TRAP");
        }
    }
}
