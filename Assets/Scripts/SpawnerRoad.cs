using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerRoad : MonoBehaviour
{
    [SerializeField] private List<WinPlace> winPlaces = new List<WinPlace>();


    private bool isRoadSpawn = false;
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
        maxDistantPlace.Activate();
        isRoadSpawn = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnRoad();
        }
    }
}
