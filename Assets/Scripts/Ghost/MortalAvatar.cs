using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class MortalAvatar: MonoBehaviour {
    private Animator anims;
    private NavMeshAgent Nav;

    public float topSpeed;
    private Rigidbody rigid;
    private bool running;
    private Vector3 lastPos;

    void Start()
    {
        anims = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        if (Mathf.Abs(topSpeed) < .01f)
        {
            topSpeed = .05f;
        }

        //start at a walking speed.
        IsRunning(false);
        lastPos = this.transform.position;
    }


    void Update()
    {
        var currVel = (this.transform.position - lastPos)/Time.deltaTime;
        var currentNormSpeed = currVel.magnitude /topSpeed;

        anims.SetFloat("Speed", currentNormSpeed);
//        Debug.Log(string.Format("speed: {0} , normSpeed: {1}", currVel.magnitude, currentNormSpeed));

        anims.SetBool("Moving", Mathf.Abs(currentNormSpeed) > .01f);

        lastPos = this.transform.position;

    }

    public void IsRunning(bool runState)
    {
        running = runState;

        if (running)
        {
            Nav.speed = topSpeed;
        }
        else
        {
            Nav.speed = topSpeed/2 - topSpeed * .1f;
        }
    }
}
