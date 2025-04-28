using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] float _spawnInterval;
    [SerializeField] int _spawnLimit;

    private BoxCollider spawnArea;
    private Bounds bounds;
    private bool spawn;
    private bool preSpawn;

    private List<GameObject> spawnedPrefabs = new();

    private void Start()
    {
        spawnArea = GetComponentInChildren<BoxCollider>();
        bounds = spawnArea.bounds;
        SpawnHandler.Instance.AddSpawner(this);
    }

    private void Update()
    {
        if(spawn && !preSpawn)
        {
            StartCoroutine(SpawnPrefabCR());
        }
        else if(!spawn && preSpawn)
        {
            StopCoroutine(SpawnPrefabCR());
        }

        preSpawn = spawn;
    }

    public void StartSpawn()
    {
        spawn = true;
    }
    public void StopSpawn()
    {
        spawn = false;
    }
    public void RefreshList()
    {
        spawnedPrefabs.RemoveAll(x => x == null);
    }
    private IEnumerator SpawnPrefabCR()
    {
        while (true)
        {
            if (spawn && spawnedPrefabs.Count < _spawnLimit)
            {
                var spawnedPrefab = Instantiate(_prefab,
                                                    new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.min.y), transform.position.y, UnityEngine.Random.Range(bounds.min.z, bounds.max.z)),
                                                    Quaternion.identity);

                spawnedPrefabs.Add(spawnedPrefab); 
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
