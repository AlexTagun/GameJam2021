using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
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
