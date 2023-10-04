using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour
{
    public static CameraMgr inst;
    public Camera mCamera;
    public FirstPersonCamera fpc;
    public ArmRenderer armRenderer;

    public GameObject testP1;
    public GameObject testP2;


    public Transform p1CameraPos;
    public Transform p2CameraPos;

    private void Awake()
    {
        inst = this;
    }

    public void Start()
    {
        fpc = mCamera.gameObject.GetComponent<FirstPersonCamera>();
    }

    public void DelayedCameraSwitch(float delay)
    {
        StartCoroutine(DelayHelpCameraSwitch(delay));
    }
    IEnumerator DelayHelpCameraSwitch(float delay)
    {
        yield return new WaitForSeconds(delay);
        CameraSwitch();
    }
    public void CameraSwitch()
    {
        
        if (GameMgr.inst.player == 1)
        {
            mCamera.gameObject.transform.SetParent(p1CameraPos, false);
            fpc.ChangePlayerTransform(testP1.transform);
        }
        else if (GameMgr.inst.player == 2)
        {
            mCamera.gameObject.transform.SetParent(p2CameraPos, false);
            fpc.ChangePlayerTransform(testP2.transform);
        }
        armRenderer.ChangeActiveArm();
        UIMgr.inst.FlipNumbers();
        UIMgr.inst.UpdatePlayerTurnUI();
    }

    
}
