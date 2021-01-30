using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{

    [SerializeField] private Image flash;

    private Coroutine coroutineFlash = null;
    public static ScreenFlash Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this) Destroy(gameObject);
    }


    public void ShowFlash()
    {
        if (coroutineFlash != null) StopCoroutine(coroutineFlash);
        coroutineFlash = StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        LeanTween.value(gameObject, 0f, 1f, 0.2f)
            .setOnUpdate(f =>
            {
                var c = flash.color;
                c.a = f;
                flash.color = c;
            }).setOnComplete(() =>
            {
                LeanTween.value(gameObject, 1f, 0f, 0.8f)
                    .setOnUpdate(f =>
                    {
                        var c = flash.color;
                        c.a = f;
                        flash.color = c;
                    });
            });
        yield return new WaitForSeconds(1f);

    }

}
