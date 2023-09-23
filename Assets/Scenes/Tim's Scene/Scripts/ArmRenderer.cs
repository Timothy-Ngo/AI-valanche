using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRenderer : MonoBehaviour
{
    [Tooltip("How far foward the arm will move")]
    public float distance = 2.0f;

    private bool armIsForward = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Mouse click");
            if (!armIsForward){
                MoveArmForward();
            }
            else
            {
                MoveArmBack();
            }
        }
    }

    public void MoveArmForward()
    {
        transform.localPosition += Vector3.forward * distance;
        armIsForward = true;
    }

    public void MoveArmBack()
    {
        transform.localPosition -= Vector3.forward * distance;
        armIsForward = false;
    }

}
