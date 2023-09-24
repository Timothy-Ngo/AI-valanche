using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
}
