using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrapGenerator : MonoBehaviour {
    public int TrapsCount = 10;
    public GameObject TrapPrefab;
    private void Start() {
        float cellSize = 1000f / TrapsCount;
        for (int i = 0; i < TrapsCount - 1; i++) {
            for (int j = 0; j < TrapsCount - 1; j++) {
                var obj = Instantiate(TrapPrefab, transform);
                Vector3 pos = new Vector3(Random.Range(cellSize * i, cellSize * i + cellSize), 0, Random.Range(cellSize * j, cellSize * j + cellSize));
                obj.transform.position = pos;
            }
        }
    }
}