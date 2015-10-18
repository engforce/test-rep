using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public float fireRate = 0;
	public float damage = 0;
	public LayerMask hitTargets;

	public Transform bulletTrailPrefab;


	private float timeToFire = 0;
	private Transform firePoint;
	private float direction;
	private WeaponDirectionHandler weaponDirectionHandlerScript;

	// Use this for initialization
	void Awake ()
	{
		firePoint = transform.FindChild("FirePoint");

		weaponDirectionHandlerScript = (WeaponDirectionHandler) FindObjectOfType(typeof(WeaponDirectionHandler));
		direction = weaponDirectionHandlerScript.GetLastDirection();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(fireRate == 0)
		{
			if(Input.GetButtonDown("Fire3"))
			{
				Shoot();
			}
		}
		else
		{
			if(Input.GetButton("Fire3") && Time.time > timeToFire)
			{
				timeToFire = Time.time + 1/timeToFire;
				Shoot();
			}
		}
	}

	private void Shoot()
	{
//		direction = weaponDirectionHandlerScript.GetLastDirection();
//		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
//		Vector2 dirVector = new Vector2(direction, 0);
//		RaycastHit2D hit = Physics2D.Raycast(firePointPosition, dirVector, 30, hitTargets);
//		Debug.DrawLine(firePointPosition, dirVector);

		direction = weaponDirectionHandlerScript.GetLastDirection();

		Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
		Vector2 dirVector = new Vector2(direction, 0);

		Ray ray = new Ray(firePointPosition, dirVector);

//		Debug.DrawRay(ray.origin, ray.direction);
		Debug.DrawLine(ray.origin, ray.origin + new Vector3(30*direction, 0, 0));

		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 30, hitTargets))
		{
			Effect();
			Debug.Log("Hit confirmed");
		}
		else
		{
			Effect();
			Debug.Log("Shot missed");
		}



	}


	private void Effect()
	{
		Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
	}
}
