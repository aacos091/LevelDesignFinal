using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	// Variables
	public float health;
	public static int pointsToGive = 200;
    public static int damage;
    public float Movespeed = 150;
    float minDist = 0.0f;

	private GameObject player;
    private GameObject target;

	public float waitTime;
	private float currentTime;
	private bool shot;
	//public float moveSpeed;
	public float smoothTime = 100.0f;
	private Vector3 smoothVelocity = Vector3.zero;

    public GameObject coin;
	public GameObject bullet;
	public GameObject bulletSpawnPoint;
	private Transform bulletSpawned;

	// Methods
	public void Start() {
		player = GameObject.Find("Player Holder");
        target = GameObject.Find("PlayerTarget");
    }

	public void Update() {
		if(health <= 0) {
			Die();
		}

		transform.LookAt(player.transform);
        if(Vector3.Distance(transform.position,player.transform.position)>= minDist)
        { transform.position += transform.forward * smoothTime * Time.deltaTime; }

		transform.position = Vector3.MoveTowards(transform.position, player.transform.position, smoothTime * Time.deltaTime);
	}

	public void Die() {
        if(this.gameObject.name == "Boss(Clone)")
        {
            pointsToGive *= 4;
        }
		player.GetComponent<PlayerScript>().points += pointsToGive;
        Destroy(gameObject);
    }

	public void Shoot() {
		shot = true;

		bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
		bulletSpawned.rotation = this.transform.rotation;
	}

}
