//Benjamin Apprill
//7/7/2016
//Credit:  http://docs.unity3d.com/Manual/nav-CreateNavMeshAgent.html

using UnityEngine;
using System.Collections;

//Determines pathing for NPC.  Takes inspector transforms into an array and randomizes them for the NavAgent to travel through.
//Also re-randomizes the path once the current one is completed.
public class NPCRoute : MonoBehaviour {

    
    //The integer value for the current destination element in the navPoints array
    private int currentDest;

    //Holds the NavMeshAgent
    private NavMeshAgent agent;

    //Check in inspector to randomize the navPoints array
    [SerializeField]
    bool randomizeNavPoints;
    
    //Distance from a point required to iterate to the next element of the navPoints array
    [SerializeField]
    float nextPointDist;

    //Need points for the NavAgent to travel to
    [SerializeField]
    Transform[] navPoints;

       
	void Start () {

        //Assigns NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        //Initial destination is first element of the array
        currentDest = 0;

        //
        if(randomizeNavPoints)
            RandomizeNavPoints();

        //Assigns agent's first destination to first array element
        agent.destination = navPoints[currentDest].position;
	}
	

	void Update () {

        //
        ProceedThroughPoints();       
	}


    //Moves the agent to each point in the navPoint array starting from the 0th element to the last element
    private void ProceedThroughPoints()
    {
        
        //Calculates distance between NavAgent and current destination
        float distanceFromDest = Vector3.Distance(transform.position, navPoints[currentDest].position);

        //If distanceFromDest is less than nextPointDist...
        if (distanceFromDest < nextPointDist)
        {

            Debug.Log(currentDest);

            //Resets the pathfinding to the beginning of the array
            //This will need to be hashed out a bit more...
            if (currentDest == navPoints.Length - 1)
            {
                //Restarts the pathing through the array
                currentDest = 0;
                
                if(randomizeNavPoints)
                    //Randomizes the navPoints array when the current path has been finished
                    RandomizeNavPoints();
            }

            //Iterate currentDest if the NavAgent is closer to the current destination than nextPointDist
            else
                currentDest++;
            
            //Assigns the destination of the agent to the position of the current element in the navPoints array
            agent.destination = navPoints[currentDest].position;
        }
    }


    //Randomizes the order of navPoints[]
    private void RandomizeNavPoints()
    {

        //
        for(int i = 0; i < navPoints.Length; i++)
        {

            //Random value between current i and the length of navPoints[]
            int rand = Random.Range(i, navPoints.Length);
            
            //Swaps current i element and rand element
            Transform a = navPoints[i];
            navPoints[i] = navPoints[rand];
            navPoints[rand] = a;
        }
    }
}