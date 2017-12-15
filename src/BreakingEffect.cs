using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingEffect : MonoBehaviour
{
    //Indicate if explosion happens
    bool start = false;

    //Store timeStamp when explosion happens
    static float startTime;

    //Input explosion level E. Receive input from GUI
    public float E;

    //Store initial speed after explosion happens
    float initSpeed;

    //Input coefficient of friction. Receive input from GUI
    public float InputMu;

    //Static Mu that used in calculation
    public static float Mu;

    //Store position of explosionPoint that is bottom center of target object 
    static Vector3 explosionPoint;

    //Consider gravity acceleration
    float g = (float)-9.8;

    //Store target object
    public static GameObject targetObj;

    //A list to store child of target object
    Transform[] subObj;

    //A list to store custom piece objects
    public PieceObj[] pieceObj;

    //Custom class for pieces (M4)
    public class PieceObj
    {
        //3D model of PieceObj in scene.
        public GameObject obj;

        //Indicates if the object is on the ground.
        public bool onGround;

        //The angle between initial speed and horizontal.
        public float thetaOne;

        //The angle between x axiom and projection on horizontal of initial speed
        public float thetaTwo;

        //Initial speed the object has when explosion happens.
        public float initSpeed;

        //Gravity acceleration
        public float gravity;

        //speed*Frame* values are used when calculate displacement on the ground for each piece.
        //SpeedLastFrameX and speedLastFrameZ is the speed on X or Z direction at the beginning of last frame.
        //SpeedThisFrameX and speedThisFrameZ is the speed on X or Z direction at the beginning of this frame.
        //Unity considers negative speed as speed on opposite direction. 
        //So these 2 value are used to detect if speed of a piece reaches 0 by checking if speedThisFrameX* speedLastFrameX <= 0. 
        //Then set stop to true.
        public float speedThisFrameX;
        public float speedLastFrameX;
        public float speedThisFrameZ;
        public float speedLastFrameZ;

        //Indicates if the speed of object already equals to 0.
        public bool stop;
 
        //Constructor of piece object
        public PieceObj(GameObject o, float v, float g)
        {
            obj = o;
            onGround = false;
            initSpeed = v;
            gravity = g;
            stop = false;

            //Calculate thetaOne and thetaTwo
            this.thetaOneCalc();
            this.thetaTwoCalc();

            //Calculate inital value of speedLastFrameX and speedLastFrameZ
            speedLastFrameX = this.initSpeed * Mathf.Cos(this.thetaOne) * Mathf.Sin(this.thetaTwo);
            speedLastFrameZ = this.initSpeed * Mathf.Cos(this.thetaOne) * Mathf.Cos(this.thetaTwo);
        }

        //Calculate thetaOne by IM1
        public void thetaOneCalc()
        {
            this.thetaOne = Mathf.Atan((this.obj.transform.position.y - explosionPoint.y) / Mathf.Sqrt(Mathf.Pow(this.obj.transform.position.x - explosionPoint.x, 2) + Mathf.Pow(this.obj.transform.position.z - explosionPoint.z, 2)));
        }

        //Calculate thetaTwo by IM1
        public void thetaTwoCalc()
        {
            this.thetaTwo = Mathf.Atan2(this.obj.transform.position.x - explosionPoint.x, this.obj.transform.position.z - explosionPoint.z);
        }

        //All displacement calculation functions are M6
        //Collect displacement in the air
        public void MoveInAir()
        {
            this.obj.transform.Translate(DisAirCalX(), DisAirCalY(), DisAirCalZ());
        }

        //Collect displacement on the ground
        public void MoveOnGround()
        {
            this.obj.transform.Translate(DisGroCalX(), 0, DisGroCalZ());
        }

        //Calculate displacement in the air on X by using IM2
        public float DisAirCalX()
        {
            return this.initSpeed * Mathf.Cos(this.thetaOne) * Mathf.Sin(this.thetaTwo) * Time.fixedDeltaTime;
        }

        //Calculate displacement in the air on Y by using IM2
        public float DisAirCalY()
        {
            return (this.initSpeed * Mathf.Sin(this.thetaOne) + this.gravity * (Time.realtimeSinceStartup - BreakingEffect.startTime)) * Time.fixedDeltaTime + 1 / 2 * this.gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
        }

        //Calculate displacement in the air on Z by using IM2
        public float DisAirCalZ()
        {
            return this.initSpeed * Mathf.Cos(this.thetaOne) * Mathf.Cos(this.thetaTwo) * Time.fixedDeltaTime;
        }

        //Calculate displacement on the ground on X by using IM3
        public float DisGroCalX()
        {
            //Check direction on X
            if (speedLastFrameX >= 0)
            {
                gravity = -Mathf.Abs(gravity);
            }
            else
            {
                gravity = Mathf.Abs(gravity);
            }

            //Calculate speed at the beginning of this frame
            speedThisFrameX = speedLastFrameX + BreakingEffect.Mu * gravity * Time.fixedDeltaTime;

            //Use IM3
            float Sx = speedThisFrameX * Time.fixedDeltaTime + 1 / 2 * BreakingEffect.Mu * gravity * Mathf.Pow(Time.fixedDeltaTime, 2);

            //Check if speed reaches 0
            if (stop == true || speedThisFrameX * speedLastFrameX <= 0)
            {
                stop = true;
                return 0;
            }
            else
            {
                this.speedLastFrameX = this.speedThisFrameX;
                return Sx;
            }
        }

        //Use the same method as DisGroCalX() to calculate displacement on the ground on Z
        public float DisGroCalZ()
        {
            
            if (speedLastFrameZ >= 0)
            {
                gravity = -Mathf.Abs(gravity);
            }
            else
            {
                gravity = Mathf.Abs(gravity);
            }

            speedThisFrameZ = speedLastFrameZ + BreakingEffect.Mu * gravity * Time.fixedDeltaTime;
            float Sz = speedThisFrameZ * Time.fixedDeltaTime + 1 / 2 * BreakingEffect.Mu * gravity * Mathf.Pow(Time.fixedDeltaTime, 2);

            if (stop == true || speedThisFrameZ * speedLastFrameZ <= 0)
            {
                stop = true;
                return 0;
            }
            else
            {
                this.speedLastFrameZ = this.speedThisFrameZ;
                return Sz;
            }
        }
    }

    //Input verification (M3)
    public void InputVerify(float e, float mu, GameObject go)
    {
        if (e > 10 || e < 1)
        {
            throw new System.Exception("Invalid Explosion Level. Must be a number from 1 to 10");
        }

        if (mu >= 1 || mu <= 0)
        {
            throw new System.Exception("Invalid coefficient of ground friction. Must be a number from 0 to 1");
        }

        if (go.transform.position.x > 1000 || go.transform.position.x < -1000)
        {
            throw new System.Exception("Invalid x coordinate of target object");
        }

        if (go.transform.position.y > 1000 || go.transform.position.y < 0)
        {
            throw new System.Exception("Invalid y coordinate of target object");
        }

        if (go.transform.position.z > 1000 || go.transform.position.x < -1000)
        {
            throw new System.Exception("Invalid z coordinate of target object");
        }
    }

    // Initialization of scene. Acquire and create instance for pieces object (M5)
    public void Start()
    {
        //Assign targetObj by the game object this component is attached to. 
        //A component is always attached to a game object.
        targetObj = gameObject;

        //Call M3 to do input verification
        InputVerify(E, InputMu, targetObj);
        
        //Assign input value of Mu to static variable
        Mu = InputMu;

        //Value of initial velocity given by explosion is ten times input E unit length in unity per second.
        initSpeed = 10 * E;

        //subObj[] is a list of sub objects under target Object. 
        //Since all pieces make up the whole target object
        //All pieces object are considered as sub objects of the target object in Unity3D. 
        //We need to get all sub objects firstly and then use these sub objects to construct piece objects defined by myself.
        subObj = targetObj.GetComponentsInChildren<Transform>();

        //Initialize list of piece objects
        pieceObj = new PieceObj[targetObj.transform.childCount];

        //Acquire explosion point
        explosionPoint = GetExplosionPoint();

        //Do traversal to initialize all pieces.
        for (int i = 1; i < subObj.Length; i++)
        {
            pieceObj[i - 1] = new PieceObj(subObj[i].gameObject, initSpeed, g);

            if (pieceObj[i - 1].obj.GetComponent<Rigidbody>() == null)
            {
                pieceObj[i - 1].obj.AddComponent<Rigidbody>();
            } 
            pieceObj[i - 1].obj.GetComponent<Rigidbody>().useGravity = false;
            pieceObj[i - 1].obj.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    //Update is called once per frame. But time gap is not fixed.
    void Update()
    {
        //Explosion happens when user press E button.
        if (Input.GetKey(KeyCode.E))
        {
            start = true;
            startTime = Time.time;
        }
    }

    //FixedUpdate is called per frame with fixed time gap. 
    //Official doc suggest that codes related to physical should be put into FixedUpdate.
    void FixedUpdate()
    {
        if (start == false) return;

        //Do traversal to "move" all pieces.
        for (int i = 0; i < pieceObj.Length; i++)
        {
            if (pieceObj[i].onGround != true)
            {
                pieceObj[i].MoveInAir();
            }
            else
            {
                pieceObj[i].MoveOnGround();
            }
        }
    }

    private void LateUpdate()
    {

    }

    //Calculate physical center of an object
    public static Vector3 GetCenter(Transform tt)
    {

        Transform parent = tt;

        Vector3 postion = parent.position;

        Quaternion rotation = parent.rotation;

        Vector3 scale = parent.localScale;

        parent.position = Vector3.zero;

        parent.rotation = Quaternion.Euler(Vector3.zero);

        parent.localScale = Vector3.one;

        Vector3 center = Vector3.zero;

        Renderer[] renders = parent.GetComponentsInChildren<Renderer>();

        foreach (Renderer child in renders)
        {
            center += child.bounds.center;
        }

        center /= parent.GetComponentsInChildren<Renderer>().Length;

        Bounds bounds = new Bounds(center, Vector3.zero);

        foreach (Renderer child in renders)
        {

            bounds.Encapsulate(child.bounds);

        }

        parent.position = postion;

        parent.rotation = rotation;

        parent.localScale = scale;

        foreach (Transform t in parent)
        {

            t.position = t.position - bounds.center;

        }
        parent.transform.position = bounds.center + parent.position;
        return parent.transform.position;
    }

    //Calculate position of explosionPoint that is bottom center of target object
    public Vector3 GetExplosionPoint()
    {
        //Initial bottomCenter
        float bottomCenter = 1000;
        
        //Store index of the piece at bottom
        int index = 0;

        //Do traversal to find the piece at bottom by checking y.
        for (int i = 1; i < subObj.Length; i++)
        {
            if (subObj[i].gameObject.transform.position.y < bottomCenter)
            {
                bottomCenter = subObj[i].gameObject.transform.position.y;
                index = i;
            }
        }

        //Acquire physical center of target object.
        Vector3 center = GetCenter(targetObj.transform);

        //Return explosion point.
        return new Vector3(center.x, bottomCenter - subObj[index].gameObject.transform.localScale.y, center.z);
    }

    //For test. Get name of static variable targetObject
    public string getTargetObjName()
    {
        return targetObj.name;
    }
 }
