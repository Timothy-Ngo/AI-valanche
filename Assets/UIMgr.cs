using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMgr : MonoBehaviour
{
    public static UIMgr inst;

    private void Awake()
    {
        inst = this;
    }

    [Header("Player 1 Pits")]
    public TextMeshProUGUI p1pit0;
    public TextMeshProUGUI p1pit1;
    public TextMeshProUGUI p1pit2;
    public TextMeshProUGUI p1pit3;
    public TextMeshProUGUI p1pit4;
    public TextMeshProUGUI p1pit5;
    public TextMeshProUGUI p1Store;

    [Header("Player 2 Pits")]
    public TextMeshProUGUI p2pit0;
    public TextMeshProUGUI p2pit1;
    public TextMeshProUGUI p2pit2;
    public TextMeshProUGUI p2pit3;
    public TextMeshProUGUI p2pit4;
    public TextMeshProUGUI p2pit5;
    public TextMeshProUGUI p2Store;

    public RectTransform[] allPits;

    // Start is called before the first frame update
    void Start()
    {
        //p1pit0Transform = p1pit0.GetComponent<RectTransform>();
        //Debug.Log(p1pit0Transform.rotation);
        //p1pit0Transform.Rotate(0f, 0f, 180f, Space.Self);
        //FlipNumbers();
        
    }

    // Update is called once per frame
    void Update()
    {

       // UpdateNumbers();

    }

    /*public void UpdateNumbers()
    {
        p1pit0.text = StateMgr.inst.currentState.p1[0].ToString();
        p1pit1.text = StateMgr.inst.currentState.p1[1].ToString();
        p1pit2.text = StateMgr.inst.currentState.p1[2].ToString();
        p1pit3.text = StateMgr.inst.currentState.p1[3].ToString();
        p1pit4.text = StateMgr.inst.currentState.p1[4].ToString();
        p1pit5.text = StateMgr.inst.currentState.p1[5].ToString();
        p1Store.text = StateMgr.inst.currentState.p1[6].ToString();

        p2pit0.text = StateMgr.inst.currentState.p2[0].ToString();
        p2pit1.text = StateMgr.inst.currentState.p2[1].ToString();
        p2pit2.text = StateMgr.inst.currentState.p2[2].ToString();
        p2pit3.text = StateMgr.inst.currentState.p2[3].ToString();
        p2pit4.text = StateMgr.inst.currentState.p2[4].ToString();
        p2pit5.text = StateMgr.inst.currentState.p2[5].ToString();
        p2Store.text = StateMgr.inst.currentState.p2[6].ToString();

    }*/

    public void FlipNumbers()
    {
        foreach (RectTransform pit in allPits)
        {
            pit.Rotate(0f, 0f, 180f, Space.Self);
        }
    }
}
