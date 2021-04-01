using UnityEngine;
using System.Collections;

// AI ของ Enemy C-TYPE
public class EnemyMain_C : EnemyMain {

	// === พารามิเตอร์สำคัญที่จะเอาไปใช้กำหนดค่าต่างๆ ของ Enemy ======================
    // ความหมายก็ตามชื่อที่เขียนเลยนะครับ ลองปรับเล่นดู

	public int 		aiIfATTACKONSIGHT 		= 50; 
	public int 		aiIfRUNTOPLAYER 		= 10;
	public int	 	aiIfESCAPE 				= 10;
	public int 		aiIfRETURNTODOGPILE 	= 10;
	public float 	aiPlayerEscapeDistance = 0.0f;

	public int 		damageAttack_A 			= 1;

	public int 		fireAttack_A			= 3;
	public float 	waitAttack_A			= 10.0f;


	// === ยิงดาวกระจายใส่ โดยเริ่มนับจาก 0 ======================================
	int fireCountAttack_A = 0;


	AudioSource myAudio;
	public AudioClip attackClip;
	public AudioClip jumpClip;
	bool sound = false;

	// ฟังก์ชันนี้จะเขียนทับ FixedUpdateAI() ของคลาสแม่ตามหลักการของ OOP นะ
	public override void FixedUpdateAI () {
		// ถ้า player เข้ามาใกล้ให้วิ่งหนี 
		enemyCtrl.ActionMoveToFar (player, aiPlayerEscapeDistance);

		// สถานะของ AI
		switch (aiState) {
		case ENEMYAISTS.ACTIONSELECT	: // สถานะเริ่มต้น
			// เลือก action ว่าจะทำอะไรต่อ
            // เงื่อนไขต่างๆ ลองดูเองนะครับว่าเงื่อนไขแบบนี้ต้องทำอะไร 

			int n = SelectRandomAIState();
			if (n < aiIfATTACKONSIGHT) {
				SetAIState(ENEMYAISTS.ATTACKONSIGHT,100.0f);
			} else
			if (n < aiIfATTACKONSIGHT + aiIfRUNTOPLAYER) {
				SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
			} else
			if (n < aiIfATTACKONSIGHT + aiIfRUNTOPLAYER + aiIfESCAPE) {
				SetAIState(ENEMYAISTS.ESCAPE,Random.Range(2.0f,5.0f));
			} else
			if (n < aiIfATTACKONSIGHT + aiIfRUNTOPLAYER + aiIfESCAPE + aiIfRETURNTODOGPILE) {
				if (dogPile != null) {
					SetAIState(ENEMYAISTS.RETURNTODOGPILE,3.0f);
				}
			} else {
				SetAIState(ENEMYAISTS.WAIT,1.0f + Random.Range(0.0f,1.0f));
			}
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.WAIT			: //พัก
			enemyCtrl.ActionLookup(player,0.1f);
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.ATTACKONSIGHT 	: //โจมตีจากตำแหน่งนั้น
			Attack_A();
			break;

		case ENEMYAISTS.RUNTOPLAYER		: // เข้าไปใกล้อีกนิด
			if (!enemyCtrl.ActionMoveToNear(player,10.0f)) {
				Attack_A();
			}
			break;

		case ENEMYAISTS.ESCAPE			: // หนีห่างออกไป
			if (!enemyCtrl.ActionMoveToFar(player,4.0f)) {
				Attack_A();
			}
			break;
		
		case ENEMYAISTS.RETURNTODOGPILE: // ไปที่ตำแหน่งของ dogpile
			if (enemyCtrl.ActionMoveToNear(dogPile,2.0f)) {
				if (GetDistanePlayer() < 2.0f) {
					Attack_A();
				}
			} else {
				SetAIState(ENEMYAISTS.ACTIONSELECT,1.0f);
			}
			break;
		}
	}
    // อันนี้เป็นการโจมตีพื้นๆ 
	void Attack_A() {
		enemyCtrl.ActionLookup(player,0.1f);
		enemyCtrl.ActionMove (0.0f);
		enemyCtrl.ActionAttack("Attack_A",damageAttack_A);
		if(sound == true)
		{
			playsound(attackClip);
		}
		sound = true;

		fireCountAttack_A ++;
		if (fireCountAttack_A >= fireAttack_A) {
			fireCountAttack_A = 0;
			SetAIState (ENEMYAISTS.FREEZ, waitAttack_A);
		}
	}
	
	// === อันนี้เป็น Combat AI  ==========================
	public override void SetCombatAIState(ENEMYAISTS sts) {
		base.SetCombatAIState (sts);
		switch (aiState) {
		case ENEMYAISTS.ACTIONSELECT	: break;
		case ENEMYAISTS.WAIT			: aiActionTimeLength = 1.0f + Random.Range(0.0f,1.0f); break;
		case ENEMYAISTS.RUNTOPLAYER		: aiActionTimeLength = 3.0f; break;
		case ENEMYAISTS.JUMPTOPLAYER	: aiActionTimeLength = 1.0f; break;
		case ENEMYAISTS.ESCAPE			: aiActionTimeLength = Random.Range(2.0f,5.0f); break;
		case ENEMYAISTS.RETURNTODOGPILE	: aiActionTimeLength = 3.0f; break;
		}
	}

	public void playsound(AudioClip myClip)
	{
		AudioSource audio = GetComponent<AudioSource>();
		audio.clip = myClip;
		audio.volume = 0.25f;
		audio.PlayDelayed(0.4f);
	}
}
