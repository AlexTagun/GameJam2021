using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberFlashlight : MonoBehaviour
{
    public bool NeedSpawnFlashLight { get; private set; }
    public bool NeedActivateRoad { get; private set; }
    public static RememberFlashlight Instance { get; private set; }


    private Vector3 position = Vector3.zero;
    private WinPlace winPlace = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void RememberLastPosition(Vector3 pos)
    {
        NeedSpawnFlashLight = true;
        position = pos;
    }

    public void RememberWinPlace(WinPlace winPlace)
    {
        if (winPlace != null) NeedActivateRoad = true;
        this.winPlace = winPlace;
    }



    public Vector3 GetLastPosition() 
    {
        return position;
    }

    public WinPlace GetWinPlace()
    {
        return winPlace;
    }
}
