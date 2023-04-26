using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject manPrefab;

    public int mansAlive = 0;
    public int maxMansAlive = 5;

    [SerializeField]
    private int spawnInterval = 2;
    void Start()
    {
        StartCoroutine(SpawnMans(spawnInterval));        
    }

    // Update is called once per frame
    void Update()
    {
        if(mansAlive == 0){
            StartCoroutine(SpawnMans(spawnInterval));
        }
    }

    private IEnumerator SpawnMans(float interval){

        while (mansAlive < maxMansAlive){

            Instantiate(manPrefab, transform.position, transform.rotation);
            mansAlive++;

            yield return new WaitForSeconds(interval);
        }
        

    }
}
