//Benjamin Apprill
//7/9/2016
//Credit:  http://docs.unity3d.com/Manual/nav-CreateNavMeshAgent.html

using UnityEngine;
using System.Collections;

//Determines pathing for NPC.  Takes inspector transforms into an array and randomizes them for the NavAgent to travel through.
//Also re-randomizes the path once the current one is completed.
public class NPCController : MonoBehaviour {


    //Can add in other states if need be
    enum fearCase { feared, notFeared };

    //Keeps track of fear state as a boolean
    private bool hasBeenFeared;

    //Stores the curent fear state
    private fearCase fearState;

    //Current value of the NPC's fear
    private float fearCurrent;

    //The integer value for the current destination element in the navPoints array
    private int currentDest;

    //Holds the NavMeshAgent
    private NavMeshAgent agent;

    //Check in inspector to randomize the navPoints array
    [SerializeField]
    bool randomizeNavPoints;
    
    //Max fear value
    [SerializeField]
    float fearMax = 100;

    //An amount of fear required for the fearState to reset to not feared
    [SerializeField]
    float fearReset = 0.0f;
    
    //Distance from a point required to iterate to the next element of the navPoints array
    [SerializeField]
    float nextPointDist;

    //Value for how much the fear reset should scale upon entering fear state
    [SerializeField]
    float scaleFearReset;

    //Value for chaning how fast the fear state ends
    [SerializeField]
    float scaleVal;

    //Need points for the NavAgent to travel to
    [SerializeField]
    GameObject exitPoint;

    //Need points for the NavAgent to travel to
    [SerializeField]
    GameObject[] navPoints;


    void Start()
    {

        //Assigns NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        //Initial destination is first element of the array
        currentDest = 0;

        //Sets initial fearState to not feared
        fearState = fearCase.notFeared;

        //
        if (randomizeNavPoints)
            RandomizeNavPoints();

        //Assigns agent's first destination to first array element
        agent.destination = navPoints[currentDest].transform.position;
    }


    void Update()
    {

        //Ensure fear will never go below zero or above fearMax, regardless of the passed _val
        fearCurrent = Mathf.Clamp(fearCurrent, fearReset, fearMax);

        //
        DetermineFearState();

        //Can also add an end state for if the NPC reaches a final objective?
        switch (fearState)
        {

            case fearCase.feared:

                //
                TravelToExit();

                //Reduces fear value by scaleVal each second
                fearCurrent -= scaleVal * Time.deltaTime;

                break;

            case fearCase.notFeared:

                //
                ProceedThroughPoints();
                
                break;

            default:
                break;
        }
    }

    
    //This can be upgraded 
    private void DetermineFearState()
    {

        //
        if (fearCurrent == fearMax && fearReset != fearMax)
        {

            //
            fearState = fearCase.feared;

            //Increases the fear reset point for when the NPC leaves fear
            fearReset += scaleFearReset;

            //
            hasBeenFeared = true;
        }

        //This creates a diminishing return where the NPC eventually becomes unfearable
        else if (fearCurrent == fearReset)
        {

            //
            fearState = fearCase.notFeared;

            //
            hasBeenFeared = false;
        }
    }


    //Moves the agent to each point in the navPoint array starting from the 0th element to the last element
    private void ProceedThroughPoints()
    {

        //Assigns the destination of the agent to the position of the current element in the navPoints array
        agent.destination = navPoints[currentDest].transform.position;

        //Calculates distance between NavAgent and current destination
        float distanceFromDest = Vector3.Distance(transform.position, navPoints[currentDest].transform.position);

        //If distanceFromDest is less than nextPointDist...
        if (distanceFromDest < nextPointDist)
        {

            //Resets the pathfinding to the beginning of the array
            //This will need to be hashed out a bit more...
            if (currentDest == navPoints.Length - 1)
            {
                //Restarts the pathing through the array
                currentDest = 0;

                if (randomizeNavPoints)
                    //Randomizes the navPoints array when the current path has been finished
                    RandomizeNavPoints();
            }

            //Iterate currentDest if the NavAgent is closer to the current destination than nextPointDist
            else
                currentDest++;
        }
    }


    //Randomizes the order of navPoints[]
    private void RandomizeNavPoints()
    {

        //
        for (int i = 0; i < navPoints.Length; i++)
        {

            //Random value between current i and the length of navPoints[]
            int rand = Random.Range(i, navPoints.Length);

            //Swaps current i element and rand element
            GameObject a = navPoints[i];
            navPoints[i] = navPoints[rand];
            navPoints[rand] = a;
        }
    }

    
    //Sets the agent's destination to the exit point transform
    private void TravelToExit()
    {
        //       
        agent.destination = exitPoint.transform.position;
    }


    //
    public void SetFearCurrent(float _val)
    {

        //Does not alter fearCurrent if hasBeenFeared is true
        if(!hasBeenFeared)
            fearCurrent += _val;
    }
}