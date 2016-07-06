using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SphereCollider))]
public class Inhabitable : MonoBehaviour {
    public GameObject _respawnDropLocation;

    private SphereCollider cCollider;
    private bool usingDropLocation;
    private Vector3 despawnLocation;


    // Use this for initialization
	void Start ()
	{
	    if (_respawnDropLocation != null)
	    {
	        usingDropLocation = true;
	    }

	    cCollider = this.gameObject.GetComponent<SphereCollider>();
	    cCollider.isTrigger = true;
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
}
