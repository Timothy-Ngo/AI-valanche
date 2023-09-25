using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /*
    public GameObject transitionCamera;
    public GameObject p1Camera;
    public GameObject p2Camera;
    public GameObject pathP1toP2;
    public GameObject pathP2toP1;

    // Start is called before the first frame update
    void Start()
    {
        p1Camera.SetActive(true);
        p2Camera.SetActive(false);
        transitionCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            if (p1Camera.activeSelf == true)
            {
                SwitchToP2();
            } 
            else
            {
                SwitchToP1();
            }

        }

        if (transitionCamera.transform.position == p2Camera.transform.position)
        {
            transitionCamera.SetActive(false);
            p2Camera.SetActive(true);
        }
        if (transitionCamera.transform.position == p1Camera.transform.position)
        {
            transitionCamera.SetActive(false);
            p1Camera.SetActive(true);
        }
    }

    public void SwitchToP2()
    {
        transitionCamera.SetActive(true);
        p1Camera.SetActive(false);

        pathP1toP2.SetActive(true);
        pathP2toP1.SetActive(false);
        
    }

    public void SwitchToP1()
    {
        transitionCamera.SetActive(true);
        p2Camera.SetActive(false);

        pathP2toP1.SetActive(true);
        pathP1toP2.SetActive(false);


    }
    */

    public Transform camera;
    public GameObject player1Arm;
    public GameObject player2Arm;

    public GameObject pathP2ToP1;
    public GameObject pathP1ToP2;

    Vector3 p1Position;
    Vector3 p2Position;

    void Start()
    {
        player1Arm.SetActive(false);
        player2Arm.SetActive(false);

        pathP1ToP2.SetActive(false);
        pathP2ToP1.SetActive(false);

        p1Position = new Vector3(0f, 41.14f, -48.93f);
        p2Position = new Vector3(-0.2f, 41.2f, 33.7f);
    }

    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (pathP1ToP2.activeSelf)
            {
                SwitchToP1();
            }
            else
            {
                SwitchToP2();
            }
        }

        if (camera.position == p1Position)
        {
            player1Arm.SetActive(true);
            player2Arm.SetActive(false);
        }
        else if (camera.position == p2Position)
        {
            player1Arm.SetActive(false);
            player2Arm.SetActive(true);
        }
    }

    public void SwitchToP2()
    {
        pathP2ToP1.SetActive(false);
        pathP1ToP2.SetActive(true);
    }

    public void SwitchToP1()
    {
        pathP2ToP1.SetActive(true);
        pathP1ToP2.SetActive(false);
    }
}
