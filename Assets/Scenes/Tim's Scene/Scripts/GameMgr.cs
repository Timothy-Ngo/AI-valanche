using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;

    private void Awake()
    {
        inst = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        p1Pits = new List<Pit>(p1PitsParent.GetComponentsInChildren<Pit>());
        p2Pits = new List<Pit>(p2PitsParent.GetComponentsInChildren<Pit>());
    }

    [Header("Game Mechanics")]
    public int player = 1;
    public bool moveInAction = false;
    public FirstPersonCamera fpc;
    

    [Header("State Renderer Settings")]
    public GameObject p1PitsParent;
    public GameObject p2PitsParent;
    public List<Pit> p1Pits;
    public List<Pit> p2Pits;

    [Header("Raycast Settings")]
    public Camera playerCamera;
    RaycastHit hit;
    public float rayCastDistance = 1000f;
    public int p1Layer = 1 << 6;
    public int p2Layer = 1 << 7;

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !moveInAction)
        {
            if (SendOutRayCast() )
            {
                Debug.Log("Raycast hit: " + hit.transform.parent.gameObject.name);
                int index = GetPitIndex(hit.transform.parent.gameObject);

                if (player == 1)
                {
                    StateMgr.inst.currentState.P1Move(index);
                }
                else if (player ==2)
                {
                    StateMgr.inst.currentState.P2Move(index);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EndGame(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EndGame(2);
        }

    }

    public bool SendOutRayCast()
    {
        
        if (player == 1)
        {
            return (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayCastDistance, p1Layer));
        }
        else if (player == 2)
        {
            return (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayCastDistance, p2Layer));
        }
        Debug.LogError("Not a valid player");
        return false;

    }

    public void DelayedChangeMoveInAction(float delay)
    {
        StartCoroutine(DelayHelpChangeMoveInAction(delay));
    }
    IEnumerator DelayHelpChangeMoveInAction(float delay)
    {
        yield return new WaitForSeconds(delay);
        moveInAction = !moveInAction;
    }

    public int GetPitIndex(GameObject pitObject)
    {
        if (player == 1)
        {
            return p1Pits.IndexOf(pitObject.GetComponent<Pit>());
        }
        else if (player == 2)
        {
            return p2Pits.IndexOf(pitObject.GetComponent<Pit>());
        }
        Debug.LogError("Not a valid player");
        return -1; 
    }


    public void EndGame(int winner)
    {
        if (winner == 1)
        {
            fpc.ReleaseCursor();
            SceneMgr.inst.ChangeSceneToEndGame("Player 1 Win Screen");
        }
        else if (winner == 2)
        {
            fpc.ReleaseCursor();
            SceneMgr.inst.ChangeSceneToEndGame("Player 2 Win Screen");
        }
    }


}
