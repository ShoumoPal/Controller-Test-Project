using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandler : MonoBehaviour
{
    public static SpawnHandler Instance;

    [SerializeField] private List<SpawnerScript> spawners = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.P))
        //    StartAllSpawners();
        //else if(Input.GetKeyDown(KeyCode.O))
        //    StopAllSpawners();
    }

    public void AddSpawner(SpawnerScript spawner)
    {
        spawners.Add(spawner);
    }
    public void StartAllSpawners()
    {
        spawners?.ForEach(x => x.StartSpawn());
        StartCoroutine(RefreshListsCR());
    }

    private IEnumerator RefreshListsCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            RefreshSpawnerLists(); 
        }
    }

    public void StopAllSpawners()
    {
        spawners?.ForEach(x => x.StopSpawn());
        StopCoroutine(RefreshListsCR());
    }
    public void RefreshSpawnerLists()
    {
        spawners.ForEach(x => x.RefreshList());
    }

}
public enum Spawnables
{
    Enemy
}
