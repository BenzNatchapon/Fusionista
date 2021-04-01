using UnityEngine;
using System.Collections;


public class PlayerMain : MonoBehaviour {


	PlayerController 	playerCtrl;

	AudioSource myAudio;
	public AudioClip attackClip;
	public AudioClip jumpClip;
	public AudioClip swallowClip;
	//public AudioClip damageClip;

	Animator animator;

	public bool slime = true;

	public int heal = 1;

	bool alreadyHeal = false;

	float tempHp;

	//GameObject item;
	GameObject[] items;


	void Awake () {
		playerCtrl = GetComponent<PlayerController>();

		myAudio = GetComponent<AudioSource>();

		animator = gameObject.GetComponent<Animator>();

		slime = true;

		//item = GameObject.FindWithTag("ItemBodySkel");
		items = GameObject.FindGameObjectsWithTag("ItemBodySkel");

	}

	void Update () {
		// ถ้าผู้เล่นขยับไม่ได้ก็จบ ไม่ทำอะไรต่อ
		if (!playerCtrl.activeSts) {
			return;
		}

		// เคลื่อนไหวด้านข้างด้วยลูกศรซ้ายขวา 
		float joyMv = Input.GetAxis ("Horizontal");
		playerCtrl.ActionMove (joyMv);

        // กด space คือกระโดด
        if (Input.GetButtonDown ("Jump")) {

			playsound(jumpClip);

			playerCtrl.ActionJump ();
			return;
		}

		// โจมตีด้วยการกดเมาส์ และเงื่อนไขอื่นๆ ลองกดเล่นดู
		//if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3")) 
		if(Input.GetMouseButtonDown(0))
		{
			if (Input.GetAxisRaw ("Vertical") < 0.5f) {

				playsound(attackClip);

				playerCtrl.ActionAttack();

			} else {

				playsound(attackClip);

				playerCtrl.ActionAttackJump();
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if(slime == true)
			{
				playsound(swallowClip);
			}
			animator.SetTrigger("GetItem");
			Debug.Log("Get Item");
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			StartSwitchAnimToSlimeAnim();
			Debug.Log("Change back");
		}

		//if (Input.GetKeyDown(KeyCode.Q))
		//{
		//	StartSwitchAnimToSlimeAnim();
		//	Debug.Log("Change back");
		//}

		if (Input.GetKeyDown(KeyCode.M))
		{
			playerCtrl.ActionHeal(10);
			foreach (GameObject item in items)
			{
				item.GetComponent<SkelBodyCollider>().restore();
			}
		}

		if (slime == false && alreadyHeal == false)
		{
			alreadyHeal = true;
			playerCtrl.ActionHeal(heal);
			tempHp = playerCtrl.hp;
		}

		if (slime == true && alreadyHeal == true)
		{
			alreadyHeal = false;
		}

		if (slime == false && playerCtrl.hp <= tempHp - heal)
		{
			forceBackToSlime();
		}

	}

	public void playsound(AudioClip myClip)
	{
		if (Time.timeScale == 1f)
		{
			myAudio.clip = myClip;
			myAudio.Play();
		}
	}

	public void forceBackToSlime()
	{
		StartSwitchAnimToSlimeAnim();
		//item.GetComponent<SkelBodyCollider>().restore();
		foreach (GameObject item in items)
		{
			item.GetComponent<SkelBodyCollider>().restore();
		}
	}

	//public void switchAnimToSkelAnim()
	//{
		//if (animator.gameObject.activeSelf)
		//{
			//animator.runtimeAnimatorController = Resources.Load("Animation/SkelAnimController") as RuntimeAnimatorController;

			////OR
			////animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/SkelAnimController");
		//}
	//}

	public void switchAnimToSkelAnim()
	{
		if (animator.gameObject.activeSelf)
		{
			animator.runtimeAnimatorController = Resources.Load("Animation/SkelAnimController") as RuntimeAnimatorController;

			slime = false;
		}
	}

	public void switchAnimToSlimeAnim()
	{
		if (animator.gameObject.activeSelf)
		{
			animator.runtimeAnimatorController = Resources.Load("Animation/SlimeAnimController") as RuntimeAnimatorController;

			slime = true;
		}
	}

	public void StartSwitchAnimToSkelAnim()
	{
		StartCoroutine(StartToSkelAnimNow());
	}

	IEnumerator StartToSkelAnimNow()
	{
		//yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length + 1);

		yield return new WaitForSeconds(0.5f);
		switchAnimToSkelAnim();
	}

	public void StartSwitchAnimToSlimeAnim()
	{
		StartCoroutine(StartToSlimeAnimNow());
	}

	IEnumerator StartToSlimeAnimNow()
	{
		//yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length + 1);

		yield return new WaitForSeconds(0.2f);
		switchAnimToSlimeAnim();
	}

	public void heartHeal()
	{
		playerCtrl.ActionHeal(1);
		tempHp = playerCtrl.hp;
	}
}
