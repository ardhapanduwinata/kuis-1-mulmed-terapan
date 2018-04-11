using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
public class Player : MonoBehaviour
{
	private Rigidbody2D myRigidbody;
	private Animator myAnimator;

	[SerializeField]
	public float speed = 10;

	private bool facingRight;
	private bool slide;
	private bool isGrounded;
	private bool jump;

	[SerializeField]
	private Transform[] groundPoints;

	[SerializeField]
	private float groundRadius;

	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private LayerMask whatIsGround;

	void Start()
	{
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}

	void Update()
	{
		HandleInput ();
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleLayers ();
		ResetValues ();
	}
	private void HandleMovement(float horizontal)
    {
		if (myRigidbody.velocity.y < 0) 
		{
			myAnimator.SetBool ("land", true);
		}
		if(!myAnimator.GetBool("slide"))
		{
			myRigidbody.velocity = new Vector2(horizontal*speed,myRigidbody.velocity.y);
		}

		if (isGrounded && jump) 
		{
			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
			myAnimator.SetTrigger ("jump");
		}
			
		if (slide && !this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) 
		{
			myAnimator.SetBool ("slide", true);	
		} 
		else if (!this.myAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Slide")) 
		{
			myAnimator.SetBool ("slide", false);
		}
		myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
    }
	private void Flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) 
		{
			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;

			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
	private void HandleInput()
	{
		if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			slide = true;
		}
		if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			jump = true;
		}
	}
	private void ResetValues()
	{
		slide = false;
		jump = false;
	}
	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0) 
		{
			foreach (Transform point in groundPoints) 
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

				for (int i = 0; i < colliders.Length; i++) 
				{
					if (colliders [i].gameObject != gameObject) 
					{
						myAnimator.ResetTrigger ("jump");
						myAnimator.SetBool ("land", false);
						return true;
					}
				}
			}
		}
		return false;
	}
	private void HandleLayers()
	{
		if (!isGrounded) 
		{
			myAnimator.SetLayerWeight (1, 1);
		} 
		else 
		{
			myAnimator.SetLayerWeight (1, 0);
		}
			
			
	}
}
