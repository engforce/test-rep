using UnityEngine;
using System.Collections;

public class MoveBulletTrail : MonoBehaviour
{	
	public int moveSpeed;
	public LayerMask hitTargets;

	private BoxCollider collider;
	private Vector3 startingPosition;
	private Vector3 size;
	private Vector3 center;

	void Start()
	{
		collider = GetComponent<BoxCollider>();
		startingPosition = new Vector3();
		size = collider.size;
		center = collider.center;
	}

	// Update is called once per frame
	void Update ()
	{
		startingPosition.x = transform.position.x + center.x - size.x/2;
		startingPosition.y = transform.position.y;
		startingPosition.z = transform.position.z;

		Ray ray = new Ray(startingPosition, Vector3.right);

		Debug.DrawRay(ray.origin, ray.direction, Color.red);
		//Debug.DrawLine(ray.origin, ray.direction, Color.red);
		
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, size.x, hitTargets))
		{
			Debug.Log("Trail Edge hit");
			Destroy(this.gameObject);
		}
		else
		{
			Debug.Log("Trail Edge missed");
			transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
		}


	}
}
