using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{ 
    private GameObject gameObject;
    float x1;
    float x2;
    float x3;
    float x4;
    public float near = 20.0f;
    public float far = 100.0f;

    public float sensitivetyZ = 2f;
    public float sensitivityX = 10f;
    public float sensitivityY = 10f;
    public float sensitivetyMove = 2f;
    public float sensitivetyMouseWheel = 2f;

    // Use this for initialization 
    void Start()
    {
        gameObject = GameObject.Find("Main Camera");
    }

    // Update is called once per frame  
    void Update()
    {
        //Left button to rotate on vertical direction
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * sensitivityX;
            transform.Rotate(0, rotationX, 0);
        }

        //Space button to rotate on horizontal direction
        if (Input.GetKey(KeyCode.Space))
        {
            float rotationY = Input.GetAxis("Mouse Y") * sensitivityY;
            transform.Rotate(-rotationY, 0, 0);
        }

        //w move forward 
        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, 50 * Time.deltaTime));
        }
        //s move backword  
        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, -50 * Time.deltaTime));
        }
        //a move left  
        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-50 * Time.deltaTime, 0, 0));
        }
        //d move right
        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(50 * Time.deltaTime, 0, 0));
        }
    }
}