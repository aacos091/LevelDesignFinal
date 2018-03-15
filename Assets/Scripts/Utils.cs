using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoundsTest
{
    center, //Is the center of the gameobject on screen?
    onScreen,   //Are the bounds entirely onscreen?
    offScreen   //Are the bounds completely offscreen?
}

public class Utils : MonoBehaviour {

    /*======================================
     * Bounds functions
      ======================================*/

    //Creates bounds that encapsulate the two Bounds passed in.
    public static Bounds BoundsUnion(Bounds b0, Bounds b1){
        //If the size of one of the bounds is 0, ignore.
        if(b0.size == Vector3.zero && b1.size != Vector3.zero)      { return(b1); }
        else if(b0.size != Vector3.zero && b1.size == Vector3.zero) { return(b0); }
        else if(b0.size == Vector3.zero && b1.size == Vector3.zero) { return(b0); }
        //Stretch b0 to include b1.min and b1.max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }
    public static Bounds CombineBoundsOfChildren(GameObject go){
        //Creates empty bounds b
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        //If this gameobject has a renderer component...
        if (go.GetComponent<Renderer>() != null){
            //Expand B to contain the renderer's bounds
            b = BoundsUnion(b,go.GetComponent<Renderer>().bounds);
        }
        //If the gameobject has a collider component...
        if(go.GetComponent<Collider>() != null){
            //Expand b to contain the collider's bounds
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        }
        //Recursively iterate through each child of this gameObject.transform
        foreach(Transform t in go.transform){
            //Expand b to contain their bounds as well.
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));
        }
        return (b);
    }
    
    //Make a static read-only public property camBounds
    static public Bounds camBounds{
        get{
            //if _cambounds hasn't been set yet
            if(_camBounds.size == Vector3.zero){
                //SetCameraBounds using the default Camera
                SetCameraBounds();
            }
            return (_camBounds);
                }
    }

    //Static field used by camBounds
    static private Bounds _camBounds;

    //This function is used by camBounds to set _camBounds and can also be called directly.
    public static void SetCameraBounds(Camera cam = null){
        //If no camera was passed in, use the main camera.
        if (cam == null) cam = Camera.main;
        //This makes a couple of important assumptions about the camera:
        //The camera is orthographic
        //The camera is at a rotation of 0,0,0
        //Make vector3s at the topLeft and bottomRight of the Screen coords.
        Vector3 topLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, Screen.height,0);

        //Convert to world coordinates.
        Vector3 boundTLN = cam.ScreenToWorldPoint(topLeft);
        Vector3 boundBRF = cam.ScreenToWorldPoint(bottomRight);

        //Adjust their Zs to be at the near and far Camera clipping planes
        boundTLN.y += cam.nearClipPlane;
        boundBRF.y += cam.farClipPlane;

        //Find the center of the bounds
        Vector3 center = (boundTLN + boundBRF) / 2f;
        _camBounds = new Bounds(center, Vector3.zero);
        //Expand _camBounds to encapsulate the extents.
        _camBounds.Encapsulate(boundTLN);
        _camBounds.Encapsulate(boundBRF);
    }

    //Checks to see whether the Bounds bnd are within camBounds
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center) { return (BoundsInBoundsCheck(camBounds, bnd, test)); }

    //Checks to see whether Bounds lilB are within Bounds bigB
    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen){
        //The behavior of this function is different based on the Bounds test selected.

        //Get the center of lilB
        Vector3 pos = lilB.center;

        //Initialize the offset at 0,0,0
        Vector3 off = Vector3.zero;

        switch (test){
            //The center test determines what off(set) would have to be applied to lilB to move it's center inside bigB
            case BoundsTest.center:
                if (bigB.Contains(pos)) { return (Vector3.zero); }
                if (pos.x > bigB.max.x) { off.x = pos.x - bigB.max.x; }
                else if (pos.x < bigB.min.x) { off.x = pos.x - bigB.min.x; }
                if (pos.y > bigB.max.y) { off.y = pos.y - bigB.max.y; }
                else if (pos.y < bigB.min.y) { off.y = pos.y - bigB.min.y; }
                if (pos.z > bigB.max.z) { off.z = pos.z - bigB.max.z; }
                else if (pos.z < bigB.min.z) { off.z = pos.z - bigB.min.z; }
                return (off);

            //The onScreen test determines what off would have to be applied to keep all of lilB inside bigB
            case BoundsTest.onScreen:
                if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max)) { return (Vector3.zero); }
                if (lilB.max.x > bigB.max.x) { off.x = lilB.max.x - bigB.max.x; }
                else if (lilB.min.x > bigB.min.x) { off.x = lilB.min.x - bigB.min.x; }
                if (lilB.max.y > bigB.max.y) { off.y = lilB.max.y - bigB.max.y; }
                else if (lilB.min.y > bigB.min.y) { off.y = lilB.min.y - bigB.min.y; }
                if (lilB.max.z > bigB.max.z) { off.z = lilB.max.z - bigB.max.z; }
                else if (lilB.min.z > bigB.min.z) { off.z = lilB.min.z - bigB.min.z; }
                return (off);

            //The offScreen test determines what off would need to be applied to move any tiny part of lilB inside bigB.
            case BoundsTest.offScreen:
                bool cMin = bigB.Contains(lilB.min);
                bool cMax = bigB.Contains(lilB.max);
                if (cMin || cMax) { return Vector3.zero; }
                if (lilB.min.x > bigB.max.x) { off.x = lilB.min.x - bigB.max.x; }
                else if (lilB.max.x < bigB.min.x) { off.x = lilB.max.x - bigB.min.x; }
                if (lilB.min.y > bigB.max.y) { off.y = lilB.min.y - bigB.max.y; }
                else if (lilB.max.y < bigB.min.y) { off.y = lilB.max.y - bigB.min.y; }
                if (lilB.min.z > bigB.max.z) { off.z = lilB.min.z - bigB.max.z; }
                else if (lilB.max.z < bigB.min.z) { off.z = lilB.max.z - bigB.min.z; }
                return (off);
        }
        return (Vector3.zero);
    }
    /*======================================
     * Transform functions
      ======================================*/

    //This function will iteratively climb up the tranform.parent tree until it either finds a parent with a tag != "untagged" or no parent.
    public static GameObject FindTaggedParent(GameObject go){
        //If the gameobject has a tag...
        if (go.tag != "Untagged")
            return (go);//return the gameobject.
        //If there is no parent of the transform...
        if (go.transform.parent == null)
            return (null); //Reached the top of the heirarchy with no interesting tag.
        //Otherwise recursively clumb the tree.
        return (FindTaggedParent(go.transform.parent.gameObject));
    }
    //Handles transforms and passes them to gameobject function

    public static GameObject FindTaggedParent(Transform t)
    {
        return (FindTaggedParent(t.gameObject));
    }
    /*======================================
     * Materials functions
      ======================================*/
      
    //Returns a list of all Materials on this GameObject or its children
    static public Material[] GetAllMaterials(GameObject go)
    {
        List<Material> mats = new List<Material>();
        if(go.GetComponent<Renderer>() != null)
        {
            mats.Add(go.GetComponent<Renderer>().material);
        }
        foreach(Transform t in go.transform)
        {
            mats.AddRange(GetAllMaterials(t.gameObject));
        }
        return (mats.ToArray());
    }
}
