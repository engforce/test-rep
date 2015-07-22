using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{
	private Transform target;
	public float trackSpeed = 10;
	public float baseCameraYAxis = 3;

	private float slideAccel;
	private bool playerSlide;

	public void SetTarget(Transform transform)
	{
		target = transform;
		PlayerController other = (PlayerController)FindObjectOfType(typeof(PlayerController));

		slideAccel = other.slideAccel;
		playerSlide = other.playerSlide;
	}

	void LateUpdate()
	{
		if(target != null)
		{
			if(playerSlide)
			{
				float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
				float y = IncrementTowards(transform.position.y, target.position.y + baseCameraYAxis, trackSpeed);
				transform.position = new Vector3(x, y, transform.position.z);
			}
			else
			{
				float x = IncrementTowards(transform.position.x, target.position.x, slideAccel);
				float y = IncrementTowards(transform.position.y, target.position.y + baseCameraYAxis, slideAccel);
				transform.position = new Vector3(x, y, transform.position.z);
			}
		}
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
