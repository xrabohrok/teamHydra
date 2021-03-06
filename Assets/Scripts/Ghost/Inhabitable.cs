﻿using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SphereCollider))]
public class Inhabitable : MonoBehaviour
{
    public GameObject _respawnDropLocation;

    private SphereCollider cCollider;
    private Vector3 despawnLocation;
    private MeshRenderer thingRendered;
    private Color oldColor;

    private IInhabitableActions actionSet;
    private Vector3 lastGoodPlayerSpot;
    private bool thisIsInhabited;


    // Use this for initialization

    void Start ()
	{
	    if (_respawnDropLocation != null)
	    {
	    }

	    cCollider = this.gameObject.GetComponent<SphereCollider>();
	    cCollider.isTrigger = true;

	    thingRendered = this.gameObject.GetComponent<MeshRenderer>();
	    oldColor = thingRendered.material.color;

	    actionSet = gameObject.GetComponent<IInhabitableActions>();
	}

    // Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<GhostAvatar>().IsInPossessionZone(this);
            lastGoodPlayerSpot = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<GhostAvatar>().HasLeftPossesionZone(this);
        }
    }

    //Editor stuff
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if(_respawnDropLocation != null)
            Gizmos.DrawLine(this.transform.position, _respawnDropLocation.transform.position);
    }
#endif

    public void IsBeingInhabited(bool isInhabited)
    {
        if (isInhabited)
        {
            thingRendered.material.color = new Color(oldColor.r, oldColor.g, oldColor.b + 30);
            //if a spot isn't declared, go back to the ghost spot
            if (_respawnDropLocation == null)
            {
                _respawnDropLocation = new GameObject(string.Format("Temp Drop for {0}", this.gameObject.name));
                _respawnDropLocation.transform.position = lastGoodPlayerSpot;
            }
        }
        else
        {
            thingRendered.material.color = oldColor;
        }
        thisIsInhabited = isInhabited;

    }

    public void InvokeRotateInhabitable(float axis)
    {
        if(actionSet!=null && thisIsInhabited)
        actionSet.RotateInhabitable(axis);
    }

    public void InvokeActivateInhabitable()
    {
        if(actionSet!=null && thisIsInhabited)
        actionSet.ActivateInhabitable();
    }

    public Vector3 JumpOut()
    {
        IsBeingInhabited(false);
        return _respawnDropLocation.transform.position;
    }
}
