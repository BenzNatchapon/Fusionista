using UnityEngine;
using System.Collections;

// คลาสพื้นฐานของคลาส player & enemy ซึ่งทั้งสองคลาสสืบทอดมาจากคลาสนี้ 
// กำหนดพารามิเตอร์ที่สำคัญ ฟังก์ชันที่สำคัญ ฟังก์ชันที่จำเป็นแต่ยงไม่กำหนดเนื้อหา
// และอื่นๆ ตามหลักการของ OOP

public class BaseCharacterController : MonoBehaviour {

	
	public Vector2			velocityMin				 = new Vector2(-100.0f,-100.0f);
	public Vector2			velocityMax				 = new Vector2(+100.0f,+50.0f);
	public bool				superArmor				 = false;
	public bool				superArmor_jumpAttackDmg = true;
	public GameObject[] 	fireObjectList;

	
	[System.NonSerialized] public float		hpMax			= 10.0f;
	[System.NonSerialized] public float		hp	 			= 10.0f;
	[System.NonSerialized] public float		dir	 			= 1.0f;
	[System.NonSerialized] public float		speed			= 6.0f;
	[System.NonSerialized] public float 	basScaleX		= 1.0f;
	[System.NonSerialized] public bool		activeSts 		= false;
	[System.NonSerialized] public bool 		jumped 			= false;
	[System.NonSerialized] public bool 		grounded 		= false;
	[System.NonSerialized] public bool 		groundedPrev 	= false;

	
	[System.NonSerialized] public Animator	animator;
	protected Transform		groundCheck_L;
	protected Transform 	groundCheck_C;
	protected Transform 	groundCheck_R;


	protected float 	 	speedVx 			= 0.0f;
	protected float 		speedVxAddPower		= 0.0f;

	protected GameObject	groundCheck_OnRoadObject;
	protected GameObject	groundCheck_OnMoveObject;
	protected GameObject	groundCheck_OnEnemyObject;

	protected float			jumpStartTime		= 0.0f;

	protected float 		gravityScale 		= 10.0f;

	protected bool			addForceVxEnabled	= false;
	protected float			addForceVxStartTime = 0.0f;

	protected bool			addVelocityEnabled	= false;
	protected float			addVelocityVx 		= 0.0f;
	protected float			addVelocityVy 		= 0.0f;

	protected bool			setVelocityVxEnabled= false;
	protected bool			setVelocityVyEnabled= false;
	protected float			setVelocityVx 		= 0.0f;
	protected float			setVelocityVy 		= 0.0f;

	// === ฟังก์ชันพื้นฐานต่างๆ  ================
	protected virtual void Awake () {
		animator 			= GetComponent <Animator>();

        // เช็ค ground ด้วยจุด 3 จุด
		groundCheck_L 		= transform.Find("GroundCheck_L");
		groundCheck_C 		= transform.Find("GroundCheck_C");
		groundCheck_R 		= transform.Find("GroundCheck_R");
		
		dir 				= (transform.localScale.x > 0.0f) ? 1 : -1;
		basScaleX 			= transform.localScale.x * dir;
		transform.localScale = new Vector3 (basScaleX, transform.localScale.y, transform.localScale.z);

		activeSts 			= true;
		gravityScale 		= GetComponent<Rigidbody2D>().gravityScale;
	}

	protected virtual void Start () {
	}
	
	protected virtual void Update () {	
		// เช็คว่าตกออกไปจากฉากแล้วหรือยง
		if (transform.position.y < -30.0f) {
			if (activeSts) {
				Dead(false); // ถ้าตกไปต่ำกว่า 30 หน่วยให้คิดว่าตายไปแล้ว
			}
		}
	}

	protected virtual void FixedUpdate () {	
		// เช็ค ground ตรงนี้เป็นพื้นฐานสำคัญของการทำเกม 2D 
		groundedPrev = grounded;
		grounded 	 = false;

		groundCheck_OnRoadObject = null;
		groundCheck_OnMoveObject = null;
		groundCheck_OnEnemyObject = null;

		Collider2D[][] groundCheckCollider = new Collider2D[3][];
		groundCheckCollider [0] = Physics2D.OverlapPointAll (groundCheck_L.position);
		groundCheckCollider [1] = Physics2D.OverlapPointAll (groundCheck_C.position);
		groundCheckCollider [2] = Physics2D.OverlapPointAll (groundCheck_R.position);

		foreach(Collider2D[] groundCheckList in groundCheckCollider) {
			foreach(Collider2D groundCheck in groundCheckList) {
				if (groundCheck != null) {
					if (!groundCheck.isTrigger) {
						grounded = true;
						if (groundCheck.tag == "Road") {
							groundCheck_OnRoadObject = groundCheck.gameObject;
						} else 
						if (groundCheck.tag == "MoveObject") {
							groundCheck_OnMoveObject = groundCheck.gameObject;
						} else 
						if (groundCheck.tag == "EnemyObject") {
							groundCheck_OnEnemyObject = groundCheck.gameObject;
						}
					}
				}
			}
		}

		
		FixedUpdateCharacter (); 

		// คำนวณการเคลื่อนที่
		if (addForceVxEnabled) {
			// การเคลื่อนที่จะให้ฟิสิกส์มาช่วยกำหนด ซึ่งก็คือใช้ force เข้ามากำหนดการเคลื่อนที่ในทิศทางต่างๆ ตามหลักฟิสิกส์ 
			if (Time.fixedTime - addForceVxStartTime > 0.5f) {
				addForceVxEnabled = false;
			}
		} else {
			// คำนวณความเร็วในการเคลื่อนที่ 
			//Debug.Log (">>>> " + string.Format("speedVx {0} y {1} g{2}",speedVx,rigidbody2D.velocity.y,grounded));
			GetComponent<Rigidbody2D>().velocity = new Vector2 (speedVx + speedVxAddPower, GetComponent<Rigidbody2D>().velocity.y);
		}

		// สุดท้ายก็กำหนด vector ที่ใช้ในการเคลื่อนที่ซึ่งมีทั้งทิศทางและขนาด
		if (addVelocityEnabled) {
			addVelocityEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x + addVelocityVx, GetComponent<Rigidbody2D>().velocity.y + addVelocityVy);
		}

