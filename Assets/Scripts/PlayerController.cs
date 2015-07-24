using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour
{
	//Player Handling
	public float gravity = 30;
	public float speed = 10;
	public float acceleration = 30;
	public float jumpHeight = 12;

	private bool canWallGrab = false;
	private bool canWallSlide = false;
	private bool canDoubleJump = false;
	private bool canHoverOnJump = false;
	private bool canBoostOnJump = false;

	private bool isHovering = false;
	private bool isBoosting = false;

	private float hoverMaxTime = 2;
	private float boostMaxTime = 2;

	private float hoverTime = 0;
	private float boostTime = 0;

	private string activeMode;
	private string[] modes;
	private MovementModShifter movScript;

	public float slideAccel = 0;
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

		movScript = (MovementModShifter)FindObjectOfType(typeof(MovementModShifter));
		activeMode = movScript.GetActiveComponentName();
		modes = movScript.GetAvailableComponentsNames();
	}
	
	// Update is called once per frame
	void Update()
	{
		Debug.Log("g  -"+gravity+"\n"+
		          "v  -"+speed+"\n"+
		          "a  -"+acceleration+"\n"+
		          "jh -"+jumpHeight+"\n"+
		          "cwg-"+canWallGrab+"\n"+
		          "cws-"+canWallSlide+"\n"+
		          "cdj-"+canDoubleJump+"\n"+
		          "chj-"+canHoverOnJump+"\n"+
		          "cbj-"+canBoostOnJump+"\n"+
		          "iH -"+isHovering+"\n"+
		          "iB -"+isBoosting+"\n"+
		          "hMt-"+hoverMaxTime+"\n"+
		          "bMt-"+boostMaxTime+"\n"+
		          "hT -"+hoverTime+"\n"+
		          "bT -"+boostTime+"\n"+
		          "aM -"+activeMode+"\n"+
		          "sA -"+slideAccel+"\n"+
		          "pS -"+playerSlide+"\n"+
		          "cV -"+currentSpeed+"\n"+
		          "tV -"+targetSpeed+"\n"+
		          "aTM-"+amountToMove+"\n"+
		          "aJ -"+alreadyJumped+"\n"
		          );

		if(Input.GetButtonDown("Cycle"))
		{
			LoadValues();
		}

		if(playerPhysics.stopped)
		{
			targetSpeed = 0;
			currentSpeed = 0;
		}

		if(hoverTime >= hoverMaxTime)
		{
			LoadValues();
			isHovering = false;
		}

		if(boostTime >= boostMaxTime)
		{
			LoadValues();
			isBoosting = false;
		}

		if(isHovering && Input.GetButton("Jump"))
		{
			speed = 5;
			acceleration = 10;
			amountToMove.y = 0 + gravity * Time.deltaTime;
			hoverTime += Time.deltaTime;
		}

		if(isBoosting && Input.GetButton("Jump"))
		{
			speed = 2;
			acceleration = 2;
			amountToMove.y = 1.5f + gravity * Time.deltaTime;
			boostTime += Time.deltaTime;
		}

		playerSlide = false;
		targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		if(playerPhysics.grounded)
		{
			amountToMove.y = 0;
			isHovering = false;
			isBoosting = false;
			LoadValues();

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
				if(canDoubleJump)
				{
					amountToMove.y = jumpHeight;
					alreadyJumped = false;
				}

				if(canHoverOnJump)
				{
					amountToMove.y = 0;
					alreadyJumped = false;
					isHovering = true;
					hoverTime = 0;
				}

				if(canBoostOnJump)
				{
					amountToMove.y = 0;
					alreadyJumped = false;
					isBoosting = true;
					boostTime = 0;
				}
			}
		}

		if(!playerSlide)
		{
			if(Input.GetButtonDown("SlideLeft"))
			{
				float slideSpeed = 200000;
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


		if(playerPhysics.stopped && Input.GetButton("Grab") && canWallSlide)
		{
			amountToMove.x = currentSpeed;
			amountToMove.y = -playerPhysics.transform.position.y;
			amountToMove.y -= gravity * Time.deltaTime;
			alreadyJumped = true;
		}
		else if(playerPhysics.stopped && Input.GetButton("Grab") && canWallGrab)
		{
			amountToMove.x = currentSpeed;
			amountToMove.y = 0;
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

	private void LoadValues()
	{
		activeMode = movScript.GetActiveComponentName();

		switch(activeMode)
		{
			case "RB1":
			{
				speed = 10*2;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "RB2":
			{
				speed = 10*3;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "SB1":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12*2;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "SB2":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12*3;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "DB1":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 600;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "DB2":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 600*2;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "WGB":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = true;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "WSB":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = true;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "JPA":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = true;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}

			case "JPH":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = true;
				canBoostOnJump = false;
				break;
			}

			case "JPB":
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = true;
				break;
			}

			default:
			{
				speed = 10;
				acceleration = 30;
				jumpHeight = 12;
				slideAccel = 0;
				canWallGrab = false;
				canWallSlide = false;
				canDoubleJump = false;
				canHoverOnJump = false;
				canBoostOnJump = false;
				break;
			}
		}

	}
}
