using UnityEngine;
using System.Collections;

public class WeaponDirectionHandler : MonoBehaviour
{
	private Transform weapon;
	private float lastDirection;

	// Use this for initialization
	void Start ()
	{
		weapon = transform.FindChild("Weapon");
		lastDirection = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float direction;

		float rawAxis = Input.GetAxisRaw("Horizontal");

		if(rawAxis != 0)
		{
			direction = Mathf.Sign(rawAxis);
			if(lastDirection != direction)
			{
				if(direction == -1)
				{	
					Vector3 center = new Vector3();
					center.x = transform.position.x;
					center.y = transform.position.y;
					center.z = transform.position.z;
					weapon.RotateAround(center, new Vector3(0, 0, 1), -180);
				}

				if(direction == 1)
				{
					Vector3 center = new Vector3();
					center.x = transform.position.x;
					center.y = transform.position.y;
					center.z = transform.position.z;
					weapon.RotateAround(center, new Vector3(0, 0, 1), 180);
				}
				lastDirection = direction;
			}
		}
	}

	public float GetLastDirection()
	{
		return lastDirection;
	}
}
