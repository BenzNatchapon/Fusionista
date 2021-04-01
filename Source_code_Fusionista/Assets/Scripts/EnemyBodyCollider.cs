using UnityEngine;
using System.Collections;

public class EnemyBodyCollider : MonoBehaviour {

	EnemyController 	enemyCtrl;
	Animator 			playerAnim;
	int 				attackHash = 0;

	public AudioClip Clip;
	bool sound = true;


	void Awake () {
		enemyCtrl 	= GetComponentInParent<EnemyController>();
		playerAnim 	= PlayerController.GetAnimator();
	}

	//void OnTriggerEnter2D(Collider2D other) {
	//	//Debug.Log ("Enemy OnTriggerEnter2D : " + other.name);
	//	if (other.tag == "PlayerArm") {
	//		AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
	//		if (attackHash != stateInfo.fullPathHash) {
	//			attackHash = stateInfo.fullPathHash;
	//			enemyCtrl.ActionDamage ();
	//		}
	//	} else
	//	if (other.tag == "PlayerArmBullet") {
	//		Destroy (other.gameObject);
	//		enemyCtrl.ActionDamage ();
	//	}
	//}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log ("Enemy OnTriggerEnter2D : " + other.name);
		if (other.tag == "PlayerArm")
		{
			enemyCtrl.ActionDamage();
			playsound();
		}
		else
		if (other.tag == "PlayerArmBullet")
		{
			Destroy(other.gameObject);
			enemyCtrl.ActionDamage();
			playsound();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		//Debug.Log ("Enemy OnTriggerEnter2D : " + other.name);
		if (other.tag == "PlayerBeam")
		{
			enemyCtrl.ActionDamage();
			if(sound == true)
			{
				playsound();
			}
			sound = false;
		}
	}

	void Update () {
		AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
		if (attackHash != 0 && stateInfo.fullPathHash == PlayerController.ANISTS_Idle) {
			attackHash = 0;
		}
	}

	public void playsound()
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = Clip;
		audio.volume = 0.3f;
		audio.Play();
	}
}

