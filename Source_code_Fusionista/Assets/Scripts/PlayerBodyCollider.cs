using UnityEngine;
using System.Collections;

public class PlayerBodyCollider : MonoBehaviour {

	PlayerController playerCtrl;
	private Rigidbody2D rb2D;
	GameObject player;
	bool isSlime;
	public float waterForce = 50.0f;
	AudioSource myAudio;
	public AudioClip Clip;

	void Awake () {
		playerCtrl = transform.parent.GetComponent<PlayerController> ();
		rb2D = gameObject.GetComponentInParent<Rigidbody2D>();
		player = transform.parent.gameObject;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		
        // เช็คการชนกันในส่วนต่างๆ 

		if (other.tag == "EnemyArm") {
			EnemyController enemyCtrl  = other.GetComponentInParent<EnemyController>();
		
			if (enemyCtrl.attackEnabled) {
				enemyCtrl.attackEnabled = false;
				playerCtrl.dir = (playerCtrl.transform.position.x < enemyCtrl.transform.position.x) ? +1 : -1; 
				playerCtrl.AddForceAnimatorVx(-enemyCtrl.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy( enemyCtrl.attackNockBackVector.y);
				playerCtrl.ActionDamage (enemyCtrl.attackDamage);
				playsound();
			}
		} else
		if (other.tag == "EnemyArmBullet") {
			FireBullet fireBullet = other.transform.GetComponent<FireBullet>();
			if (fireBullet.attackEnabled) {
				fireBullet.attackEnabled = false;
				playerCtrl.dir = (playerCtrl.transform.position.x < fireBullet.transform.position.x) ? +1 : -1; 
				playerCtrl.AddForceAnimatorVx(-fireBullet.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy( fireBullet.attackNockBackVector.y);
				playerCtrl.ActionDamage (fireBullet.attackDamage);
				playsound();
				Destroy (other.gameObject);
			}
		}

		// add trap code
		else
		if (other.tag == "TrapArm")
		{
			TrapController trapCtrl = other.GetComponentInParent<TrapController>();

			if (trapCtrl.attackEnabled)
			{
				trapCtrl.attackEnabled = false;
				playerCtrl.dir = (playerCtrl.transform.position.x < trapCtrl.transform.position.x) ? +1 : -1;
				playerCtrl.AddForceAnimatorVx(-trapCtrl.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy(trapCtrl.attackNockBackVector.y);
				playerCtrl.ActionDamage(trapCtrl.attackDamage);
				playsound();
			}
		}


	}

	void OnTriggerStay2D(Collider2D other)
	{

		if (other.tag == "TrapLava")
		{
			TrapController trapCtrl = other.GetComponentInParent<TrapController>();
			if (trapCtrl.attackEnabled)
			{
				trapCtrl.attackEnabled = false;
				playerCtrl.dir = (playerCtrl.transform.position.x < trapCtrl.transform.position.x) ? +1 : -1;
				playerCtrl.AddForceAnimatorVx(-trapCtrl.attackNockBackVector.x);
				playerCtrl.AddForceAnimatorVy(trapCtrl.attackNockBackVector.y);
				playerCtrl.ActionDamage(trapCtrl.attackDamage);
				playsound();
			}
			
		}

		if (other.tag == "WaterCollider")
		{
			isSlime = player.GetComponent<PlayerMain>().slime;
			if (isSlime)
			{
				rb2D.AddForce(transform.up * waterForce);
			}

		}
	}

	public void playsound()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = Clip;
		audio.Play();
	}
}
