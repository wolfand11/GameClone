using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	private bool hasSpawn;
	private MoveScript moveScript;
	private WeaponScript[] weapons;

	void Awake()
	{
		weapons = GetComponentsInChildren<WeaponScript> ();
		moveScript = GetComponent<MoveScript> ();
	}

	void Start()
	{
		hasSpawn = false;
		collider2D.enabled = false;
		moveScript.enabled = false;
		foreach (WeaponScript weapon in weapons) {
			weapon.enabled = false;		
		}
	}

	// Update is called once per frame
	void Update () 
	{

		if (hasSpawn == false) 
		{
			if (renderer.IsVisibleFrom (Camera.main)) 
			{
				Spawn ();
			}
		} 
		else 
		{
			foreach (WeaponScript weapon in weapons) 
			{
				if (weapon != null && weapon.CanAttack) 
				{
					weapon.Attack (true);
					SoundEffectsHelper.Instance.MakeEnemyShotSound();
				}
			}

			if (renderer.IsVisibleFrom (Camera.main) == false) 
			{
				Destroy(gameObject);	
			}
		}
	}

	void Spawn()
	{
		hasSpawn = true;
		collider2D.enabled = true;
		moveScript.enabled = true;
		foreach (WeaponScript weapon in weapons) {
			weapon.enabled = true;		
		}
	}
}
