using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
	[SerializeField] private GameObject ExplosionPrefab;
	private static GameObject StaticExplosionPrefab;


	private void Start()
	{
		StaticExplosionPrefab = ExplosionPrefab;
	}

	
	public static void InstantiateExplosion(Vector3 _pos)
	{
		var ex = Instantiate(StaticExplosionPrefab, _pos, StaticExplosionPrefab.transform.rotation);
		// Sound
	}
	

}
