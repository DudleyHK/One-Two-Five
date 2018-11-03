using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField] private Animator anim;
	[SerializeField] private float speed = 1f;

	
	
	private void Start()
	{
		anim.speed = speed;
	}

	
	private void DestroyOnComplete()
	{
		Destroy(gameObject);
	}
}
