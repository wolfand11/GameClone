using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {

	private bool hasSpawn;

	private MoveScript 			moveScript;
	private WeaponScript[] 		weapons;
	private Animator			animator;
	private SpriteRenderer[]	renderers;

	public float minAttackCooldown = 0.5f;
	public float maxAttackCooldown = 2f;

	private float 	aiCooldown;
	private bool	isAttacking;
	private Vector2 positionTarget;

	void Awake()
	{
		weapons = GetComponentsInChildren<WeaponScript> ();
		moveScript = GetComponent<MoveScript> ();
		animator = GetComponent<Animator> ();
		renderers = GetComponentsInChildren<SpriteRenderer> ();
	}

	void Start()
	{
		hasSpawn = false;
		collider2D.enabled = false;
		moveScript.enabled = false;
		foreach (WeaponScript weapon in weapons) {
			weapon.enabled = false;		
		}
		isAttacking = false;
		aiCooldown = maxAttackCooldown;
	}

	void Update()
	{
		if (hasSpawn == false) 
		{
			if(renderers[0].IsVisibleFrom(Camera.main))
			{
				Spawn();
			}
		}
		else
		{
			aiCooldown -= Time.deltaTime;
			if(aiCooldown<=0f)
			{
				isAttacking = !isAttacking;
				aiCooldown = Random.Range(minAttackCooldown,maxAttackCooldown);
				positionTarget = Vector2.zero;

				animator.SetBool("Attack",isAttacking);
			}
			if(isAttacking)
			{
				moveScript.direction = Vector2.zero;
				foreach(WeaponScript weapon in weapons)
				{
					if(weapon!=null && weapon.enabled && weapon.CanAttack)
					{
						weapon.Attack(true);
						SoundEffectsHelper.Instance.MakeEnemyShotSound();
					}
				}
			}
			else
			{
				if(positionTarget == Vector2.zero)
				{
					Vector2 randomPoint = new Vector2(Random.Range(0f,1f),Random.Range(0f,1f));
					positionTarget = Camera.main.ViewportToWorldPoint(randomPoint);
				}

				if(collider2D.OverlapPoint(positionTarget))
				{
					positionTarget = Vector2.zero;
				}

				Vector3 direction = ((Vector3)positionTarget - this.transform.position);
				moveScript.direction = Vector3.Normalize(direction);
			}
		}
	}

	private void Spawn()
	{
		hasSpawn = true;
		collider2D.enabled = true;
		moveScript.enabled = true;
		foreach (WeaponScript weapon in weapons) {
			weapon.enabled = true;		
		}

		foreach (ScrollingScript scrolling in FindObjectsOfType<ScrollingScript>()) {
			if(scrolling.isLinkedToCamera)
			{
				scrolling.speed = Vector2.zero;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider2D)
	{
		ShotScript shot = otherCollider2D.gameObject.GetComponent<ShotScript> ();
		if (shot != null) {
			if(shot.isEnemyShot == false)
			{
				aiCooldown = Random.Range(minAttackCooldown,maxAttackCooldown);
				isAttacking = false;

				animator.SetTrigger("Hit");
			}
		}
	}

	void OnDrawGizmos()
	{
		if (hasSpawn && isAttacking == false) {
			Gizmos.DrawSphere(positionTarget, 0.25f);		
		}
	}
}
