using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
    public Transform firePoint;
    public float defaultFirePoint;
    public float crouchFirePoint;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
    public bool roll = false;

    // Update is called once per frame
    void Update () {
        
        if (!crouch)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }
		
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}
        if (Input.GetButtonDown("Roll"))
        {
            if (controller.rollReady)
            {
                roll = true;
                animator.SetBool("IsRolling", true);
            }
        }

	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}

	public void OnCrouching (bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
        if (isCrouching)
        {
            firePoint.transform.localPosition = new Vector3(firePoint.transform.localPosition.x, crouchFirePoint, 0f);
        } else
        {
            firePoint.transform.localPosition = new Vector3(firePoint.transform.localPosition.x, defaultFirePoint, 0f);
        }
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, roll);
		jump = false;
	}

}
