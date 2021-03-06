﻿using UnityEngine;
using System.Collections;
using Characters.CustomThirdPerson;

[RequireComponent(typeof(CustomThirdPersonCharacter))]

public class GhostAvatar : MonoBehaviour {
    private SkinnedMeshRenderer Skin;
    private Color oldColor;

    private bool IsInZone;
    private int notInZoneCount;
    private GhostDriver GhostMaster;
    private Inhabitable targetZone;
    private bool jumping;
    private float timeJumping;
    private CustomThirdPersonCharacter controller;
    private Rigidbody rigid;
    private RigidbodyConstraints oldConstraints;
    private Animator anims;

    public bool isJumping
    {
        get
        {
            return jumping;
        } 
    }

    void Start()
    {
        Skin = GetComponentInChildren<SkinnedMeshRenderer>();
        oldColor = Skin.material.color;
        GhostMaster = GameObject.FindObjectOfType<GhostDriver>();
        controller = this.GetComponent<CustomThirdPersonCharacter>();
        rigid = GetComponent<Rigidbody>();
        anims = GetComponent<Animator>();


    }


    void Update()
    {
        if (IsInZone)
        {
            Skin.material.color = new Color(oldColor.r, oldColor.g + 50, oldColor.b);

            //ok, here we go
            if (Input.GetKeyDown(KeyCode.Space) && !jumping)
            {
                GhostMaster.playerInhabitingZone(targetZone);
                controller.Locked = true;
                jumping = true;
                anims.Play("JumpIn");
                anims.SetBool("Possessing",true);
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
            var currAnim = anims.GetCurrentAnimatorStateInfo(0);
            if (currAnim.IsName("Inhabiting"))
            {
                jumping = false;
                timeJumping = 0;
                GhostMaster.FinishedJumping();
            }
        }

        anims.SetFloat("Speed",rigid.velocity.magnitude);

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

    public void isInPlay(bool playing)
    {
        //I don't need the ghost falling forever and creating and overflow, just...float there.
        if (!playing)
        {
            oldConstraints = rigid.constraints;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
        }
        else
        {
            controller.Locked = false;
            rigid.constraints = oldConstraints;
            anims.SetBool("Possessing",false);
        }
    }
}
