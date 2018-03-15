using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    public enum mode { knockback, normal }
    // Variables
    public float movementSpeed;
    public float knockbackSpeeed = 300f;
    public float knockbackDuration = 0.75f;
    public bool invincible = false;
    public float invincibleDuration = 0.25f;
    public mode currentMode = mode.normal;

    public GameObject PlayerObj;

	public GameObject bulletSpawnPoint;
	public float waitTime;
	public GameObject bullet;

	private Transform bulletSpawned;
    private Vector3 knockbackVel;
    private Rigidbody rigid;
    private float invincibleDone = 0;

    private float knockbackDone;

    public int points = 0;
	public int coins = 0;

	public Text coinText;
	public Text pointText;
	public Text healthText;

	public float health;

    // Methods
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update() {
		// Player Death
		if(health <= 0) {
			Die();
		}
        //Invulnerability
        if (invincible && Time.time > invincibleDone)
            invincible = false;
        if (currentMode == mode.knockback)
        {
            rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone) return;
        }
        rigid.velocity = Vector3.zero;
        // Player facing mouse
        Plane playerPlane = new Plane(Vector3.up, transform.position);
		Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
		float hitDist = 0.0f;

		if (playerPlane.Raycast(ray, out hitDist)) {
			Vector3 targetPoint = ray.GetPoint(hitDist);
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			targetRotation.x = 0;
			targetRotation.z = 0;
			PlayerObj.transform.rotation = Quaternion.Slerp(PlayerObj.transform.rotation, targetRotation, 7f * Time.deltaTime);
		}

		// Player Movement
		if(Input.GetKey(KeyCode.W)) {
			transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.A)) {
			transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.S)) {
			transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.D)) {
			transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
		}

		// Shooting
		if(Input.GetMouseButtonDown(0)) {
			Shoot();
		}

		coinText.text = coins.ToString();
		pointText.text = points.ToString();
		healthText.text = health.ToString();
	}

	void Shoot() {
		bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        
        bulletSpawned.rotation = bulletSpawnPoint.transform.rotation;
        
	}

    public void ChangeHealth(float f)
    {
        health += f;
    }

    public void ChangeSpeed(float f)
    {
        movementSpeed += f;
        if (movementSpeed < 1) movementSpeed = 1;
    }

    public void Die() {
		print("You've Died!");
        GameObject.Destroy(this);
        SceneManager.LoadScene("Test");
    }

	void OnTriggerEnter(Collider coll) {
        if (invincible) return;
        DamageEffect dEf = coll.gameObject.GetComponent<DamageEffect>();
        if (dEf == null) return; //nothing happens if no damage effect is tied to it.
        health -= dEf.damage;
        invincible = true;
        invincibleDone = Time.time + invincibleDuration;
        if (dEf.knockback)
        {
            //Direction
            Vector3 delta = transform.position - coll.transform.position;
            //apply speed to rigidbody.
            knockbackVel = delta * knockbackSpeeed;
            rigid.velocity = knockbackVel;
            currentMode = mode.knockback;
            knockbackDone = Time.time + knockbackDuration;
        }
        if (coll.gameObject.tag == "Coin") {
			Debug.Log("Got a Coin!");
			Destroy(coll.gameObject);
			coins += 1;
		}
	}
}