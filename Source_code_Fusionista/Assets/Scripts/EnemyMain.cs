using UnityEngine;
using System.Collections;

public enum ENEMYAISTS // --- สถานะต่างๆ ของ AI ของ Enemy ---
{
	ACTIONSELECT,		// เลือก Action ว่าจะทำอะไร ซึ่งเป็นจุดเริ่มต้นของสถานะ
	WAIT,				// หยุดรอแป๊บนึง
	RUNTOPLAYER,		// วิ่งเข้าไปหา player
	JUMPTOPLAYER,		// กระโดดเข้าไปหา player
	ESCAPE,				// วิ่งหนี player
	RETURNTODOGPILE,	// กลับไปที่ dogpile
	ATTACKONSIGHT,		// โจมตีจากตำแหน่งนั้นเลย
	FREEZ,				// หยุด action ทั้งหมด (แต่ยังเคลื่อนที่ได้เหมือนเดิม)
}

public class EnemyMain : MonoBehaviour {

	// === พารามิเตอร์สำคัญ  =====================
	public 		bool				cameraSwitch 			= true;
	public		bool				inActiveZoneSwitch		= false;
	public		bool				combatAIOerder			= true;
	public 		float 				dogPileReturnLength 	= 10.0f;

	public		int					debug_SelectRandomAIState = -1;

    // === พารามิเตอร์สำคัญ ======================================
    [System.NonSerialized] public bool		  	cameraEnabled 	= false;
	[System.NonSerialized] public bool		  	inActiveZone	= false;
	[System.NonSerialized] public ENEMYAISTS 	aiState			= ENEMYAISTS.ACTIONSELECT;
	[System.NonSerialized] public GameObject	dogPile;

    // === พารามิเตอร์สำคัญ ==========================================
    protected EnemyController 	enemyCtrl;
	protected 	GameObject		 	player;
	protected 	PlayerController 	playerCtrl;

	protected 	float				aiActionTimeLength		= 0.0f;
	protected 	float				aiActionTImeStart		= 0.0f;
	protected 	float				distanceToPlayer 		= 0.0f;
	protected 	float				distanceToPlayerPrev	= 0.0f;

	
	public virtual void Awake() {
		enemyCtrl 	 	= GetComponent <EnemyController>();
		player 			= PlayerController.GetGameObject ();
		playerCtrl 		= player.GetComponent<PlayerController>();
	}

