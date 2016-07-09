//Benjamin Apprill
//7/9/2016
//Credit:

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Handles fearing NPCs by the player ghost
public class FearMortals : MonoBehaviour {

    
    //List to dynamically store potential NPCs
    private List<GameObject> npcsToFear = new List<GameObject>();

    //Amount that the player ghost can fear an NPC
    [SerializeField]
    float fearAmount;

    
	void Update () {
	
        //If npcToFear is not null...
        if(npcsToFear != null)
        {

            //If fear button is pressed...
            if (Input.GetKey(KeyCode.F))
            {

                //For each npc in the list, increase the NPC's fear by fearAmount per second
                foreach (GameObject npc in npcsToFear)
                {

                    //Takes the collided with npc and adds fear to them
                    npc.GetComponent<NPCController>().SetFearCurrent(fearAmount * Time.deltaTime);
                }
            }
        }
	}

    
    //
    void OnTriggerEnter(Collider col)
    {

        //If the collider tag is NPC...
        if (col.tag == "NPC")
        {

            //Assings npcToFear to the collider's game object
            npcsToFear.Add(col.gameObject);
        }
    }

    
    //
    void OnTriggerExit(Collider col)
    {

        //If the collider tag is NPC...
        if (col.tag == "NPC")
        {

            //Set npcToFear to null
            npcsToFear.Remove(col.gameObject);
        }
    }
}
