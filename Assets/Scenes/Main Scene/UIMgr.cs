using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    public static UIMgr inst;

    private void Awake()
    {
        inst = this;
    }


    public RectTransform[] allPits;
    public GameObject extraTurn;

    // Start is called before the first frame update
    void Start()
    {
        //p1pit0Transform = p1pit0.GetComponent<RectTransform>();
        //Debug.Log(p1pit0Transform.rotation);
        //p1pit0Transform.Rotate(0f, 0f, 180f, Space.Self);
        //FlipNumbers();
        UpdatePlayerTurnUI();
        endScreen.SetActive(false);
        UpdateExtraTurnUI();
    }

    // Update is called once per frame
    void Update()
    {
        DelayUpdateExtraTurnUI(0.05f);
        if (onEndScreen)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneMgr.inst.ChangeSceneToMainMenu();
                onEndScreen = false;
                GameMgr.inst.fpc.ReleaseCursor();
            }
        }
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
        if (SceneManager.GetActiveScene().name == "Play Against AI")
        {
            if (GameMgr.inst.player == 1)
            {
                playerTurnUI.text = "Your turn";
            }
            else
            {
                playerTurnUI.text = "AI turn";
            }
        }
        else
        {
            playerTurnUI.text = "Player " + GameMgr.inst.player;
        }
        
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

    public void UpdateExtraTurnUI()
    {
        if (StateMgr.inst.currentState.canP1MoveAgain || StateMgr.inst.currentState.canP2MoveAgain)
        {
            Debug.Log("extra turn");
            extraTurn.SetActive(true);
        }
        else
        {
            extraTurn.SetActive(false);
        }

        if(onEndScreen)
        {
            extraTurn.SetActive(false);
        }
    }

    IEnumerator DelayHelperUpdateExtraTurnUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateExtraTurnUI();
    }

    public void DelayUpdateExtraTurnUI(float delay)
    {
        StartCoroutine(DelayHelperUpdateExtraTurnUI(delay));
    }

    public TextMeshProUGUI endScreenUI;
    public GameObject endScreen;
    bool onEndScreen = false;

    public AudioSource winSfx;
    public AudioSource lostSfx;
    public void DisplayEndScreen()
    {
        
        onEndScreen = true;
        Time.timeScale = 0;
        playerTurnUI.enabled = false;
        
        if (SceneManager.GetActiveScene().name == "Play Against AI")
        {
            extraTurn.SetActive(false);
            if (StateMgr.inst.currentState.WhoWon() == 2)
            {
                endScreenUI.text = "AI Won!";
                lostSfx.Play();
            }
            else if (StateMgr.inst.currentState.WhoWon() == 1)
            {
                endScreenUI.text = "You Won!";
                winSfx.Play();
            } else
            {
                endScreenUI.text = "Draw";
                lostSfx.Play();
            }
        } 
        else
        {
            if (StateMgr.inst.currentState.WhoWon() == 0)
            {
                endScreenUI.text = "Draw";
                lostSfx.Play();
            }
            else
            {

                endScreenUI.text = "Player " + StateMgr.inst.currentState.WhoWon().ToString() + " Won!";
                winSfx.Play();
            }
        }

        
        endScreen.SetActive(true);
        

    }
    
}
