using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    public enum roll
    {
        buff1,
        buff2,
        debuff1,
        debuff2,
    }

    static public Main S; //Singleton
    RectTransform canvas;
    Vector3 startingPosition;
    bool isPaused = false;
    public GameObject camera;
    public GameObject reelController;

    public Enemy enemy;
    public Spawner[] spawner;
    public GameObject player;
    public GameObject[] enemies;
    public GameObject[] bullets;
    public GameObject[] spawns;

    private GameObject tempReel1;
    private GameObject tempReel2;
    private GameObject tempReel3;

    public GameObject[] reelSprites;
    // Use this for initialization
    private void Awake()
    {
        
    }
    void Start () {
        player = GameObject.Find("Player Holder");
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        startingPosition = transform.position;

        Utils.SetCameraBounds();
      	//Invoke("SpawnReelSprite", 0f);
    }

    IEnumerator SpawnReelSprite()
    {
        
        //Instantiate a random sprite from the array.
        int ndx = Random.Range(0, reelSprites.Length);
        GameObject go = Instantiate(reelSprites[ndx]) as GameObject;
        go.transform.parent = canvas.transform;
        Vector3 pos = Vector3.zero;
        pos.z = canvas.rect.yMax;
        
        pos.y = 3; //Raise sprites over other gameobjects.
        go.transform.position = pos;
        //Call SpawnReelSprite() again in aa couple of seconds
        yield return WaitForRealSeconds(2f);
        Invoke("SpawnReelSprite",0f);
        //yield return WaitForRealSeconds(2f);
        //StartCoroutine("SpawnReelSprite");
    }

    // Update is called once per frame
    void Update () {

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //bullets = GameObject.FindGameObjectsWithTag("Bullet");
        //spawns = GameObject.FindGameObjectsWithTag("Spawn");
        Reel();
        if (Input.GetButtonDown(KeyCode.Space.ToString()))
        {
            if (isPaused == true) {
            	//Unpause();
            } else if (isPaused == false) {
            	Pause();
            }
        } 
	}

    void Pause()
    {
        
       // Time.timeScale = 0.0f;
        /*player.GetComponent<PlayerScript>().movementSpeed = 0;
        foreach(GameObject b in bullets) {
        	b.GetComponent<Bullet>().speed = 0;
        }
		foreach(GameObject e in enemies) {
        	e.GetComponent<Enemy>().enabled = false;
        }
		foreach(GameObject s in spawns) {
        	s.GetComponent<Spawner>().enabled = false;
        }*/
        //for (int x = 0; x < 3; x++) {
		//StartCoroutine("SpawnReelSprite");
        //}
        //isPaused = true;
       // this.GetComponent<Main>().enabled = true;
        //Reel();
    }

    void KillReels()
    {
        Destroy(tempReel1);
        Destroy(tempReel2);
        Destroy(tempReel3);        
    }

    public IEnumerator _WaitForRealSeconds(float aTime)
    {
        while (aTime > 0f)
        {
            aTime -= Mathf.Clamp(Time.unscaledDeltaTime, 0, 0.2f);
        }
        yield return null;
    }
    public Coroutine WaitForRealSeconds(float aTime)
    {
        return StartCoroutine(_WaitForRealSeconds(aTime));
    }

    public void Reel()
    {
        if(player.GetComponent<PlayerScript>().points % 1000 == 0 && player.GetComponent<PlayerScript>().points  != 0)
        {
            player.GetComponent<PlayerScript>().points += Enemy.pointsToGive;
            //Pause();
            GameObject go1 = GameObject.Find("Reel1");
            GameObject go2 = GameObject.Find("Reel2");
            GameObject go3 = GameObject.Find("Reel3");
            int i = Random.Range(0, reelSprites.Length);
            tempReel1 = Instantiate(reelSprites[i], go1.transform);
            tempReel2 = Instantiate(reelSprites[i], go2.transform);
            tempReel3 = Instantiate(reelSprites[i], go3.transform);
            switch (i)
            {
                case 0:
                    SpeedBonus();
                    break;
                case 1:
                    Heal();
                    break;
                case 2:
                    Slow();
                    break;
                case 3:
                    SpeedBonus();
                    break;
                case 4:
                    DamageUp();
                    break;
                default:
                    break;
            }
            
            Invoke("KillReels",1.5f);
        }
        
    }

    public void SpeedBonus()
    {
        player.GetComponent<PlayerScript>().ChangeSpeed(0.5f);
        return;
    }

    public void Heal()
    {
        player.GetComponent<PlayerScript>().ChangeHealth(25f);
        return;
    }

    public void Slow()
    {
        player.GetComponent<PlayerScript>().ChangeSpeed(-1f);
        return;
    }

    public void DamageUp()
    {
        player.GetComponent<Bullet>().ChangeDamage(5f);
        return;
    }
}
