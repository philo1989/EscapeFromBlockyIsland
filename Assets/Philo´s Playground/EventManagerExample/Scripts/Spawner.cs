using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{   /**/
    public int spawnCount;
    [Range(1, 100)]
    public int spawnRadius = 1;
    public float minionOffset = 1;
    public GameObject minion;
    /**/
    /*BlockyIslands Stuff*/
    public TerrainHandler terrainHandler;
    public List<GameObject> itemList;
    //private UnityAction spawnListener;

    //private void Awake()
    //{
    //    spawnListener = new UnityAction(Spawn);
    //}

    private void OnEnable()
    {
        //EventManager.StartListening("Spawn", spawnListener);
        EventManager.StartListening("Spawn", Spawn);
    }
    
    private void OnDisable()
    {
        //EventManager.StartListening("Spawn", spawnListener);
        EventManager.StopListening("Spawn", Spawn);
    }
    private void Spawn()
    { 
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();

            Quaternion spawnRotation = new Quaternion();
            spawnRotation.eulerAngles = new Vector3 (0.0f, Random.Range(0.0f,360.0f));
            if(spawnPosition != Vector3.zero)
            {
                Instantiate(minion, spawnPosition, spawnRotation);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3();
        float startTime = Time.realtimeSinceStartup;
        bool test = false;
        while (test == false)
        {
            Vector2 spawnPositionRaw = Random.insideUnitCircle * spawnRadius;
            spawnPosition = new Vector3(spawnPositionRaw.x, minionOffset, spawnPositionRaw.y);
            test = !Physics.CheckSphere(spawnPosition, 0.75f); //wenn checkSphere == false/!nicht dann test = false;sonst while ...
            if(Time.realtimeSinceStartup - startTime > 0.75f) //ex.: realTime = 2 
            {
                Debug.Log("Time out placing Minion!");
                return Vector3.zero;
            }
        }
        return spawnPosition;
    }
    private Vector3 CreateSpawnPosition(int maxY,int maxX)
    {
        

        Vector3 spawnPoint = new Vector3(Random.Range(0, maxY), 5,Random.Range(0, maxX));
        return spawnPoint;
    }
    /*Old'nUgly Spaghettie Style*/
    public void SpawnItems(bool respawn = false)
    {
        foreach (var item in itemList)
        {
            Instantiate(item, /*CreateSpawnPosition(terrainHandler.Breite,terrainHandler.Höhe)*/terrainHandler.GetRandomPosition(), item.transform.rotation);
            Debug.Log("Spawned: " + item);
        }
    }
    public void ReSpawnItems()
    {
        foreach(var item in itemList)
        {
            item.gameObject.SetActive(false);
            item.gameObject.transform.position = terrainHandler.GetRandomPosition();
            item.gameObject.SetActive(true);
        }
    }
    
}
