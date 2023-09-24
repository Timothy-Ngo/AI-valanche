using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMgr : MonoBehaviour
{

    // current state initialized to starting board state
    public State currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new State();

        //PrintState(currentState);

        //Debug.Log("game ends: " + currentState.CheckEndGame());
        currentState.P2Move(0);
        //currentState.P2Move(0);
        Debug.Log("can move again: " + currentState.canP2MoveAgain);
        //currentState.GetValidMoves(currentState.p1);

        //PrintValidMoves(currentState);
        //Debug.Log(currentState.WhoWon());
        PrintState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        p1 = new int[7] { 4,4,4,4,4,4,0 };

        p2 = new int[7] { 4,4,4,4,4,4,0 };
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
    public bool CheckEndGame()
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

        return p1Empty || p2Empty;
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

    public void P1Move(int indexOfPit) // When passing in index check to make sure pit is not empty
    {
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
        
        p1[indexOfPit] = 0;


        // goes through player 1's side
        for (int i = indexOfPit + 1; i < p1.Length; i++) // while nextIndex < 7
        {
            // if holding stones
            if (numStones > 0)
            {
                // drops a stone
                numStones--;
                p1[i]++; 
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
                }
                else
                {
                    // if ended in your store, then you can move again
                    if (i == 6)
                    {
                        canP1MoveAgain = true;
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
                    }
                    else
                    {
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
                }

                // if holding no stones
                if (numStones == 0) // checks if pit has one stone since we dropped one in earlier
                {
                    // if current pit has stones
                    if (p1[i] > 1) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p1[i];
                        p1[i] = 0;
                    }
                    else
                    {
                        // if ended in your store, then you can move again
                        if (i == 6)
                        {
                            canP1MoveAgain = true;
                        }
                        return;
                    }
                  
                }
            }


        }

        

    }

    public void P2Move(int indexOfPit) // When passing in index check to make sure pit is not empty
    {


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

        p2[indexOfPit] = 0;


        // goes through player 2's side
        for (int i = indexOfPit + 1; i < p2.Length; i++) // while nextIndex < 7
        {
            // if holding stones
            if (numStones > 0)
            {
                // drops a stone
                numStones--;
                p2[i]++;
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
                }
                else
                {
                    // if ended in your store, then you can move again
                    if (i == 6)
                    {
                        canP2MoveAgain = true;
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
                    }
                    else
                    {
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
                }

                // if holding no stones
                if (numStones == 0) // checks if pit has one stone since we dropped one in earlier
                {
                    // if current pit has stones
                    if (p2[i] > 1) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
                    {
                        // pick up stones from current pit
                        numStones = p2[i];
                        p2[i] = 0;
                    }
                    else
                    {
                        // if ended in your store, then you can move again
                        if (i == 6)
                        {
                            canP2MoveAgain = true;
                        }
                        return;
                    }

                }
            }

        }



    }
}
