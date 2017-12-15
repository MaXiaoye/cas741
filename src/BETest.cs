using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor;

public class BETest {

    /**********************************************
     
        In all test cases, (X,Y,Z) = (a,b,c) means a target object with explosion point (a,b,c). 
        The target object for all test cases named "targetObj" consists of 32 cubes with size 1 x 1 x 1. 
        It locates under projectDir/Assets/prefab/targetObj.prefab. 
        The BreakingEffect script are attached on the prefab already with E = 5 and Mu = 0.2 

         ***********************************************/

    double delta = 0.01;
    void print(string s)
    {
        print(s);
    }

    [Test]
    public void CorrectIn()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
    }

    [Test]
    public void NoObj()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        //Input a "null" target object
        Assert.Catch<System.NullReferenceException>(() => BE.InputVerify(5, (float)0.2, GameObject.Find("Null")));
    }

    [Test]
    public void NoMu()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user does not input mu, Unity will consider it as 0 by default. Get the same exception invalid mu.
        Assert.Catch<System.Exception>(() => BE.InputVerify(5, (float)0, GameObject.Find("TargetObj")));
    }

    [Test]
    public void NoExpLv()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user does not input E, Unity will consider it as 0 by default. Get the same exception invalid E.
        Assert.Catch<System.Exception>(() => BE.InputVerify(0, (float)0.2, GameObject.Find("TargetObj")));
    }

    [Test]
    public void NonNumE()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user input E as non-number value, Unity will consider it as 0 by default. Get the same exception invalid E.
        Assert.Catch<System.Exception>(() => BE.InputVerify(0, (float)0.2, GameObject.Find("TargetObj")));
    }

    [Test]
    public void NonNumMu()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user input mu as non-number value, Unity will consider it as 0 by default. Get the same exception invalid mu.
        Assert.Catch<System.Exception>(() => BE.InputVerify(5, (float)0, GameObject.Find("TargetObj")));
    }

    [Test]
    public void OutScoE()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user input E with out of scope value. Get exception invalid mu.
        Assert.Catch<System.Exception>(() => BE.InputVerify(-1, (float)0.2, GameObject.Find("TargetObj")));
    }

    [Test]
    public void OutScoM()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // If user input Mu with out of scope value. Get the exception invalid mu.
        Assert.Catch<System.Exception>(() => BE.InputVerify(5, (float)99, GameObject.Find("TargetObj")));
    }

    //If user input x/y/z coordinate as non-number value, Unity will consider it as 0 by default.
    [Test]
    public void OutScoC()
    {
        // Use the Assert class to test conditions.
        BreakingEffect BE = new BreakingEffect();
        // Create a target object that has invalid coordinates.
        GameObject GO = new GameObject();
        GO.transform.position = new Vector3(0,99999,0);
        Assert.Catch<System.Exception>(() => BE.InputVerify(5, (float)0.2, GO));
    }

    [UnityTest]
    public IEnumerator pieceInit()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //Test if all piece objects are successfully initialed
        Assert.AreEqual(32, BE.pieceObj.Length);
        yield return null;
    }

    [UnityTest]
    public IEnumerator testInitialSpeed()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //Test if all piece objects are successfully initialed
        Assert.AreEqual(50, BE.pieceObj[0].initSpeed);
        yield return null;
    }

    [UnityTest]
    public IEnumerator testAngles1()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        Debug.Log("Explosion point is: " + BE.GetExplosionPoint());
        Debug.Log("Position of pieceObj[0] is: " + BE.pieceObj[0].obj.transform.position);
        Debug.Log("ThetaOne is: " + BE.pieceObj[0].thetaOne);
        Debug.Log("ThetaTwo is: " + BE.pieceObj[0].thetaTwo);
        Assert.AreEqual(0.90, BE.pieceObj[0].thetaOne, delta);
        Assert.AreEqual(1.25, BE.pieceObj[0].thetaTwo, delta);
        yield return null;
    }
    [UnityTest]
    public IEnumerator testAngles2()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        Debug.Log("Explosion point is: " + BE.GetExplosionPoint());
        Debug.Log("Position of pieceObj[1] is: " + BE.pieceObj[1].obj.transform.position);
        Debug.Log("ThetaOne is: " + BE.pieceObj[1].thetaOne);
        Debug.Log("ThetaTwo is: " + BE.pieceObj[1].thetaTwo);
        Assert.AreEqual(0.76, BE.pieceObj[1].thetaOne, delta);
        Assert.AreEqual(0.79, BE.pieceObj[1].thetaTwo, delta);
        yield return null;
    }

    [UnityTest]
    public IEnumerator testMotionAir()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Load ground with script
        //Instantiate ground
        GameObject ground = Object.Instantiate(Resources.Load("1/ground") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //Assert.AreEqual(0.79, BE.pieceObj[1].thetaTwo, delta);
        //Test displacement in the air in one frame
        Assert.AreEqual(0.59, BE.pieceObj[0].DisAirCalX(), delta);
        Assert.AreEqual(0.40, BE.pieceObj[0].DisAirCalY(), delta);
        Assert.AreEqual(0.20, BE.pieceObj[0].DisAirCalZ(), delta);
        yield return null;
    }

    [UnityTest]
    public IEnumerator testMotionGround()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Load ground with script
        //Instantiate ground
        GameObject ground = Object.Instantiate(Resources.Load("1/ground") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //Test displacement on the ground in one frame
        Assert.AreEqual(0.58, BE.pieceObj[0].DisGroCalX(), delta);
        Assert.AreEqual(0.19, BE.pieceObj[0].DisGroCalZ(), delta);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Constructor_PieceObj()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //UTPieceClass1
        Assert.AreEqual("Cube1", BE.pieceObj[0].obj.name);
        //UTPieceClass2
        Assert.AreEqual(false, BE.pieceObj[0].onGround);
        //UTPieceClass3
        Assert.AreEqual(50, BE.pieceObj[0].initSpeed);
        //UTPieceClass4
        Assert.AreEqual(-9.8, BE.pieceObj[0].gravity,delta);
        //UTPieceClass5
        Assert.AreEqual(false, BE.pieceObj[0].stop);
        //UTPieceClass6
        Assert.AreEqual(29.41, BE.pieceObj[0].speedLastFrameX, delta);
        //UTPieceClass7
        Assert.AreEqual(9.8, BE.pieceObj[0].speedLastFrameZ, delta);
        yield return null;
    }

    [UnityTest]
    public IEnumerator UTTarObj1()
    {
        //Load and instantiate target object
        GameObject targetObj = Object.Instantiate(Resources.Load("1/targetObj") as GameObject);
        //Get instance of script 
        BreakingEffect BE = targetObj.GetComponent<BreakingEffect>();
        //Call start to initial scene
        BE.Start();
        //Test if target object is successfully created by checking count of sub objects.
        Assert.AreEqual(32, targetObj.transform.childCount);
        yield return null;
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
	public IEnumerator BETestWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        GameObject go1 = Resources.Load("1/targetObj") as GameObject;
        Object.Instantiate(go1);
        //Debug.Log(go1.name);
        go1.name = "targetObj";
        go1.transform.name = "targetObj";
        //Debug.Log(go1.transform.childCount);
        //Debug.Log(GameObject.FindGameObjectWithTag("targetObj").name);
        //Debug.Log(GameObject.Find("targetObj").name);
        GameObject ground = Resources.Load("1/ground") as GameObject;
        BreakingEffect BE = ground.GetComponent<BreakingEffect>();
        BE.Start();
        Assert.AreEqual(32, BE.pieceObj.Length);
        yield return null;
	}
    
}
