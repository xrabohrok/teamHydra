using UnityEngine;

namespace Characters.CustomThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class CustomThirdPersonCharacter : MonoBehaviour
	{
	    [SerializeField] float m_GroundCheckDistance = 0.1f;
		[SerializeField] float m_BaseGroundSpeed = 0.1f;

		Rigidbody m_Rigidbody;
		bool m_IsGrounded;
	    Vector3 m_GroundNormal;
	    bool m_Crouching;
	    private Vector3 m_Heading;


	    void Start()
		{
			m_Rigidbody = GetComponent<Rigidbody>();
			GetComponent<CapsuleCollider>();

	        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

        public void FixedUpdate()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Heading * m_BaseGroundSpeed ) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }


	    public void Move(Vector3 move)
	    {

	        // convert the world relative moveInput vector into a local-relative
	        // turn amount and forward amount required to head in the desired
	        // direction.
//	        Debug.Log(string.Format("move is {0}, {1}, {2}", move.x, move.y, move.z));

	        if (move.magnitude > 1f) move.Normalize();
	        move = transform.InverseTransformDirection(move);
	        CheckGroundStatus();
	        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
	        Mathf.Atan2(move.x, move.z);


	          m_Heading = Vector3.zero;
	        
//            Debug.Log(string.Format("move is {0}, {1}, {2}", move.x, move.y, move.z));

	        // control and velocity handling is different when grounded and airborne:
	    }


	    void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
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
