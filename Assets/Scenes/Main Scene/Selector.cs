using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public MeshRenderer floorBoundary;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("working");
        if (GameMgr.inst.player == 1 && transform.parent.gameObject.layer == GameMgr.inst.p1Layer)
        {
            floorBoundary.enabled = true;
        }
        else if (GameMgr.inst.player == 2 && transform.parent.gameObject.layer == GameMgr.inst.p2Layer)
        {
            floorBoundary.enabled = true;
        }

    }

    public void OnPointerExit(PointerEventData eventData) 
    {
        if (GameMgr.inst.player == 1 && transform.parent.gameObject.layer == GameMgr.inst.p1Layer)
        {
            floorBoundary.enabled = false;
        }
        else if (GameMgr.inst.player == 2 && transform.parent.gameObject.layer == GameMgr.inst.p2Layer)
        {
            floorBoundary.enabled = false;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
