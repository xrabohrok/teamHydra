using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SphereCollider))]
public class Inhabitable : MonoBehaviour {
    public GameObject _respawnDropLocation;

    private SphereCollider cCollider;
    private Vector3 despawnLocation;
    private MeshRenderer thingRendered;
    private Color oldColor;

    private Vector3 lastGoodPlayerSpot;


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
    }

    public void RotateInhabitable(float getAxis)
    {
        //It is ok if this does nothing
    }

    public void ActivateInhabitable()
    {
        throw new System.NotImplementedException();
    }

    public Vector3 JumpOut()
    {
        return _respawnDropLocation.transform.position;
    }
}
