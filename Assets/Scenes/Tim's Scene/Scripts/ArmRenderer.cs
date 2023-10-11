using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRenderer : MonoBehaviour
{
    [Tooltip("How far foward the arm will move")]
    public float distance = 2.0f;

    public GameObject armRender1;
    public GameObject armRender2;

    
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
            MoveArmForward();
        }
    }

    public void ChangeActiveArm()
    {
        if (GameMgr.inst.player == 1)
        {
            armRender1.SetActive(true); 
            armRender2.SetActive(false);
        }
        else if (GameMgr.inst.player == 2)
        {
            armRender2.SetActive(true);
            armRender1.SetActive(false);
        }
    }

    IEnumerator DelayedMoveArmBack(int player)
    {
        yield return new WaitForSeconds(0.5f);
        MoveArmBack(player);
    }

    public void MoveArmForward()
    {
        if (GameMgr.inst.player == 1)
        {
            armRender1.transform.localPosition += Vector3.forward * distance;
            armIsForward = true;
            StartCoroutine(DelayedMoveArmBack(1));
        }
        else if (GameMgr.inst.player == 2 && !GameMgr.inst.againstAI)
        {
            armRender2.transform.localPosition += Vector3.forward * distance;
            armIsForward = true;
            StartCoroutine(DelayedMoveArmBack(2));
        }
        
    }

    public void MoveArmBack(int i )
    {
        if (i == 1)
        {
            armRender1.transform.localPosition -= Vector3.forward * distance;
        }
        else if (i == 2)
        {
            armRender2.transform.localPosition -= Vector3.forward * distance;
        }
        armIsForward = false;
    }

}
