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

    [SerializeField] private List<AudioClip> batteryIsLow = null;
    [SerializeField] private AudioClip flashlightDead = null;
    public bool HasFlashlight { get; private set; }

    public bool IsBurn { get; private set; } = false;
    private bool isTurnOn = false;
    private bool isDischarged = false;

    private float curTime = 0f;
    private float curTimeToBlick = 3f;
    private int numberBlick = 0;

    private Coroutine coroutineBlick = null;

    private Player player;

    public void SetHasFlashlight(bool value)
    {
        HasFlashlight = value;
    }

    private void Start()
    {
        player = GetComponent<Player>();
        HasFlashlight = !RememberFlashlight.Instance.NeedSpawnFlashLight;
        isTurnOn = HasFlashlight;
        SetIsBurn(HasFlashlight);
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
            if(!isDischarged)
            {
                if (curTime >= timeToDischarge)
                {
                    isDischarged = true;
                    SetIsBurn(false);
                }
                else if (curTime >= 60 && numberBlick == 0) coroutineBlick = StartCoroutine(ShowBlick());
                else if (curTime >= 120 && numberBlick == 1) coroutineBlick = StartCoroutine(ShowBlick());
                else if (curTime >= 180 && numberBlick == 2) coroutineBlick = StartCoroutine(ShowBlick());
                else curTime += Time.deltaTime;
            }

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
        if (!isDischarged) SetIsBurn(b);
        isTurnOn = b;
        var clip = (b) ? flashlightOn : flashlightOff;
        audioFlashlight.clip = clip;
        audioFlashlight.Play();
    }

    private IEnumerator ShowBlick()
    {
        if (isDischarged)
        {
            SetIsBurn(true);
            yield return new WaitForSeconds(1f);
            SetIsBurn(false);
            curTimeToBlick = 0f;
            coroutineBlick = null;

            player.PlayVoice(flashlightDead);
        }
        else
        {
            numberBlick++;
            for (int i = 0; i < 3; i++)
            {
                SetIsBurn(false);
                yield return new WaitForSeconds(0.2f);
                SetIsBurn(true);
                yield return new WaitForSeconds(0.2f);
            }
            player.PlayVoice(batteryIsLow[Random.Range(0, batteryIsLow.Count)]);
            coroutineBlick = null;
        }
    }

    private void StopBlick()
    {
        if (coroutineBlick == null) return;
        StopCoroutine(coroutineBlick);
        SetIsBurn(false);
        curTimeToBlick = 0f;
        coroutineBlick = null;
    }

    private void SetIsBurn(bool isOn)
    {
        IsBurn = isOn;
        player.SetVisibilityLevel(isOn);
    }
}
