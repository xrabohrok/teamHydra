using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class GhostDriver : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject playerInstance;
    public float timeToJump = 1.2f;

    private MeshRenderer Skin;
    private Color oldColor;

    // Use this for initialization
	void Start () {


	}

    // Update is called once per frame
    private void Update()
    {
    }

    public void playerInhabitingZone(Inhabitable targetZone)
    {
        throw new System.NotImplementedException();
    }

    public void FinishedJumping()
    {
        throw new System.NotImplementedException();
    }
}
