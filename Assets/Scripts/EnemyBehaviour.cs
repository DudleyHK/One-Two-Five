using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public float Speed;
	
	
	[SerializeField] private GameObject player;
	
	
	
	// Use this for initialization
	void Start () {
		
		player = GameObject.FindGameObjectWithTag("Player");

		var pixelPos = Camera.main.WorldToScreenPoint(transform.localPosition);
		var offset = new Vector2(player.transform.localPosition.x - pixelPos.x, player.transform.localPosition.y - pixelPos.y);
		var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		
		transform.rotation = Quaternion.Euler(90f, 0f, angle);
		
	}
	
	// Update is called once per frame
	void Update ()
	{

		transform.position += transform.right * (Speed * 5f) * Time.deltaTime;

	}
}
