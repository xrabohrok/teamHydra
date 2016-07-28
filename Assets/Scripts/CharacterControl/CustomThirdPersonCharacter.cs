using UnityEngine;

namespace Characters.CustomThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class CustomThirdPersonCharacter : MonoBehaviour
	{
	    [SerializeField] float m_GroundCheckDistance = 0.1f;
	    [SerializeField] float m_BaseGroundSpeed = 0.1f;
	    public float verticalCenterChange = 0.1f;
	    public bool Locked { get; set; }
	    public float turnRate = 5;

	    Rigidbody m_Rigidbody;
	    bool m_IsGrounded;
	    Vector3 m_GroundNormal;
	    bool m_Crouching;
	    private Vector3 m_Heading;
	    private Vector3 m_scaledHeading;

	    public bool isGrounded { get { return m_IsGrounded;} }


	    void Start()
		{
			m_Rigidbody = GetComponent<Rigidbody>();

	        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

        public void FixedUpdate()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (m_IsGrounded && Time.deltaTime > 0 && !Locked)
            {
                var angle = Mathf.Atan2(m_Heading.x, m_Heading.z)*Mathf.Rad2Deg;
                var moveThisFrame = Quaternion.Euler(new Vector3(0,angle * turnRate * Time.deltaTime,0));
                var finalRoation = m_Rigidbody.rotation * moveThisFrame;
                m_Rigidbody.MoveRotation(finalRoation);

                var localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
                localVelocity.z = m_scaledHeading.magnitude * m_BaseGroundSpeed;
                localVelocity.x = 0;
                localVelocity.y = 0;
                m_Rigidbody.velocity = transform.TransformDirection(localVelocity);
//                m_Rigidbody.AddForce(transform.TransformDirection(localVelocity));

            }

//            if (m_Rigidbody.velocity.magnitude > 1 && m_IsGrounded && m_Rigidbody.velocity.z < .5f)
//            {
//                //                m_Rigidbody.rotation = Quaternion.LookRotation(m_Rigidbody.velocity, Vector3.up);

//            }
        }


	    public void Move(Vector3 move)
	    {

	        // convert the world relative moveInput vector into a local-relative
	        // turn amount and forward amount required to head in the desired
	        // direction.
//	        Debug.Log(string.Format("move is {0}, {1}, {2}", move.x, move.y, move.z));
	        m_scaledHeading = new Vector3(move.x, move.y, move.z);
	        if (move.magnitude > 1f) move.Normalize();
	        move = transform.InverseTransformDirection(move);
	        CheckGroundStatus();
	        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
	        m_Heading = move;


//            Debug.Log(string.Format("move is {0}, {1}, {2}", move.x, move.y, move.z));

	        // control and velocity handling is different when grounded and airborne:
	    }


	    void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
	        
	        Debug.DrawLine(transform.position + (Vector3.up * verticalCenterChange), transform.position + (Vector3.up * verticalCenterChange) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
	        int filter = 1 << LayerMask.NameToLayer("Ground");
            var raycastResult = Physics.Raycast(transform.position + (Vector3.up * verticalCenterChange), Vector3.down, out hitInfo, m_GroundCheckDistance, filter);
            if (raycastResult)
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
			}
//            Debug.Log(string.Format("Grounded is {0}", m_IsGrounded));
		}
	}
}
