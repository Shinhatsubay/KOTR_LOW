using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Transform PlayerTransform;

    private int AmountOnScreen = 10;
    private float SafeZone = 200f;

    private float TileLength = 180f; //testplane 2000f
    private float SpawnZ = 0; // testplane 6000f
    private float LastLine = 1;
    public float SpawnBar = 50f;
    public float spawnEnemyDist = 100f;
    public float EnemySpawnTime = 30;
    public float enemiesOnStart = 3;

    public GameObject[] EnemyPrefabs;
    private List<GameObject> ActiveEnemy;
    //private float SpawnJumpy = 100f;
    //private float JumpyGapBar = 120f;

    private void Start()
    {
        ActiveEnemy = new List<GameObject>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("SpawnTile", 30, 4);
        InvokeRepeating("DelTile", 30, 4);

        InvokeRepeating("SpawnCrates", 3, 3);
        InvokeRepeating("DelCrates", 2, 1);

        InvokeRepeating("SpawnBarrels", 5, 5);
        InvokeRepeating("DelBarrels", 5, 3);

        InvokeRepeating("SpawnEnemy", 10, EnemySpawnTime);
        InvokeRepeating("DelEnemy", 10, 5);

        for (int i = 0; i < AmountOnScreen; i++)
        {
            GameObject obj = ObjectPoolerTerrain.current.GetPooledObject();
            if (obj == null) return;
            obj.transform.position = new Vector3(0, 0, 1 * SpawnZ);
            SpawnZ += TileLength;
            obj.SetActive(true);

            if (i <= 0)
                SpawnEnemy();
        }
    }

    private void DelCrates()
    {
        GameObject ActiveObj = ObjectPoolerCrates.current.GivePooledObject();
        if (ActiveObj == null) return;
        if (PlayerTransform.transform.position.z - 60f > ActiveObj.transform.position.z || ActiveObj.transform.position.y < -1)
            ActiveObj.SetActive(false);
        
    }

    private void SpawnCrates()
    {
        GameObject obj = ObjectPoolerCrates.current.GetPooledObject();
        if (obj == null) return;
        obj.transform.position = new Vector3((50) + ((2.5f) * RandomLine()), 0.1f, PlayerTransform.transform.position.z + SpawnBar + RandomDist());
        obj.SetActive(true);
    }

    private void DelBarrels()
    {
        GameObject ActiveObj = ObjectPooler.current.GivePooledObject();
        if (ActiveObj == null) return;
        if (PlayerTransform.transform.position.z - 60 > ActiveObj.transform.position.z || ActiveObj.transform.position.y < -1)
            ActiveObj.SetActive(false);
    }

    private void SpawnBarrels()
    {
        GameObject obj = ObjectPooler.current.GetPooledObject();
        if (obj == null) return;
        obj.transform.position = new Vector3((50f) + ((2.5f) * RandomLine()), 0.8f, PlayerTransform.transform.position.z + SpawnBar + RandomDist());
        obj.SetActive(true);
    }

    private void SpawnTile()
    {
        if (PlayerTransform.position.z - SafeZone > (SpawnZ - AmountOnScreen * TileLength))
        {
            GameObject obj = ObjectPoolerTerrain.current.GetPooledObject();
            if (obj == null) return;
            obj.transform.position = new Vector3(0, 0, 1 * SpawnZ);
            SpawnZ += TileLength;
            obj.SetActive(true);
        }
    }

    private float RandomLine()
    {
        float Line = LastLine;
        while (Line == LastLine) Line = Random.Range(1, 8);
        LastLine = Line;
        return Line;
    }

    private float RandomDist() { return Random.Range(40, 80); }

    private void DelTile()
    {
        GameObject ActiveObj = ObjectPoolerTerrain.current.GivePooledObject();
        if (ActiveObj == null) return;
        ActiveObj.SetActive(false);
    }

    private void SpawnEnemy()
    {
        if (ActiveEnemy.Count < enemiesOnStart)
        {
            GameObject enemy;
            enemy = Instantiate(EnemyPrefabs[0]) as GameObject;
            enemy.transform.SetParent(transform);
            enemy.transform.position = new Vector3((50f) + ((2.5f) * RandomLine()), 0.1f, PlayerTransform.transform.position.z + spawnEnemyDist);
            //SpawnJumpy += JumpyGapBar;
            ActiveEnemy.Add(enemy);
        }
    }

    private void DelEnemy()
    {
        for (int i = 0; i < ActiveEnemy.Count; i++) 
        {
            if (ActiveEnemy[i] != null)
                if (ActiveEnemy[i].transform.position.y < -1 || !ActiveEnemy[i].activeInHierarchy)
                {
                    Destroy(ActiveEnemy[i]);
                    ActiveEnemy.Remove(ActiveEnemy[i]);
                }         
        }
    }
}

//    private int RandomPrefabIndex()
//{
//    if (TilePrefabs.Length <= 1)
//        return 0;
//    int RandomIndex = LastPrefabIndex;
//    while (RandomIndex == LastPrefabIndex)
//    {
//        RandomIndex = Random.Range(0, TilePrefabs.Length);
//    }

//    LastPrefabIndex = RandomIndex;
//    return RandomIndex;

//}