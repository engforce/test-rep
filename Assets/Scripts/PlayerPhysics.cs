using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
	public LayerMask collisionMask;

	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public bool stopped;

	private BoxCollider collider;
	private Vector3 size;
	private Vector3 center;
	private float skin = 0.005f;

	Ray ray;
	RaycastHit hit;

	void Start()
	{
		collider = GetComponent<BoxCollider>();
		size = collider.size;
		center = collider.center;
	}

	public void Move(Vector2 moveAmount)
	{
		float deltaX = moveAmount.x;
		float deltaY = moveAmount.y;
		Vector2 position = transform.position;



		//Detect Y axis collisions
		grounded = false;

		for(int i = 0; i < 3; i++)
		{
			float dir = Mathf.Sign(deltaY);
			float x = (position.x + center.x - size.x/2) + size.x/2 * i;
			float y = position.y + center.y + size.y/2 * dir;

			ray = new Ray(new Vector2(x, y), new Vector2(0, dir));

			Debug.DrawRay(ray.origin, ray.direction);

			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + skin, collisionMask))
			{
				float distance = Vector3.Distance(ray.origin, hit.point);

				if(distance > skin)
				{
					//deltaY = skin - distance;
					deltaY = distance * dir - skin * dir;
				}
				else
				{
					deltaY = 0;
				}
				grounded = true;
				break;
			}
		}


		//Detect X axis collisions
		stopped = false;

		for(int i = 0; i < 3; i++)
		{
			float dir = Mathf.Sign(deltaX);
			float x = position.x + center.x + size.x/2 * dir;
			float y = (position.y + center.y - size.y/2) + size.y/2 * i;

			ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));

			Debug.DrawRay(ray.origin, ray.direction);

			if(Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask))
			{
				float distance = Vector3.Distance(ray.origin, hit.point);
				
				if(distance > skin)
				{
					deltaX = distance * dir - skin * dir;
				}
				else
				{
					deltaX = 0;
				}

				stopped = true;
				break;
			}
		}



		Vector2 finalTransform = new Vector2(deltaX, deltaY);

		transform.Translate(finalTransform);
	}
}
