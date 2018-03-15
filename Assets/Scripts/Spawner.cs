using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject enemy;
    public GameObject boss;
    public GameObject player;
	public float spawnTime;
    bool isAlive = false;
    bool spawnBoss = true;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player Holder");
        InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
        InvokeRepeating("SpawnBoss", spawnTime, spawnTime);
    }

	// Update is called once per frame
	void Update () {
        
	}

	void SpawnEnemy () {
		Instantiate(enemy, transform.position, transform.rotation);
	}

    void SpawnBoss()
    {        
        if (player.GetComponent<PlayerScript>().points >= 5000 && isAlive == false)
        {
            Instantiate(boss, transform.position, transform.rotation);
            isAlive = true;
        }

    }
}
