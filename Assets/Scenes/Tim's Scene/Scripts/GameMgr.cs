using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;

    public AudioSource penguinSfx;
    private void Awake()
    {
        inst = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        p1Pits = new List<Pit>(p1PitsParent.GetComponentsInChildren<Pit>());
        p2Pits = new List<Pit>(p2PitsParent.GetComponentsInChildren<Pit>());

        difficultyLevel = Difficulty.inst.difficulty;
        if (againstAI)
        {
            inputBlocker.SetActive(false);
        }
    }

    [Header("Game Mechanics")]
    public int player = 1;
    public bool moveInAction = false;
    public FirstPersonCamera fpc;
    public bool againstAI = false;
    public MinimaxAI ai;
    public int difficultyLevel = 1;
    public GameObject inputBlocker;

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

    public MeshRenderer lastHitRenderer = null;

    void Update()
    {
        if (againstAI && player == 2 && !inputBlocker.activeSelf)
        {
            Debug.Log("input blocker active");
            inputBlocker.SetActive(true);
        }
        else if (againstAI && player == 1 && inputBlocker.activeSelf)
        {
            Debug.Log("input blocker inactive");
            inputBlocker.SetActive(false);  
        }

        
        if (player == 1)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayCastDistance, p1Layer) && !moveInAction)
            {
                if (lastHitRenderer != null && lastHitRenderer != hit.collider.gameObject.GetComponent<MeshRenderer>())
                {
                    lastHitRenderer.enabled = false;
                }
                lastHitRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                lastHitRenderer.enabled = true;
            }
            else
            {
                if (lastHitRenderer != null)
                {
                    lastHitRenderer.enabled = false;
                }
            }
        }
        else if (player == 2 && !againstAI)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayCastDistance, p2Layer) && !moveInAction)
            {
                lastHitRenderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                lastHitRenderer.enabled = true;
            }
            else
            {
                if (lastHitRenderer != null)
                {
                    lastHitRenderer.enabled = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !moveInAction)
        {
            penguinSfx.PlayOneShot(penguinSfx.clip);
            if (player == 1 && inputBlocker.activeSelf)
            {
                inputBlocker.SetActive(false);
            }
            if (SendOutRayCast() )
            {
                if (lastHitRenderer!= null)
                {
                    lastHitRenderer.enabled = false;
                }
                //Debug.Log("Raycast hit: " + hit.transform.parent.gameObject.name);
                int index = GetPitIndex(hit.transform.parent.gameObject);

                if (player == 1)
                {
                    
                    StateMgr.inst.currentState.P1Move(index);
                    
                }
                else if (player == 2)
                {
                    if (!againstAI)
                    {
                        
                        StateMgr.inst.currentState.P2Move(index);
                        
                    }

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

    public void DelayedPlayAgainstAI(float delay)
    {

        StartCoroutine(DelayHelperPlayAgainstAI(delay));
    }

    IEnumerator DelayHelperPlayAgainstAI(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        UIMgr.inst.UpdatePlayerTurnUI();
        inputBlocker.SetActive(true);
        PlayAgainstAI();
        penguinSfx.PlayOneShot(penguinSfx.clip);
    }

   
    public void PlayAgainstAI()
    {
        if (againstAI)
        {
            Debug.Log("Against AI");
            ai.FindEvaluator(StateMgr.inst.currentState, difficultyLevel, false);
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
    
    public void DelayedEndGame(float delay, int winner)
    {
        StartCoroutine(DelayHelperEndGame(delay, winner));
    }
    IEnumerator DelayHelperEndGame(float delay, int winner)
    {
        yield return new WaitForSeconds(delay);
        EndGame(winner);
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
