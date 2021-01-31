using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
    [SerializeField] private Light light;

    public void ChangeLightIntensity(float value = 200)
    {
        light.intensity = value;
    }
    

    public IEnumerator StartBlink(float seconds) {
        bool value = false;

        for (int i = 0; i < Mathf.RoundToInt(seconds / 0.3f); i++) {
            value = !value;
            light.color = value ? Color.blue : Color.red;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