		// กำหนดความเร็วไปเลยตรงๆ ว่าอยากได้เท่าไร
		if (setVelocityVxEnabled) {
			setVelocityVxEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (setVelocityVx, GetComponent<Rigidbody2D>().velocity.y);
		}
		if (setVelocityVyEnabled) {
			setVelocityVyEnabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,setVelocityVy);
		}

		// เช็ความเร็วในทิศทางต่างๆ 
		float vx = Mathf.Clamp (GetComponent<Rigidbody2D>().velocity.x, velocityMin.x, velocityMax.x);
		float vy = Mathf.Clamp (GetComponent<Rigidbody2D>().velocity.y, velocityMin.y, velocityMax.y);
		GetComponent<Rigidbody2D>().velocity = new Vector2(vx,vy);
	}

	protected virtual void FixedUpdateCharacter () {	
	}

	// === ค่าต่างๆ ที่คำนวณได้จากฟังก์ชันต่อไปนี้ จะเอาไปกำหนดอิริยาบถการเคลื่อนไหวของ animation  ===============
	public virtual void AddForceAnimatorVx(float vx) {
		//Debug.Log (string.Format("--- AddForceAnimatorVx {0} ----------------",vx));
		if (vx != 0.0f) {
			GetComponent<Rigidbody2D>().AddForce (new Vector2(vx * dir,0.0f));
			addForceVxEnabled	= true;
			addForceVxStartTime = Time.fixedTime;
		}
	}
	
	public virtual void AddForceAnimatorVy(float vy) {
		//Debug.Log (string.Format("--- AddForceAnimatorVy {0} ----------------",vy));
		if (vy != 0.0f) {
			GetComponent<Rigidbody2D>().AddForce (new Vector2(0.0f,vy));
			jumped = true;
			jumpStartTime = Time.fixedTime;
		}
	}
	
	public virtual void AddVelocityVx(float vx) {
		//Debug.Log (string.Format("--- AddVelocityVx {0} ----------------",vx));
		addVelocityEnabled = true;
		addVelocityVx = vx * dir;
	}
	public virtual void AddVelocityVy(float vy) {
		//Debug.Log (string.Format("--- AddVelocityVy {0} ----------------",vy));
		addVelocityEnabled = true;
		addVelocityVy = vy;
	}
	
	public virtual void SetVelocityVx(float vx) {
		//Debug.Log (string.Format("--- setelocityVx {0} ----------------",vx));
		setVelocityVxEnabled = true;
		setVelocityVx = vx * dir;
	}
	public virtual void SetVelocityVy(float vy) {
		//Debug.Log (string.Format("--- setVelocityVy {0} ----------------",vy));
		setVelocityVyEnabled = true;
		setVelocityVy = vy;
	}
	
	public virtual void SetLightGravity() {
		//Debug.Log ("--- SetLightGravity ----------------");
		GetComponent<Rigidbody2D>().velocity 	 = new Vector2(0.0f,0.0f);
		GetComponent<Rigidbody2D>().gravityScale = 0.1f;
	}
	
	public void EnableSuperArmor() {
		//Debug.Log ("--- EnableSuperArmor ----------------");
		superArmor = true;
	}
	public void DisableSuperArmor() {
		//Debug.Log ("--- DisableSuperArmor ----------------");
		superArmor = false;
	}
	
	// === ฟังก์ชัน action ต่างๆ  =============================
	public virtual void ActionMove(float n) {
		if (n != 0.0f) {
			dir 	= Mathf.Sign(n);
			speedVx = speed * dir;
			animator.SetTrigger("Run");
		} else {
			speedVx = 0;
			animator.SetTrigger("Idle");
		}
	}

	public void ActionFire() {
		Transform goFire = transform.Find ("Muzzle");
		foreach(GameObject fireObject in fireObjectList) {
			GameObject go = Instantiate (fireObject,goFire.position,Quaternion.identity) as GameObject;
			go.GetComponent<FireBullet>().ownwer = transform;
		}
	}
	
	public bool ActionLookup(GameObject go,float near) {
		if (Vector3.Distance(transform.position,go.transform.position) > near) {
			dir = (transform.position.x < go.transform.position.x) ? +1 : -1;
			return true;
		}
		return false;
	}
	
	public bool ActionMoveToNear(GameObject go,float near) {
		if (Vector3.Distance(transform.position,go.transform.position) > near) {
			ActionMove( (transform.position.x < go.transform.position.x) ? +1.0f : -1.0f );
			return true;
		}
		return false;
	}
	
	public bool ActionMoveToFar(GameObject go,float far) {
		if (Vector3.Distance(transform.position,go.transform.position) < far) {
			ActionMove( (transform.position.x > go.transform.position.x) ? +1.0f : -1.0f );
			return true;
		}
		return false;
	}

	// === ฟังก์ชันอื่นๆ ที่คิดว่าจำเป็น เช่น Dead() เป็นต้น ====================================
	public virtual void Dead (bool gameOver) {
		animator.SetTrigger ("Dead");
		activeSts = false;
	}

	public virtual bool SetHP(float _hp,float _hpMax) {
		hp 	  		= _hp;
		hpMax 		= _hpMax;
		return (hp <= 0);
	}
}
