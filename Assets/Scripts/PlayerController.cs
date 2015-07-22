using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour
{
	//Player Handling
	public float gravity = 1;
	public float speed = 10;
	public float acceleration = 12;
	public float jumpHeight = 5;

	public float slideAccel = 600;
	public bool playerSlide = false;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private bool alreadyJumped;

	private PlayerPhysics playerPhysics;
	
	// Use this for initialization
	void Start()
	{
		playerPhysics = GetComponent<PlayerPhysics>();
		alreadyJumped = false;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(playerPhysics.stopped)
		{
			targetSpeed = 0;
			currentSpeed = 0;
		}

		playerSlide = false;
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		if(playerPhysics.grounded)
		{
			amountToMove.y = 0;

			if(Input.GetButtonDown("Jump"))
			{
				amountToMove.y = jumpHeight;
				alreadyJumped = true;
			}
		}
		else if(alreadyJumped)
		{
			if(Input.GetButtonDown("Jump"))
			{
				amountToMove.y = jumpHeight;
				alreadyJumped = false;
			}
		}

		if(!playerSlide)
		{
			if(Input.GetButtonDown("SlideLeft"))
			{
				float slideSpeed = 200000;
				float slideAccel = 600;
				float slideDistance = 50000000000;
				
				targetSpeed = -slideSpeed * slideDistance;
				currentSpeed = IncrementTowards(currentSpeed, targetSpeed, slideAccel);

				playerSlide = true;
			}
			if(Input.GetButtonDown("SlideRight"))
			{
				float slideSpeed = 200000;
				float slideDistance = 50000000000;
				
				targetSpeed = slideSpeed * slideDistance;
				currentSpeed = IncrementTowards(currentSpeed, targetSpeed, slideAccel);

				playerSlide = true;
			}
		}

		if(playerPhysics.stopped && Input.GetButton("Grab"))
		{
			amountToMove.x = currentSpeed;
			amountToMove.y = -playerPhysics.transform.position.y;
			amountToMove.y -= gravity * Time.deltaTime;
			alreadyJumped = true;
		}
		else
		{
			amountToMove.x = currentSpeed;
			amountToMove.y -= gravity * Time.deltaTime;
		}

		playerPhysics.Move(amountToMove * Time.deltaTime);
	}
	
	private float IncrementTowards(float n, float target, float acceleration)
	{
		if(n == target)
		{
			return n;
		}
		else
		{
			float dir = Mathf.Sign(target - n);
			n += acceleration * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target - n)) ? n : target;
		}
	}
}
