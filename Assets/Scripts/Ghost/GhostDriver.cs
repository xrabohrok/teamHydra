using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class GhostDriver : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject playerInstance;
    private GhostAvatar playerAvatar;

    public GameObject playerHidingSpot;
    public float timeToJump = 1.2f;

    private MeshRenderer Skin;
    private Color oldColor;

    private bool jumpOut;
    private bool jumpIn;
    

    private Inhabitable currentInhabitable;
    private float jumpOutTime;
    private Vector3 jumpZone;

    void Start () {
        if (playerInstance == null)
        {
            playerAvatar = GameObject.FindObjectOfType<GhostAvatar>();
            playerInstance = playerAvatar.gameObject;
        }
        else
        {
            playerAvatar = playerInstance.GetComponent<GhostAvatar>();
        }

        currentInhabitable = null;
    }

    private void Update()
    {
        if (currentInhabitable != null)
        {
            //you are in something.  Now what.
            //rotate thing
            currentInhabitable.RotateInhabitable(Input.GetAxis("Horizontal")); 

            //do action (activate trap?)
            if(Input.GetKeyUp(KeyCode.E))
            {
                currentInhabitable.ActivateInhabitable();
            }

            //Annnndd most importantly, jump out
            if (Input.GetKeyUp(KeyCode.Space) && !playerAvatar.isJumping)
            {
                jumpZone = currentInhabitable.JumpOut();
                jumpOut = true;
            }

            if (jumpOut)
            {
                jumpOutTime += Time.deltaTime;

                if (jumpOutTime > timeToJump)
                {
                    playerInstance.gameObject.transform.position = jumpZone;
                    playerAvatar.isInPlay(true);
                    jumpOutTime = 0;
                    jumpOut = false;
                }
            }
        }
    }

    //Lets the controller know that the player is jumping to an inhabitable
    public void playerInhabitingZone(Inhabitable targetZone)
    {
        currentInhabitable = targetZone;
        currentInhabitable.IsBeingInhabited(true);
    }

    //a formality to hide the ghost once it is in the thing, and give control to the shell
    public void FinishedJumping()
    {
        playerAvatar.isInPlay(false);
        playerInstance.transform.position = playerHidingSpot.transform.position;
    }
}
