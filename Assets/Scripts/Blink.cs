using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
    [SerializeField] private Light light;

    private void Start() {
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine() {
        bool value = false;
        while (true) {
            value = !value;
            light.color = value ? Color.blue : Color.red;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
