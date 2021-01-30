using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
    [SerializeField] private Light red;
    [SerializeField] private Light blue;

    private void Start() {
        red.gameObject.SetActive(false);
        blue.gameObject.SetActive(true);
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine() {
        bool value = false;
        while (true) {
            value = !value;
            red.gameObject.SetActive(value);
            blue.gameObject.SetActive(!value);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
