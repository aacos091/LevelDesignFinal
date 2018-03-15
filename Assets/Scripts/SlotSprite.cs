using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSprite : MonoBehaviour {
    public float speed = 0.10f;
    public bool slowDown = false;

    public Bounds bounds; //The Bounds of this and it's children
    public Vector3 boundsCenterOffset; //Dis of bounds.center

    RectTransform canvas;
    Vector3 startingPosition;
 

    // Use this for initialization
    void Awake () {
        //InvokeRepeating("CheckOffscreen", 0f, 2f);
	}

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
        Move();
	}

    public void Move()
    {
        
        Vector3 tempPos = pos;
        tempPos.z -= speed * Time.unscaledDeltaTime;
        pos = tempPos;
        
    }

    void CheckSpriteValue()
    {

    }

    public Vector3 pos{
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }
    //Deletes 
    void CheckOffscreen()
    {
        Vector3 camPos = Camera.main.WorldToViewportPoint(transform.position);
        //If bounds are still their default value...
        if (bounds.size == Vector3.zero)
        {
            //then set them
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            //Also find the difference of position between bounds.center & transform.position 
            boundsCenterOffset = bounds.center - transform.position;
        }
        //Every time, update the bounds to the current position.
        bounds.center = transform.position + boundsCenterOffset;
        //Check to see whether the bounds are completely offscreen
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
        {
            //If this enemy has gone off the bottom edge of the screen...
            if (off.z < Camera.main.rect.yMin)
                //Destroy it.
                Destroy(this.gameObject);
            
        }
    }
}
