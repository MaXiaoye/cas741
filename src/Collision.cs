using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

    static GameObject[] targetObj;
    BreakingEffect[] BE;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < BE.Length; i++)
        {
            for (int j = 0; j < BE[i].pieceObj.Length; j++)
            {
                /*if (BE.pieceObj[i].obj.name == other.gameObject.name)
                {
                    BE.pieceObj[i].onGround = true;
                }*/
                if (BE[i].pieceObj[j].obj.GetInstanceID() == other.gameObject.GetInstanceID())
                {
                    BE[i].pieceObj[j].onGround = true;
                }
            }
        }      
    }

    // Use this for initialization
    void Start () {
        targetObj = GameObject.FindGameObjectsWithTag("targetObj");
        BE = new BreakingEffect[targetObj.Length];
        //GameObject.FindGameObjectsWithTag("targetObj");
        for (int i = 0; i < targetObj.Length; i++)
        {
            BE[i] = targetObj[i].GetComponent<BreakingEffect>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
