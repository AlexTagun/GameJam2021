using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {

            var player = other.gameObject.GetComponent<Player>();
            if (player) player.DiedFromTraps();
            Debug.Log("TRAP");
        }
    }
}
