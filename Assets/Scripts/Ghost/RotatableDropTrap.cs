using UnityEngine;

class RotatableDropTrap : MonoBehaviour, IInhabitableActions
{
    public float rotationSpeed  = 1.3f;
    public float actionTime;
    private bool acting;
    private float actingTime;
    private Vector3 oldScale;

    public void Update()
    {
        if (acting)
        {
            actingTime += Time.deltaTime;
            if (actingTime >= actionTime)
            {
                this.gameObject.transform.localScale = oldScale;
            }
        }
    }

    public void RotateInhabitable(float axis)
    {
//        this.gameObject.transform.rotation = Quaternion.AngleAxis(axis * rotationSpeed, Vector3.up);
        gameObject.transform.Rotate(Vector3.up, axis * rotationSpeed);
    }

    public void ActivateInhabitable()
    {
        acting = true;
        actingTime = 0;
        oldScale = this.gameObject.transform.localScale;
        this.gameObject.transform.localScale *= 1.2f;

    }
}