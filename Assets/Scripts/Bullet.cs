using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	// Variables
	public float speed;
	public float maxTime;

	private GameObject triggeringEnemy;
	private GameObject triggeringPlayer;
	public float damage;

	// Methods
	void Start() {
        //player = GameObject.FindWithTag("Player");
        transform.Rotate(-90,0,0);
    }
    public void ChangeDamage(float f)
    {
        damage += f;
    }
	void Update () {
		transform.Translate(Vector3.down * Time.deltaTime * speed);
        
        Destroy(this.gameObject, maxTime);
	}

	public void OnTriggerEnter(Collider other) {
		Debug.Log("Hit Detected!");
		if(other.tag == "Enemy") {
			triggeringEnemy = other.gameObject;
			triggeringEnemy.GetComponent<Enemy>().health -= damage;
			Destroy(this.gameObject);
		}
		if(other.name == "Player Holder") {
			triggeringPlayer = other.gameObject;
			triggeringPlayer.GetComponent<PlayerScript>().health -= damage;
			Destroy(this.gameObject);
		}
	}
}
