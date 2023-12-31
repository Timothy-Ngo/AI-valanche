using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMgr : MonoBehaviour
{
    public static StateMgr inst;

    private void Awake()
    {
        inst = this;
    }
    // current state initialized to starting board state
    public State currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new State();
        
        //PrintState(currentState);

        //Debug.Log("game ends: " + currentState.CheckEndGame());
        //currentState.P2Move(0);
        //currentState.P2Move(0);
        //Debug.Log("can move again: " + currentState.canP2MoveAgain);
        //currentState.GetValidMoves(currentState.p1);

        //PrintValidMoves(currentState);
        //Debug.Log(currentState.WhoWon());
        //PrintState(currentState);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(currentState.CheckEndState());
        }
    }


    // DEBUGGING

    public void PrintState(State state)
    {
        Debug.Log("--------------------");

        string stateString1 = "p1: ";
        string stateString2 = "p2: ";
        foreach (int numStone in state.p1)
        {
            //Debug.Log(numStone);
            stateString1 += numStone.ToString() + ",";
        }

        foreach (int numStone in state.p2)
        {
            //Debug.Log(numStone);
            stateString2 += numStone.ToString() + ",";
        }
        Debug.Log(stateString1);
        Debug.Log(stateString2);

        Debug.Log("p1 score: " + state.GetP1Score());
        Debug.Log("p2 score: " + state.GetP2Score());  
    }

    public void PrintValidMoves(State state)
    {
        string validMovesString = "";
        foreach (int index in state.GetValidMoves(state.p2))
        {
            validMovesString += index.ToString() + ",";
        }

        Debug.Log("valid moves (indexes): " + validMovesString);
    }
}

[System.Serializable]
public class State
{
    public int[] p1;

    public int[] p2;

    public bool canP1MoveAgain = false;

    public bool canP2MoveAgain = false;

    public State()
    {
        p1 = new int[7] { 4, 4, 4, 4, 4, 4, 0 };

        p2 = new int[7] { 4, 4, 4, 4, 4, 4, 0 };
    }

    public State(State state)
    {
        p1 = new int[7] { 4, 4, 4, 4, 4, 4, 0 };

        p2 = new int[7] { 4, 4, 4, 4, 4, 4, 0 };

        Array.Copy(state.p1, p1, p1.Length);
        Array.Copy(state.p2, p2, p2.Length);

        canP1MoveAgain = state.canP1MoveAgain;
        canP2MoveAgain = state.canP2MoveAgain;
    }


    public int GetP1Score()
    {
        return p1[6];
    }

    public int GetP2Score()
    {
        return p2[6];
    }
    public int WhoWon()
    {
        if (GetP1Score() > GetP2Score())
        {
            return 1; // Player 1 won
        }
        else if (GetP2Score() > GetP1Score())
        {
            return 2; // Player 2 won
        }
        else
        {
            return 0; // The game is a draw
        }
    }

    
    public void CheckEndGame(float delay)
    {
        bool p1Empty = true;
        bool p2Empty = true;

        for (int i = 0; i < p1.Length - 1; i++)
        {
            if (p1[i] > 0)
            {
                p1Empty = false;
                break;
            }
        }

        for (int i = 0; i < p2.Length - 1; i++)
        {
            if (p2[i] > 0)
            {
                p2Empty = false;
                break;
            }
        }

        if (p1Empty || p2Empty || p1[6] >= 25 || p2[6] >= 25)
        {
            UIMgr.inst.DisplayEndScreen();
        }
        else
        {
            return;
        }
    }

    public bool CheckEndState()
    {
        bool p1Empty = true;
        bool p2Empty = true;
        for (int i = 0; i < p1.Length - 1; i++)
        {
            if (p1[i] > 0)
            {
                p1Empty = false;
                break;
            }
        }

        for (int i = 0; i < p2.Length - 1; i++)
        {
            if (p2[i] > 0)
            {
                p2Empty = false;
                break;
            }
        }


        return p1Empty || p2Empty || p1[6] >= 25 || p2[6] >= 25;
    }

    public List<int> GetValidMoves(int[] player)
    {
        List<int> validMoves = new List<int>();
        for (int index = 0; index < player.Length - 1; index++)
        {
            if (player[index] != 0)
            {
                validMoves.Add(index);
            }
        }
        return validMoves;
    }

    public float spawnInterval = 0.05f;