	public virtual void Start () {
		// Dog Pile Set
		StageObject_DogPile[] dogPileList = GameObject.FindObjectsOfType<StageObject_DogPile>();
		foreach(StageObject_DogPile findDogPile in dogPileList) {
			foreach(GameObject go in findDogPile.enemyList) {
				if (gameObject == go) {
					dogPile = findDogPile.gameObject;
					break;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
	}
	void OnTriggerStay2D(Collider2D other) {
		// เช็คสถานะต่างๆ แล้วเคลื่อนไหวตามเงื่อนไขนั้นๆ 
		if (enemyCtrl.grounded && CheckAction ()) {
			
			if (other.name == "EnemyJumpTrigger_L") {
				if (enemyCtrl.ActionJump ()) {
					enemyCtrl.ActionMove(-1.0f);
				}
			} else
			if (other.name == "EnemyJumpTrigger_R") {
				if (enemyCtrl.ActionJump ()) {
					enemyCtrl.ActionMove(+1.0f);
				}
			} else
			if (other.name == "EnemyJumpTrigger") {
				enemyCtrl.ActionJump ();
			} 
		}
	}
	void OnTriggerExit2D(Collider2D other) {
	}
	
	public virtual void Update () {
		cameraEnabled = false;
	}

	public virtual void FixedUpdate () {
		if (BeginEnemyCommonWork ()) {
			FixedUpdateAI();
			EndEnemyCommonWork ();
		}
	}

	public virtual void FixedUpdateAI () {
	}


	// === AI พื้นฐาน =============================
	public bool BeginEnemyCommonWork () {
		// ยังมีชีวิตอยู่หรือไม่ 
		if (enemyCtrl.hp <= 0) {
			return false;
		}

		// เข้ามาใน active zone แล้วหรือยัง
		if (inActiveZoneSwitch) {
			inActiveZone = false;
			Vector3 vecA = player.transform.position + playerCtrl.enemyActiveZonePointA;
			Vector3 vecB = player.transform.position + playerCtrl.enemyActiveZonePointB;
			if (transform.position.x > vecA.x && transform.position.x < vecB.x && 
			    transform.position.y > vecA.y && transform.position.y < vecB.y) {
				inActiveZone  = true;
			}
		}
		
		
		if (enemyCtrl.grounded) {
			// เข้ามาอยู่หน้ากล้องแล้วหรือยัง 
			if (cameraSwitch && !cameraEnabled && !inActiveZone) {
				// ถ้าไม่อยู่หน้ากล้อง (มองไม่เห็น) ให้กำหนดค่าต่างๆ ตามนี้ 
				enemyCtrl.ActionMove (0.0f);
				enemyCtrl.cameraRendered 	= false;
				enemyCtrl.animator.enabled 	= false;
				GetComponent<Rigidbody2D>().Sleep ();
				return false;
			}
		}
		enemyCtrl.animator.enabled 	= true;
		enemyCtrl.cameraRendered 	= true;


		// เช็คสถานะ
		if (!CheckAction ()) {
			return false;
		}

		// เช็ค dogPile 
		if (dogPile != null) {
			if (GetDistaneDogPile() > dogPileReturnLength) {
				aiState = ENEMYAISTS.RETURNTODOGPILE;
			}
		}

		return true;
	}

	public void EndEnemyCommonWork() {
		// เกินเวลาที่กำหนดไว้แล้วหรือยัง ถ้าเกินให้หยุดแล้วเลือกสถานะใหม่ 
		float time = Time.fixedTime - aiActionTImeStart;
		if (time > aiActionTimeLength) {
			aiState = ENEMYAISTS.ACTIONSELECT;
		}
	}

	public bool CheckAction() {
		// เช็คสถานะ 
		AnimatorStateInfo stateInfo = enemyCtrl.animator.GetCurrentAnimatorStateInfo(0);

		if (stateInfo.tagHash  == EnemyController.ANITAG_ATTACK ||
		    stateInfo.nameHash == EnemyController.ANISTS_DMG_A ||
		    stateInfo.nameHash == EnemyController.ANISTS_DMG_B ||
		    stateInfo.nameHash == EnemyController.ANISTS_Dead) {
			return false;
		}

		return true;
	}

	public int SelectRandomAIState() {

		return Random.Range (0, 100 + 1);
	}

	public void SetAIState(ENEMYAISTS sts,float t) {
		aiState 			= sts;
		aiActionTImeStart  	= Time.fixedTime;
		aiActionTimeLength 	= t;
	}
	
	public virtual void SetCombatAIState(ENEMYAISTS sts) {
		aiState 		  = sts;
		aiActionTImeStart = Time.fixedTime;
		enemyCtrl.ActionMove (0.0f);
	}


	public float GetDistanePlayer() {
		distanceToPlayerPrev = distanceToPlayer;
		distanceToPlayer = Vector3.Distance (transform.position, playerCtrl.transform.position);
		return distanceToPlayer;
	}

	public bool IsChangeDistanePlayer(float l) {
		return (Mathf.Abs(distanceToPlayer - distanceToPlayerPrev) > l);
	}

	public float GetDistanePlayerX() {
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.y = 0; posA.z = 0;
		posB.y = 0; posB.z = 0;
		return Vector3.Distance (posA, posB);
	}
	
	public float GetDistanePlayerY() {
		Vector3 posA = transform.position;
		Vector3 posB = playerCtrl.transform.position;
		posA.x = 0; posA.z = 0;
		posB.x = 0; posB.z = 0;
		return Vector3.Distance (posA, posB);
	}

	public float GetDistaneDogPile() {
		return Vector3.Distance (transform.position, dogPile.transform.position);
	}
}

/*
#if UNITY_EDITOR
		if (debug_SelectRandomAIState >= 0) {
			return debug_SelectRandomAIState;
		}
#endif
*/