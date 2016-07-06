using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SphereCollider))]
public class Inhabitable : MonoBehaviour {
    public GameObject _respawnDropLocation;

    private SphereCollider cCollider;
    private bool usingDropLocation;
    private Vector3 despawnLocation;
    private MeshRenderer thingRendered;
    private Color oldColor;


    // Use this for initialization
	void Start ()
	{
	    if (_respawnDropLocation != null)
	    {
	        usingDropLocation = true;
	    }

	    cCollider = this.gameObject.GetComponent<SphereCollider>();
	    cCollider.isTrigger = true;

	    thingRendered = this.gameObject.GetComponent<MeshRenderer>();
	    oldColor = thingRendered.material.color;
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
    void OnDrawGizmosSelected()
    {
        if(_respawnDropLocation != null)
            Gizmos.DrawLine(this.transform.position, _respawnDropLocation.transform.position);
            
    }

    public void ShouldLookInhabited(bool isInhabited)
    {
        if (isInhabited)
        {
            thingRendered.material.color = new Color(oldColor.r, oldColor.g, oldColor.b + 30);
        }
        else
        {
            thingRendered.material.color = oldColor;
        }
    }
}
