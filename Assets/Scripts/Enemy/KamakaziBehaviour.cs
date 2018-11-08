using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamakaziBehaviour : EnemyBehaviour
{
	public delegate void KamakaziSmashPlayer();
	public static event KamakaziSmashPlayer kamakaziSmashPlayer;

	
	
	private void Start()
	{
		direction = Target - transform.position;
		direction.y = 0f;
        
		Debug.DrawRay(transform.position, direction, Color.green, 4f);
        
		transform.right = direction;
		transform.Rotate(90f, transform.localRotation.y, 0f);

		SpriteRenderer = GetComponent<SpriteRenderer>();
	}


	private void FixedUpdate()
	{
		transform.position += direction.normalized * Speed * Time.fixedDeltaTime;
	}


	private void Update()
	{
		if (Partner != null)
		{
			if (Distance(transform.position, Partner.transform.position))
			{
				ExplosionAndDestruction();
			}
		}

		if (PatrolEnemy.PatrolVehicle != null)
		{
			if (Distance(transform.position, PatrolEnemy.PatrolVehicle.transform.position))
			{
				ExplosionAndDestruction();
			}
		}

		if (PlayerData.PlayerObject != null)
		{
			if (Vector3.Distance(transform.position, PlayerData.PlayerObject.transform.position) <= 200f)
			{
				ExplosionAndDestruction();
			}
		}
	}


	private void ExplosionAndDestruction()
	{
		ExplosionManager.InstantiateExplosion(transform.position);
		Destroy(gameObject);
	}


	private bool Distance(Vector3 _a, Vector3 _b)
	{
		return Vector3.Distance(_a, _b) <= 100f;
	}
}
