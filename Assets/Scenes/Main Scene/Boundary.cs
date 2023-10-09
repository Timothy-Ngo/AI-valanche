using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.gameObject.layer == GameMgr.inst.p1Layer)
        {

            if (GameMgr.inst.player == 2) 
            {
                mesh.enabled = false;
            }
        }
        else if (transform.parent.gameObject.layer == GameMgr.inst.p2Layer)
        {
            if (GameMgr.inst.player == 1)
            {
                mesh.enabled = false;
            }
        }
    }
}