    public void P1Move(int indexOfPit) // When passing in index check to make sure pit is not empty
    {
        if (canP1MoveAgain)
        {
            canP1MoveAgain = false;
        }
        float spawnDuration = 0;

        if (indexOfPit == 6)
        {
            Debug.Log("Cannot move stones out of store");
            return;
        }
        int numStones = p1[indexOfPit];
        if (numStones == 0)
        {
            Debug.Log("No stones to move");
            return;
        }
        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
        p1[indexOfPit] = 0;
        GameMgr.inst.p1Pits[indexOfPit].DelayedClearPit(spawnDuration += spawnInterval);

        // goes through player 1's side
        for (int i = indexOfPit + 1; i < p1.Length; i++) // while nextIndex < 7
        {
            // if holding stones
            if (numStones > 0)
            {
                // drops a stone
                numStones--;
                p1[i]++;
                GameMgr.inst.p1Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
            }

            // if holding no stones
            if (numStones == 0) 
            {
                // if current pit has stones
                if (p1[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                {
                    // pick up stones from current pit
                    numStones = p1[i];
                    p1[i] = 0;
                    GameMgr.inst.p1Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                }
                else
                {
                    // if ended in your store, then you can move again
                    if (i == 6)
                    {
                        canP1MoveAgain = true;
                    }
                    else
                    {
                        GameMgr.inst.player = 2;
                        
                        if (!GameMgr.inst.againstAI)
                        { 
                            CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                        }
                    }
                    GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                    if (CheckEndState())
                    {
                        CheckEndGame(spawnDuration += spawnInterval + 1);
                    }
                    else
                    {
                        
                        if (GameMgr.inst.againstAI && !canP1MoveAgain)
                        {
                            Debug.Log("Against AI");
                            GameMgr.inst.DelayedPlayAgainstAI(spawnDuration += spawnInterval + 1);
                        }
                    }
                    return;
                }
                
            }
        }

        // if holding stones
        while (numStones > 0)
        {

            // goes through p2's side
            for (int i = 0; i < p2.Length - 1; i++)
            {

                // if holding stones
                if (numStones > 0)
                {
                    // drops stone
                    numStones--;
                    p2[i]++;
                    GameMgr.inst.p2Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
                }

                // if holding no stones
                if (numStones == 0) 
                {
                    // if current pit has stones
                    if (p2[i] > 1) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p2[i];
                        p2[i] = 0;
                        GameMgr.inst.p2Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                    }
                    else
                    {
                        GameMgr.inst.player = 2;
                        
                        if (!GameMgr.inst.againstAI)
                        {
                            CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                        }
                        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                        if (CheckEndState())
                        {
                            CheckEndGame(spawnDuration += spawnInterval + 1);
                        }
                        else
                        {
                            if (GameMgr.inst.againstAI)
                            {
                                
                                GameMgr.inst.DelayedPlayAgainstAI(spawnDuration += spawnInterval + 1);
                            }
                        }
                        return;
                    }
                    
                }
            }

            //Debug.Log("numberOfStones = " + numStones);


            // goes through p1's side
            for (int i = 0; i < p1.Length; i++)
            {
                // if holding stones
                if (numStones > 0)
                {
                    // drop a stone
                    numStones--;
                    p1[i]++;
                    GameMgr.inst.p1Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
                }

                // if holding no stones
                if (numStones == 0) // checks if pit has one stone since we dropped one in earlier
                {
                    // if current pit has stones
                    if (p1[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p1[i];
                        p1[i] = 0;
                        GameMgr.inst.p1Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                    }
                    else
                    {
                        // if ended in your store, then you can move again
                        if (i == 6)
                        {
                            canP1MoveAgain = true;
                        }
                        else
                        {
                            GameMgr.inst.player = 2;
                            if (!GameMgr.inst.againstAI)
                            {
                                CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                            }
                        }
                        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                        if (CheckEndState())
                        {
                            CheckEndGame(spawnDuration += spawnInterval + 1);
                        }
                        else
                        {
                            if (GameMgr.inst.againstAI && !canP1MoveAgain)
                            {
                                Debug.Log("Against AI");
                                GameMgr.inst.DelayedPlayAgainstAI(spawnDuration += spawnInterval + 1);
                            }
                        }
                        return;
                    }
                  
                }
            }


        }

        

    }

    public void P2Move(int indexOfPit) // When passing in index check to make sure pit is not empty
    {
        if (canP2MoveAgain)
        {
            canP2MoveAgain = false;
        }
        float spawnDuration = 0;

        if (indexOfPit == 6)
        {
            Debug.Log("Cannot move stones out of store");
            return;
        }
        int numStones = p2[indexOfPit];
        if (numStones == 0)
        {
            Debug.Log("No stones to move");
            return;
        }
        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);

        p2[indexOfPit] = 0;
        GameMgr.inst.p2Pits[indexOfPit].DelayedClearPit(spawnDuration += spawnInterval);

        // goes through player 2's side
        for (int i = indexOfPit + 1; i < p2.Length; i++) // while nextIndex < 7
        {
            // if holding stones
            if (numStones > 0)
            {
                // drops a stone
                numStones--;
                p2[i]++;
                GameMgr.inst.p2Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
            }

            // if holding no stones
            if (numStones == 0)
            {
                // if current pit has stones
                if (p2[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                {
                    // pick up stones from current pit
                    numStones = p2[i];
                    p2[i] = 0;
                    GameMgr.inst.p2Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                }
                else
                {
                    // if ended in your store, then you can move again
                    if (i == 6)
                    {
                        canP2MoveAgain = true;
                    }
                    else
                    {
                        GameMgr.inst.player = 1;
                        if (!GameMgr.inst.againstAI)
                        {
                            CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                        }
                    }
                    GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                    if (CheckEndState())
                    {
                        CheckEndGame(spawnDuration += spawnInterval + 1);
                    }
                    else
                    {
                        if (GameMgr.inst.againstAI && canP2MoveAgain)
                        {
                            GameMgr.inst.DelayedPlayAgainstAI(spawnDuration += spawnInterval + 1);
                            canP2MoveAgain = false;
                        }
                        UIMgr.inst.DelayUpdatePlayerTurnUI(spawnDuration += spawnInterval + 1);
                    }
                    return;
                }

            }
        }

        // if holding stones
        while (numStones > 0)
        {

            // goes through p1's side
            for (int i = 0; i < p1.Length - 1; i++)
            {

                // if holding stones
                if (numStones > 0)
                {
                    // drops stone
                    numStones--;
                    p1[i]++;
                    GameMgr.inst.p1Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
                }

                // if holding no stones
                if (numStones == 0)
                {
                    // if current pit has stones
                    if (p1[i] > 1) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p1[i];
                        p1[i] = 0;
                        GameMgr.inst.p1Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                    }
                    else
                    {
                        GameMgr.inst.player = 1;
                        if (!GameMgr.inst.againstAI)
                        {
                            CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                        }
                        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                        if (CheckEndState())
                        {
                            CheckEndGame(spawnDuration += spawnInterval + 1);
                        }
                        else
                        {
                            UIMgr.inst.DelayUpdatePlayerTurnUI(spawnDuration += spawnInterval + 1);
                        }
                        return;
                    }

                }
            }

            //Debug.Log("numberOfStones = " + numStones);


            // goes through p2's side
            for (int i = 0; i < p2.Length; i++)
            {
                // if holding stones
                if (numStones > 0)
                {
                    // drop a stone
                    numStones--;
                    p2[i]++;
                    GameMgr.inst.p2Pits[i].DelayedAddStone(spawnDuration += spawnInterval);
                }

                // if holding no stones
                if (numStones == 0) // checks if pit has one stone since we dropped one in earlier
                {
                    // if current pit has stones
                    if (p2[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p2[i];
                        p2[i] = 0;
                        GameMgr.inst.p2Pits[i].DelayedClearPit(spawnDuration += spawnInterval);
                    }
                    else
                    {
                        // if ended in your store, then you can move again
                        if (i == 6)
                        {
                            canP2MoveAgain = true;
                        }
                        else
                        {
                            GameMgr.inst.player = 1;
                            if (!GameMgr.inst.againstAI)
                            {
                                CameraMgr.inst.DelayedCameraSwitch(spawnDuration += spawnInterval + 1);
                            }
                        }
                        GameMgr.inst.DelayedChangeMoveInAction(spawnDuration += spawnInterval);
                        if (CheckEndState())
                        {
                            CheckEndGame(spawnDuration += spawnInterval + 1);
                        }
                        else
                        {
                            if (GameMgr.inst.againstAI)
                            {
                                if (canP2MoveAgain)
                                {
                                    GameMgr.inst.DelayedPlayAgainstAI(spawnDuration += spawnInterval + 1);
                                    canP2MoveAgain = false;
                                }
                                else
                                {
                                    UIMgr.inst.DelayUpdatePlayerTurnUI(spawnDuration += spawnInterval + 1);
                                }
                            }
                            
                        }
                        return;
                    }

                }
            }

        }

        


    }

    

}
