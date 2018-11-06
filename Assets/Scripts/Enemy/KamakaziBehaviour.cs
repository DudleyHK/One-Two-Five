using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamakaziBehaviour : EnemyBehaviour
{
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


	private void OnCollisionEnter(Collision _other)
	{
		var n = _other.collider.name;
		Debug.Log("OTHER - " + n);

		if (n.Contains(ObjectNames.Bud)) return;

		if (n.Contains(ObjectNames.Kamakazi))
		{
			//if (Main)
				ExplosionManager.InstantiateExplosion(transform.position);
			Destroy(gameObject);
		}
		else
		{
			ExplosionManager.InstantiateExplosion(transform.position);
			Destroy(gameObject);
		}
		
	}
}
