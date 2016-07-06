using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class GhostDriver : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject playerInstance;
    private GhostAvatar playerAvatar;
    public float timeToJump = 1.2f;

    private MeshRenderer Skin;
    private Color oldColor;

    private Inhabitable currentInhabitable;

    void Start () {
	    if (playerInstance == null)
	    {
            playerAvatar = GameObject.FindObjectOfType<GhostAvatar>();
	        playerInstance = playerAvatar.gameObject;
	    }

        currentInhabitable = null;
    }

    private void Update()
    {
        if (currentInhabitable != null)
        {
            
        }
    }

    //Lets the controller know that the player is jumping to an inhabitable
    public void playerInhabitingZone(Inhabitable targetZone)
    {
        currentInhabitable = targetZone;
        currentInhabitable.ShouldLookInhabited(true);
    }

    //a formality to hide the ghost once it is in the thing, and give control to the shell
    public void FinishedJumping()
    {
        playerInstance.SetActive(false);
    }
}
