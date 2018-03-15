using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Spin { result_1,
                   result_2,
                   result_3,
                   result_4 };

public class SlotMachine : MonoBehaviour {

    public int reelSpeed;
    public int maxAngle = 360;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        SpinReels();
        if (Input.GetKeyDown("space"))
        {
            SlowReels();
        }
    }
    void Pause() {
        if(gameObject.activeInHierarchy == true)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void SpinReels()
    {
        reelSpeed = 1000000;
        Spin result = (Spin)Random.Range(0, 3);
        GameObject slots = this.gameObject;
        GameObject[] reels = GameObject.FindGameObjectsWithTag("Reel");
        for (int i = 0; i < reels.Length; i++)
        {
            Debug.Log("Spinning " + i + "th reel.");
            var spinStart = Time.time;
            Debug.Log("Time is currently " + spinStart);
            if ((Time.time - spinStart) >= 0)
            {
                for(int j = 0; j < maxAngle; j++)
                reels[i].transform.Rotate(Vector3.up,reelSpeed);
            }
        }

    }


    void SlowReels()
    {
        
        GameObject slots = this.gameObject;
        GameObject[] reels = GameObject.FindGameObjectsWithTag("Reel");
        for (int i = 0; i < reels.Length; i++)
        {
            StartCoroutine(Hold(3));
            while(reelSpeed > 0)
            {
                reelSpeed /= 10;
            }
        }
    }
    void SummonMachine()
    {

    }

    void GetSprite()
    {

    }

    IEnumerator Hold(int x = 0)
    {
        bool waiting = true;
        while (waiting)
        {
            if(x != 0)
            {
                waiting = false;
                yield return new WaitForSecondsRealtime(x);
            }
        }
        yield return new WaitForFixedUpdate();
    }
}
