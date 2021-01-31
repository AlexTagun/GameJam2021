using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoad : MonoBehaviour
{
    [SerializeField] private List<WinPlace> winPlaces = new List<WinPlace>();

    public WinPlace CurWinPlace { get; private set; }

    private bool isRoadSpawn = false;

    public static SpawnerRoad Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance == this) Destroy(gameObject);
    }
    void Start()
    {
        
    }


    private void SpawnRoad()
    {
        if (isRoadSpawn) return;
        float maxDistance = 0;
        WinPlace maxDistantPlace = null;
        var playerPos = GameController.Instance.Player.transform.position;
        foreach (var place in winPlaces)
        {
            var distance = Vector3.Distance(playerPos, place.Place.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                maxDistantPlace = place;
            }
        }
        CurWinPlace = maxDistantPlace;
        maxDistantPlace.Activate();
        SetIsRoadSpawn(true);
    }

    public void SetIsRoadSpawn(bool b)
    {
        isRoadSpawn = b;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnRoad();
        }
    }
}
