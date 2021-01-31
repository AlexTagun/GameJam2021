using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private float timeToDischarge = 240;
    [SerializeField] private float timeDelayBlick = 5;
    [SerializeField] private GameObject spotLight = null;

    [SerializeField] private AudioSource audioFlashlight = null;
    [SerializeField] private AudioClip flashlightOn = null;
    [SerializeField] private AudioClip flashlightOff = null;
    public bool HasFlashlight { get; private set; }

    public bool IsBurn { get; private set; } = false;
    private bool isTurnOn = false;
    private bool isDischarged = false;

    private float curTime = 0f;
    private float curTimeToBlick = 0f;

    private Coroutine coroutineBlick = null;

    public void SetHasFlashlight(bool value)
    {
        HasFlashlight = value;
    }

    private void Start()
    {
        HasFlashlight = !RememberFlashlight.Instance.NeedSpawnFlashLight;
        isTurnOn = HasFlashlight;
        IsBurn = HasFlashlight;
        spotLight.SetActive(isTurnOn);

    }
    private int i = 0;
    void Update()
    {
        if (!HasFlashlight) return;
        if (!isTurnOn)
        {
            if (Input.GetMouseButtonDown(1))
            {
                TurnOnFlashlight(!isTurnOn);
            }
        }
        else
        {
            if (curTime >= timeToDischarge) isDischarged = true;
            else curTime += Time.deltaTime;

            if (Input.GetMouseButtonDown(1))
            {
                TurnOnFlashlight(!isTurnOn);
                StopBlick();
            }
        }
        if (isDischarged)
        {
            if (curTimeToBlick >= timeDelayBlick)
            {
                if (isTurnOn)
                {
                    if (coroutineBlick == null) coroutineBlick = StartCoroutine(ShowBlick());
                }
            }
            else curTimeToBlick += Time.deltaTime;
        }
        i++;
        //Debug.Log("N" + i + " isTurnOn " + isTurnOn + " IsBurn " + IsBurn);
        spotLight.SetActive(isTurnOn && IsBurn);
    }

    public void TurnOnFlashlight(bool b)
    {
        if (!isDischarged) IsBurn = b;
        isTurnOn = b;
        var clip = (b) ? flashlightOn : flashlightOff;
        audioFlashlight.clip = clip;
        audioFlashlight.Play();
    }

    private IEnumerator ShowBlick()
    {
        for (int i = 0; i < 3; i++)
        {
            IsBurn = true;
            yield return new WaitForSeconds(1f);
            IsBurn = false;
            yield return new WaitForSeconds(0.5f);
        }
        curTimeToBlick = 0f;
        coroutineBlick = null;
    }

    private void StopBlick()
    {
        if (coroutineBlick == null) return;
        StopCoroutine(coroutineBlick);
        IsBurn = false;
        curTimeToBlick = 0f;
        coroutineBlick = null;
    }
}
