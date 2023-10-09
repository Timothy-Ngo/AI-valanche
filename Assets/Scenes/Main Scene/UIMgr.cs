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

    // Start is called before the first frame update
    void Start()
    {
        //p1pit0Transform = p1pit0.GetComponent<RectTransform>();
        //Debug.Log(p1pit0Transform.rotation);
        //p1pit0Transform.Rotate(0f, 0f, 180f, Space.Self);
        //FlipNumbers();
        UpdatePlayerTurnUI();
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
            if (StateMgr.inst.currentState.WhoWon() == 2)
            {
                endScreenUI.text = "AI Won!";
                lostSfx.Play();
            }
            else
            {
                endScreenUI.text = "You Won!";
                winSfx.Play();
            }
        } 
        else
        {
            endScreenUI.text = "Player " + StateMgr.inst.currentState.WhoWon().ToString() + " Won!";
            winSfx.Play();
        }

        
        endScreen.SetActive(true);
        

    }
    
}
