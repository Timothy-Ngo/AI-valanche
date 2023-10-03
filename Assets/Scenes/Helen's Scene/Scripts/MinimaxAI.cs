using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class MinimaxAI : MonoBehaviour
{
    // Start is called before the first frame 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("key pressed");
            FindEvalutor(StateMgr.inst.currentState, ply, false);

        }
    }
    private int maxValue = int.MaxValue;
    public int ply = 2;
    public void FindEvalutor(State currentState, int depth, bool maximizingPlayer)
    {
        Dictionary<int, int> indexEvalMap = new Dictionary<int, int>();
        if (maximizingPlayer)
        {
            for (int i = 0; i < currentState.p1.Length - 1; i++)
            {
                if (currentState.p1[i] != 0)
                {
                    indexEvalMap[i] = Minimax(MoveP1(i, currentState), depth, -maxValue, maxValue, true);
                }
            }
        }
        else
        {
            for (int i = 0; i < currentState.p2.Length - 1; i++)
            {
                if (currentState.p2[i] != 0)
                {
                    indexEvalMap[i] = Minimax(MoveP2(i, currentState), depth, -maxValue, maxValue, false);
                }
            }
        }

        int minVal = int.MaxValue;
        int minIndex = 0;
        foreach(int index in indexEvalMap.Keys)
        {
            Debug.Log("index: " + index + ", eval: " + indexEvalMap[index]);
            if (indexEvalMap[index] <= minVal)
            {
                minVal = indexEvalMap[index];
                minIndex = index;
            }
        }

        StateMgr.inst.currentState.P2Move(minIndex);
        /*
        List<State> childStates = GetChildStates(currentState, maximizingPlayer);
        List<int> evaluators = new List<int>();
        foreach(State childState in childStates)
        {
            evaluators.Add(Minimax(childState, depth, maximizingPlayer));
        }

        if (maximizingPlayer)
        {
            int maxEvaluator = evaluators.Max();
            int maxIndex = evaluators.IndexOf(maxEvaluator);
            Debug.Log("maxIndex: " + maxIndex);
            return maxIndex;
        }
        else
        {
            int minEvaluator = evaluators.Min();
            int minIndex = evaluators.IndexOf(minEvaluator);
            Debug.Log("minIndex: " + minIndex);
            return minIndex;
        }
        */

    }
    public int Minimax(State currentState, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        if (depth == 0 || currentState.CheckEndState())
        {
            return StaticEvaluator(currentState, depth, alpha, beta, maximizingPlayer);
        }

        if (maximizingPlayer)
        {
            int maxEval = -maxValue;
            foreach(State childState in GetChildStates(currentState, !maximizingPlayer))
            {
                int eval = Minimax(childState, depth - 1, alpha, beta, false);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return maxEval;
        }
        else
        {
            int minEval = maxValue;
            foreach(State childState in GetChildStates(currentState, !maximizingPlayer))
            {
                int eval = Minimax(childState, depth - 1, alpha, beta, true);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return minEval;
        }
    }
    public int moveAgainWeight = 3;
    public int StaticEvaluator(State state, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        int staticEvaluator = state.p1[6] - state.p2[6];

        if (maximizingPlayer)
        {
            
            if (state.canP1MoveAgain)
            {
                //Debug.Log("P1 can move again");
                //staticEvaluator += moveAgainWeight;
                int maxEval = -maxValue; // look at this again
                foreach (State childState in GetChildStates(state, !maximizingPlayer)) //not sure if this should be inverted or not
                {
                    int eval = childState.p1[6] - childState.p2[6];
                    maxEval = Mathf.Max(maxEval, eval);
                }
                staticEvaluator = maxEval + moveAgainWeight;
            }
            else
            {
                staticEvaluator -= (moveAgainWeight / 2);
            }
        } 
        else
        {
            if (state.canP2MoveAgain)
            {
                //staticEvaluator -= moveAgainWeight;
                int minEval = maxValue; // look at this again
                foreach (State childState in GetChildStates(state, !maximizingPlayer)) //not sure if this should be inverted or not
                {
                    int eval = childState.p1[6] - childState.p2[6];
                    minEval = Mathf.Min(minEval, eval);
                }
                staticEvaluator = minEval - moveAgainWeight;
            }
            else
            {
                staticEvaluator += (moveAgainWeight / 2);
            }
        }
        //Debug.Log("inside static evaluator: " + staticEvaluator);
        return staticEvaluator;
    }

    public Stack<State> GetChildStates(State state, bool maximizingPlayer)
    {
        //List<State> states = new List<State>();
        Stack<State> states = new Stack<State>();
        if (state.canP2MoveAgain)
        {
            maximizingPlayer = !maximizingPlayer;
        }

        if (maximizingPlayer)
        {
            for (int i = 0; i < state.p1.Length - 1; i++)
            {
                if (state.p1[i] != 0)
                {
                    states.Push(new State(MoveP1(i, state)));
                }
            }
        }
        else
        {
            for (int i = 0; i < state.p2.Length - 1; i++)
            {
                if (state.p2[i] != 0)
                {
                    states.Push(new State(MoveP2(i, state)));
                }
            }
        }

        return states;
    }

    public State MoveP1(int indexOfPit, State state) // When passing in index check to make sure pit is not empty
    {
        State stateCopy = new State(state);
        int[] p1 = stateCopy.p1;
        int[] p2 = stateCopy.p2;

        if (indexOfPit == 6)
        {
            Debug.Log("Cannot move stones out of store");
            return stateCopy;
        }
        int numStones = p1[indexOfPit];
        if (numStones == 0)
        {
            Debug.Log("No stones to move");
            return stateCopy;
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
                if (p1[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
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
                        stateCopy.canP1MoveAgain = true;
                    }
                    return stateCopy;
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
                        return stateCopy;
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
                    if (p1[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
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
                            stateCopy.canP1MoveAgain = true;
                        }
                        return stateCopy;
                    }

                }
            }


        }

        return stateCopy;
    }

    public State MoveP2(int indexOfPit, State state) // When passing in index check to make sure pit is not empty
    {
        State stateCopy = new State(state);
        int[] p1 = stateCopy.p1;
        int[] p2 = stateCopy.p2;

        if (indexOfPit == 6)
        {
            Debug.Log("Cannot move stones out of store");
            return stateCopy;
        }
        int numStones = p2[indexOfPit];
        if (numStones == 0)
        {
            Debug.Log("No stones to move");
            return stateCopy;
        }

        p2[indexOfPit] = 0;


        // goes through player 1's side
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
                if (p2[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
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
                        stateCopy.canP2MoveAgain = true;
                    }
                    return stateCopy;
                }

            }
        }

        // if holding stones
        while (numStones > 0)
        {

            // goes through p2's side
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
                        return stateCopy;
                    }

                }
            }

            //Debug.Log("numberOfStones = " + numStones);


            // goes through p1's side
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
                    if (p2[i] > 1 && i != 6) // checks for >1 since pit will always have at least 1 stone from dropping one in earlier
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
                            stateCopy.canP2MoveAgain = true;
                        }
                        return stateCopy;
                    }

                }
            }


        }

        return stateCopy;
    }
}
