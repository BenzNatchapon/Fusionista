using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : BaseCharacterController {

	// === พารามิเตอร์ภายนอก =====================
						 public float 	initHpMax = 20.0f;
	[Range(0.1f,100.0f)] public float 	initSpeed = 12.0f;


	public static	float 		nowHpMax 				= 0;
	public static	float 		nowHp 	  				= 0;
	public static 	int			score 					= 0;

	
	[System.NonSerialized] public Vector3 	enemyActiveZonePointA;
	[System.NonSerialized] public Vector3 	enemyActiveZonePointB;

	// ชื่อตัวแปรที่แสดง animation ต่างๆ อันนี้เป็นวิธีการที่เก่านิดนึง
    // unity version ใหม่จะบ่น ^_^ 
    
	//public readonly static int ANISTS_Idle 	 		= Animator.StringToHash("Base Layer.Player_Idle");
	//public readonly static int ANISTS_Walk 	 		= Animator.StringToHash("Base Layer.Player_Walk");
	//public readonly static int ANISTS_Run 	 	 	= Animator.StringToHash("Base Layer.Player_Run");
	//public readonly static int ANISTS_Jump 	 		= Animator.StringToHash("Base Layer.Player_Jump");
	public readonly static int ANISTS_ATTACK_A 		= Animator.StringToHash("Base Layer.Player_ATK_A");
	public readonly static int ANISTS_ATTACK_B 		= Animator.StringToHash("Base Layer.Player_ATK_B");
	public readonly static int ANISTS_ATTACK_C	 	= Animator.StringToHash("Base Layer.Player_ATK_C");
	public readonly static int ANISTS_ATTACKJUMP_A  = Animator.StringToHash("Base Layer.Player_ATKJUMP_A");
	public readonly static int ANISTS_ATTACKJUMP_B  = Animator.StringToHash("Base Layer.Player_ATKJUMP_B");
	public readonly static int ANISTS_ATTACKJUMP_C  = Animator.StringToHash("Base Layer.Player_ATKJUMP_C");
	public readonly static int ANISTS_DEAD  		= Animator.StringToHash("Base Layer.Player_Dead");

	public readonly static int ANISTS_Idle			= Animator.StringToHash("Base Layer.Slime_Idle");
	public readonly static int ANISTS_Walk			= Animator.StringToHash("Base Layer.Slime_Walk");
	public readonly static int ANISTS_Run			= Animator.StringToHash("Base Layer.Slime_Run");
	public readonly static int ANISTS_Jump			= Animator.StringToHash("Base Layer.Slime_Jump");


	int 			jumpCount			= 0;

	volatile bool 	atkInputEnabled		= false;
	volatile bool	atkInputNow			= false;

	bool			breakEnabled		= true;
	float 			groundFriction		= 0.0f;


	public			GameObject background;
	public			GameObject background2;


	// === ฟังก์ชันที่ช่วยทำให้เขียนโปรแกรมง่ายขึ้น เพราะใช้บ่อยและยาววววว  ===============================
	public static GameObject GetGameObject() {
		return GameObject.FindGameObjectWithTag ("Player");
	}
	public static Transform GetTranform() {
		return GameObject.FindGameObjectWithTag ("Player").transform;
	}
	public static PlayerController GetController() {
		return GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>();
	}
	public static Animator GetAnimator() {
		return GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator>();
	}

	// === ฟังก์ชันพื้นฐาน ================
	protected override void Awake () {
		base.Awake ();

		// กำหนดค่าเริ่มต้น
		speed = initSpeed;
		SetHP(initHpMax,initHpMax);

		// คำนวณหา active zone จาก BoxCollider2D
		BoxCollider2D boxCol2D = transform.Find("Collider_EnemyActiveZone").GetComponent<BoxCollider2D>();
		enemyActiveZonePointA = new Vector3 (boxCol2D.offset.x - boxCol2D.size.x / 2.0f, boxCol2D.offset.y - boxCol2D.size.y / 2.0f);
		enemyActiveZonePointB = new Vector3 (boxCol2D.offset.x + boxCol2D.size.x / 2.0f, boxCol2D.offset.y + boxCol2D.size.y / 2.0f);
		boxCol2D.transform.gameObject.SetActive(false);
	}
	
	protected override void FixedUpdateCharacter () {
		// สถานะปัจจุบันของ animation
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

		// เช็คขาติดพื้นและกระโดด
		if (jumped) {
			if ((grounded && !groundedPrev) || 
				(grounded && Time.fixedTime > jumpStartTime + 1.0f)) {
				animator.SetTrigger ("Idle");
				jumped 	  = false;
				jumpCount = 0;
				GetComponent<Rigidbody2D>().gravityScale = gravityScale;
			}
			if (Time.fixedTime > jumpStartTime + 1.0f) {
				if (stateInfo.fullPathHash == ANISTS_Idle || stateInfo.fullPathHash == ANISTS_Walk || 
				    stateInfo.fullPathHash == ANISTS_Run  || stateInfo.fullPathHash == ANISTS_Jump) {
					GetComponent<Rigidbody2D>().gravityScale = gravityScale;
				}
			}
		} else {
			jumpCount = 0;
			GetComponent<Rigidbody2D>().gravityScale = gravityScale;
		}

		// กำลังโจมตีอยู่หรือไม่?
		if (stateInfo.fullPathHash == ANISTS_ATTACK_A || 
		    stateInfo.fullPathHash == ANISTS_ATTACK_B || 
		    stateInfo.fullPathHash == ANISTS_ATTACK_C || 
		    stateInfo.fullPathHash == ANISTS_ATTACKJUMP_A || 
		    stateInfo.fullPathHash == ANISTS_ATTACKJUMP_B) {
			// ถ้าใช่ก็หยุดเคลื่อนที่ในแนวแกน x คือให้ speedVx = 0 ไปซะเลย
			speedVx = 0;
		}

		// ทิศทางของตัวละคร
		transform.localScale = new Vector3 (basScaleX * dir, transform.localScale.y, transform.localScale.z);

		// ความเร็วในการกระโดดในแนวแกน x ลดลงเรื่อยๆ 
		if (jumped && !grounded && groundCheck_OnMoveObject == null) {
			if (breakEnabled) {
				breakEnabled = false;
				speedVx *= 0.9f;
			}
		}

		// หยุดเคลื่อนที่ (ค่อยๆ ลดความเร็ว) 
		if (breakEnabled) {
			speedVx *= groundFriction;
		}

		// กล้อง
		Vector3 addHigh = new Vector3(0, 2, 0);
		Camera.main.transform.position = transform.position + Vector3.back + addHigh;

		// ฉากหลัง
		float xPos = transform.position.x;
		Vector3 newPos = new Vector3(xPos, 0, 0);
		//background.transform.position = transform.position + Vector3.back;
		//background.transform.position = transform.position;
		background.transform.position = -newPos * 0.1f;
		background2.transform.position = -newPos * 0.05f;
	}

	
	public void EnableAttackInput() {
		atkInputEnabled = true;
	}
	
	public void SetNextAttack(string name) { 
		if (atkInputNow == true) {
			atkInputNow = false;
			animator.Play(name);
		}
	}

	// === action พื้นฐาน =============================
	public override void ActionMove(float n) {
		if (!activeSts) {
			return;
		}

		// กำหนดค่าเริ่มต้น
		float dirOld = dir;
		breakEnabled = false;

		// กำหนด animation โดยใช้ตัวแปร moveSpeed 
		float moveSpeed = Mathf.Clamp(Mathf.Abs (n),-1.0f,+1.0f);
		animator.SetFloat("MovSpeed",moveSpeed);
		//animator.speed = 1.0f + moveSpeed;

		// เช็คการย้ายตำแหน่ง
		if (n != 0.0f) {
			//ย้าย
			dir 	  = Mathf.Sign(n);
			moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;
			speedVx   = initSpeed * moveSpeed * dir;
		} else {
			// หยุด
			breakEnabled = true;
		}

		// เช็คการหันหน้ากลับ
		if (dirOld != dir) {
			breakEnabled = true;
		}
	}

	//public void ActionJump() {
	//	AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
	//	Debug.Log(jumpCount);
	//	if (stateInfo.fullPathHash == ANISTS_Idle || stateInfo.fullPathHash == ANISTS_Walk || stateInfo.fullPathHash == ANISTS_Run || 
	//	    (stateInfo.fullPathHash == ANISTS_Jump && GetComponent<Rigidbody2D>().gravityScale >= gravityScale)) {
	//		switch(jumpCount) {
	//		case 0 :
	//			if (grounded) {
	//				animator.SetTrigger ("Jump");
	//				GetComponent<Rigidbody2D>().velocity = Vector2.up * 30.0f;
	//				jumpStartTime = Time.fixedTime;
	//				jumped = true;
	//				jumpCount ++;
	//			}
	//			break;
	//		case 1 :
	//			if (!grounded) {
	//				animator.Play("Player_Jump",0,0.0f);
	//				GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,20.0f);
	//				jumped = true;
	//				jumpCount ++;
	//			}
	//			break;
	//		}
	//	}
	//}

	public void ActionJump()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		Debug.Log(jumpCount);
		if (true)
		{
			switch (jumpCount)
			{
				case 0:
					if (grounded)
					{
						animator.SetTrigger("Jump");
						GetComponent<Rigidbody2D>().velocity = Vector2.up * 30.0f;
						jumpStartTime = Time.fixedTime;
						jumped = true;
						jumpCount++;
					}
					break;
				case 1:
					if (!grounded)
					{
						animator.Play("Player_Jump", 0, 0.0f);
						GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 20.0f);
						jumped = true;
						jumpCount++;
					}
					break;
			}
		}
	}

	//public void ActionAttack() {
	//	AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
	//	if (stateInfo.fullPathHash == ANISTS_Idle || stateInfo.fullPathHash == ANISTS_Walk || stateInfo.fullPathHash == ANISTS_Run || 
	//	    stateInfo.fullPathHash == ANISTS_Jump || stateInfo.fullPathHash == ANISTS_ATTACK_C) {

	//		animator.SetTrigger ("Attack_A");
	//		if (stateInfo.fullPathHash == ANISTS_Jump || stateInfo.fullPathHash == ANISTS_ATTACK_C) {
	//			GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f,0.0f);
	//			GetComponent<Rigidbody2D>().gravityScale = 0.1f;
	//		}
	//	} else {
	//		if (atkInputEnabled) {
	//			atkInputEnabled = false;
	//			atkInputNow 	= true;
	//		}
	//	}
	//}

	public void ActionAttack()
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (true)
		{

			animator.SetTrigger("Attack_A");
			if (stateInfo.fullPathHash == ANISTS_Jump || stateInfo.fullPathHash == ANISTS_ATTACK_C)
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
				GetComponent<Rigidbody2D>().gravityScale = 0.1f;
			}
		}
		else
		{
			if (atkInputEnabled)
			{
				atkInputEnabled = false;
				atkInputNow = true;
			}
		}
	}


	public void ActionAttackJump() {
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (grounded && 
		    (stateInfo.fullPathHash == ANISTS_Idle || stateInfo.fullPathHash == ANISTS_Walk || stateInfo.fullPathHash == ANISTS_Run ||
		     stateInfo.fullPathHash == ANISTS_ATTACK_A || stateInfo.fullPathHash == ANISTS_ATTACK_B)) {
			animator.SetTrigger ("Attack_C");
			jumpCount = 2;
		} else {
			if (atkInputEnabled) {
				atkInputEnabled = false;
				atkInputNow 	= true;
			}
		}
	}

	public void ActionDamage(float damage) {
		if (!activeSts) {
			return;
		}
		
		animator.SetTrigger ("DMG_A");
		speedVx = 0;
		GetComponent<Rigidbody2D>().gravityScale = gravityScale;
		
		if (jumped) {
			damage *= 1.5f;
		}
		
		if (SetHP(hp - damage,hpMax)) {
			Dead(true); // ตาย
		}
	}
	
	
	public override void Dead(bool gameOver) {
		// ถ้าตายจะจัดการดังต่อไปนี้

		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (!activeSts || stateInfo.fullPathHash == ANISTS_DEAD) {
			return;
		}
		
		base.Dead (gameOver);
		
		SetHP(0,hpMax);
		Invoke ("GameOver", 3.0f);
	}
	
	public void GameOver() {
		PlayerController.score = 0;
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
		// ตรงนี้อาจจะเปลี่ยนฉากไปอีกฉากหนึ่งก็ได้
	}

	public override bool SetHP(float _hp,float _hpMax) {
		if (_hp > _hpMax) {
			_hp = _hpMax;
		}
		
		nowHp 		= _hp;
		nowHpMax 	= _hpMax;
		return base.SetHP (_hp, _hpMax);
	}

	public void ActionHeal(float heal)
	{
		SetHP(hp + heal, hpMax);
	}
}


