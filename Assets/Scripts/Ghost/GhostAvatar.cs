using UnityEngine;
using System.Collections;

public class GhostAvatar : MonoBehaviour {
    private MeshRenderer Skin;
    private Color oldColor;

    private bool IsInZone;
    private int notInZoneCount;
    // Use this for initialization

    void Start()
    {
        Skin = GetComponent<MeshRenderer>();
        oldColor = Skin.material.color;


    }

    // Update is called once per frame

    void Update()
    {
        if (IsInZone)
            Skin.material.color = new Color(oldColor.r + 50, oldColor.g, oldColor.b);

        if (notInZoneCount > 0)
            notInZoneCount++;
        
        if(notInZoneCount > 4)
        {
            Skin.material.color = oldColor;
            IsInZone = false;
        }

        

    }

    //if the ghost is close enough to a thing that can be possesed, this signal is sent

    public void IsInPossessionZone(Inhabitable zone)
    {
        IsInZone = true;
        notInZoneCount = 0;
    }

    public void HasLeftPossesionZone(Inhabitable zone)
    {
        //so, this seems silly, but I basically wait a frame or two to make sure I'm out of all zones
        //I could just poll all the zones, buuuutttt, lets not
        notInZoneCount++;
    }
}
