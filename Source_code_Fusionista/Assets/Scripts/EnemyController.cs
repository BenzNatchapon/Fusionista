using UnityEngine;
using System.Collections;


public class EnemyController : BaseCharacterController {

	
	public float 		initHpMax 			= 5.0f;
	public float 		initSpeed 			= 6.0f;
	public bool			jumpActionEnabled 	= true;
	public Vector2 		jumpPower		 	= new Vector2(0.0f,1500.0f); 
	public int			addScore 		 	= 500;


	[System.NonSerialized] public bool 		cameraRendered			= false;
	[System.NonSerialized] public bool 		attackEnabled			= false;
	[System.NonSerialized] public int		attackDamage 			= 1;
	[System.NonSerialized] public Vector2	attackNockBackVector	= Vector3.zero;
	
	
	public readonly static int ANISTS_Idle 	 	= Animator.StringToHash("Base Layer.Enemy_Idle");
	public readonly static int ANISTS_Run 	 	= Animator.StringToHash("Base Layer.Enemy_Run");
	public readonly static int ANISTS_Jump 	 	= Animator.StringToHash("Base Layer.Enemy_Jump");
	public readonly static int ANITAG_ATTACK 	= Animator.StringToHash("Attack");
	public readonly static int ANISTS_DMG_A		= Animator.StringToHash("Base Layer.Enemy_DMG_A");
	public readonly static int ANISTS_DMG_B 	= Animator.StringToHash("Base Layer.Enemy_DMG_B");
	public readonly static int ANISTS_Dead 	 	= Animator.StringToHash("Base Layer.Enemy_Dead");

	
	PlayerController 	playerCtrl;
	Animator 			playerAnim;
	
	
	protected override void Awake () {
		base.Awake ();

		playerCtrl 	= PlayerController.GetController();
		playerAnim 	= playerCtrl.GetComponent<Animator>();

		hpMax 	= initHpMax;
		hp 		= hpMax;
		speed 	= initSpeed;
	}

	protected override void FixedUpdateCharacter () {
		if (!cameraRendered) {
			return;
		}

		// เช็ค jump และกำหนดสถานะหลังจากนั้น 
		if (jumped) {
            // เช็ค ground 
            // กรณีแรกคือเช็คตอนที่ขาลงมาแตะพื้น
            // ระยะเวลาที่ลอยตัวอยู่ในอากาศหลังจากกระโดดไปแล้ว
            // ถ้าเงื่อนไขเหล่านี้อันใดอันหนึ่งเป็นจริงให้เปลี่ยนสถานะ jumped --> false
            
			if ((grounded && !groundedPrev) || 
			    (grounded && Time.fixedTime > jumpStartTime + 1.0f)) {
				jumped = false;
			}
			if (Time.fixedTime > jumpStartTime + 1.0f) {
				if (GetComponent<Rigidbody2D>().gravityScale < gravityScale) {
					GetComponent<Rigidbody2D>().gravityScale = gravityScale;
				}
			}
		} else {
			GetComponent<Rigidbody2D>().gravityScale = gravityScale;
		}

		// ทิศทางการหันหน้าของตัวละคะ
		transform.localScale = new Vector3 (basScaleX * dir, transform.localScale.y, transform.localScale.z);

		// Memo:ถ้าโดน damage กลางอากาศ ห้ามย้ายตำแหน่งในแนวแกน X ให้ตำแหน่งของ X อยู่ตรงนั้นห้ามขยับ
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.nameHash == EnemyController.ANISTS_DMG_A ||
		    stateInfo.nameHash == EnemyController.ANISTS_DMG_B ||
		    stateInfo.nameHash == EnemyController.ANISTS_Dead) {
			speedVx = 0.0f;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0.0f, GetComponent<Rigidbody2D>().velocity.y);
		}
	}

	// === Action พื้นฐานต่างๆ =============================

	public bool ActionJump() {
		if (jumpActionEnabled && grounded && !jumped) {
			animator.SetTrigger ("Jump");
			GetComponent<Rigidbody2D>().AddForce (jumpPower);
			jumped 		  = true;
			jumpStartTime = Time.fixedTime;
		}
		return jumped;
	}
	
	public void ActionAttack(string atkname,int damage) {
		attackEnabled = true;
		attackDamage  = damage;
		animator.SetTrigger (atkname);
	}

	public void ActionDamage() {
		int damage = 0;

		if (hp <= 0) {
			return;
		}

		if (superArmor) {
			animator.SetTrigger ("SuperArmor");
		}

		AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
		if (stateInfo.nameHash == PlayerController.ANISTS_ATTACK_C) {
			damage = 3;
			if (!superArmor || superArmor_jumpAttackDmg) {
				animator.SetTrigger ("DMG_B");
				jumped 			= true;
				jumpStartTime 	= Time.fixedTime;
				AddForceAnimatorVy (1500.0f);
				Debug.Log(string.Format(">>> DMG_B Jump {0}",stateInfo.nameHash));
			}
		} else
		if (!grounded) {
			damage = 2;
			if (!superArmor || superArmor_jumpAttackDmg) {
				animator.SetTrigger ("DMG_B");
				jumped 			= true;
				jumpStartTime 	= Time.fixedTime;
				//AddForceAnimatorVy (10.0f);
				playerCtrl.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f,20.0f));
				Debug.Log(string.Format(">>> DMG_B {0}",stateInfo.nameHash));
			}
		} else {
			damage = 1;
			if (!superArmor) {
				animator.SetTrigger ("DMG_A");
				Debug.Log(string.Format(">>> DMG_A {0}",stateInfo.nameHash));
			}
		}

		if (SetHP(hp - damage,hpMax)) {
			Dead(false);

			int addScoreV = ((int)((float)addScore * (playerCtrl.hp / playerCtrl.hpMax)));
			addScoreV = (int)((float)addScore * (grounded ? 1.0 : 1.5f));
			PlayerController.score += addScoreV;
		}
	}

	// === ตายยยยยยยยยยยยยย !!! ====================================
	public override void Dead(bool gameOver) {
		base.Dead (gameOver);
		Destroy(gameObject,1.0f);
	}

}

