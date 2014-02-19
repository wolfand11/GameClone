using UnityEngine;

public class HealthScript : MonoBehaviour {

	public int hp = 1;
	public bool isEnemy = true;

	public void Damage(int damageCount)
	{
		hp -= damageCount;
		if(hp <= 0)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot!=null)
		{
			if(shot.isEnemyShot!=isEnemy)
			{
				Damage(shot.damage);
				Destroy(shot.gameObject);
			}
		}
	}
}
