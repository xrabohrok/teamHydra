using UnityEngine;
using System.Collections;
using Characters.CustomThirdPerson;

[RequireComponent(typeof(CustomThirdPersonCharacter))]

public class GhostAvatar : MonoBehaviour {
    private MeshRenderer Skin;
    private Color oldColor;

    private bool IsInZone;
    private int notInZoneCount;
    private GhostDriver GhostMaster;
    private Inhabitable targetZone;
    private bool jumping;
    private float timeJumping;
    private CustomThirdPersonCharacter controller;
    // Use this for initialization

    void Start()
    {
        Skin = GetComponent<MeshRenderer>();
        oldColor = Skin.material.color;
        GhostMaster = GameObject.FindObjectOfType<GhostDriver>();
        controller = this.GetComponent<CustomThirdPersonCharacter>();


    }

    // Update is called once per frame

    void Update()
    {
        if (IsInZone)
        {
            Skin.material.color = new Color(oldColor.r + 50, oldColor.g, oldColor.b);

            //ok, here we go
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                GhostMaster.playerInhabitingZone(targetZone);
                controller.Locked = true;
                jumping = true;
                //play swoosh animation here
            }
        }

        if (notInZoneCount > 0)
            notInZoneCount++;
        
        if(notInZoneCount > 4)
        {
            Skin.material.color = oldColor;
            IsInZone = false;
        }

        if (jumping)
        {
            timeJumping += Time.deltaTime;
            if (timeJumping >= GhostMaster.timeToJump)
            {
                jumping = false;
                timeJumping = 0;
                GhostMaster.FinishedJumping();
            }
        }

        

    }

    //if the ghost is close enough to a thing that can be possesed, this signal is sent
    public void IsInPossessionZone(Inhabitable zone)
    {
        IsInZone = true;
        notInZoneCount = 0;
        targetZone = zone;
    }

    public void HasLeftPossesionZone(Inhabitable zone)
    {
        //so, this seems silly, but I basically wait a frame or two to make sure I'm out of all zones
        //I could just poll all the zones, buuuutttt, lets not
        notInZoneCount++;
        targetZone = null;
    }
}
