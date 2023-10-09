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


    public RectTransform[] allPits;

    // Start is called before the first frame update
    void Start()
    {
        //p1pit0Transform = p1pit0.GetComponent<RectTransform>();
        //Debug.Log(p1pit0Transform.rotation);
        //p1pit0Transform.Rotate(0f, 0f, 180f, Space.Self);
        //FlipNumbers();
        UpdatePlayerTurnUI();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void FlipNumbers()
    {
        foreach (RectTransform pit in allPits)
        {
            pit.Rotate(0f, 0f, 180f, Space.Self);
        }
    }

    public TextMeshProUGUI playerTurnUI;
    public void UpdatePlayerTurnUI()
    {
        playerTurnUI.text = "Player " + GameMgr.inst.player;
    }


    IEnumerator DelayHelperUpdatePlayerTurnUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdatePlayerTurnUI();
    }

    public void DelayUpdatePlayerTurnUI(float delay)
    {
        StartCoroutine(DelayHelperUpdatePlayerTurnUI(delay));
    }
    
}
