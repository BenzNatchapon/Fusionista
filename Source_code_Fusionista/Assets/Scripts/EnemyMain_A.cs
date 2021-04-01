using UnityEngine;
using System.Collections;

// AI ของ Enemy A-TYPE
public class EnemyMain_A : EnemyMain {

    // === พารามิเตอร์สำคัญที่จะเอาไปใช้กำหนดค่าต่างๆ ของ Enemy ======================
    // ความหมายก็ตามชื่อที่เขียนเลยนะครับ ลองปรับเล่นดู
    public int aiIfRUNTOPLAYER 			= 20;
	public int aiIfJUMPTOPLAYER 		= 30;
	public int aiIfESCAPE 				= 10;
	public int aiIfRETURNTODOGPILE 		= 10;

	public int damageAttack_A 			= 1;

	AudioSource myAudio;
	public AudioClip attackClip;
	public AudioClip jumpClip;

	// ฟังก์ชันนี้จะเขียนทับ FixedUpdateAI() ของคลาสแม่ตามหลักการของ OOP นะ
	public override void FixedUpdateAI () {
        // สถานะของ AI
        switch (aiState) {
		case ENEMYAISTS.ACTIONSELECT	: // สถานะเริ่มต้น
			// เลือก action ว่าจะทำอะไรต่อ
            // เงื่อนไขต่างๆ ลองดูเองนะครับว่าเงื่อนไขแบบนี้ต้องทำอะไร  
			int n = SelectRandomAIState();
			if (n < aiIfRUNTOPLAYER) {
				SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
			} else
			if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER) {
				SetAIState(ENEMYAISTS.JUMPTOPLAYER,1.0f);
			} else
			if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER + aiIfESCAPE) {
				SetAIState(ENEMYAISTS.ESCAPE,Random.Range(2.0f,5.0f));
			} else
			if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER + aiIfESCAPE + aiIfRETURNTODOGPILE) {
				if (dogPile != null) {
					SetAIState(ENEMYAISTS.RETURNTODOGPILE,3.0f);
				}
			} else {
				SetAIState(ENEMYAISTS.WAIT,1.0f + Random.Range(0.0f,1.0f));
			}
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.WAIT:           //พัก
                enemyCtrl.ActionLookup(player,0.1f);
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.RUNTOPLAYER: // เข้าไปใกล้อีกนิด
                if (GetDistanePlayerY() > 3.0f) {
				SetAIState(ENEMYAISTS.JUMPTOPLAYER,1.0f);
			}
			if (!enemyCtrl.ActionMoveToNear(player,2.0f)) {
					Attack_A();
				}
			break;

		case ENEMYAISTS.JUMPTOPLAYER	: // กระโดดเข้าไปใกล้ๆ 
			if (GetDistanePlayer() < 2.0f && IsChangeDistanePlayer(0.5f)) {
				Attack_A();
				break;
			}
			enemyCtrl.ActionJump();
			//playsound(jumpClip);

			enemyCtrl.ActionMoveToNear(player,0.1f);
			SetAIState(ENEMYAISTS.FREEZ,0.5f);
			break;
			
		case ENEMYAISTS.ESCAPE			: // หนีห่างออกไป
			if (!enemyCtrl.ActionMoveToFar(player,7.0f)) {
				SetAIState(ENEMYAISTS.ACTIONSELECT,1.0f);
			}
			break;
		
		case ENEMYAISTS.RETURNTODOGPILE	: // กลับไปที่ dogpile
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
		playsound(attackClip);
		SetAIState(ENEMYAISTS.WAIT,2.0f);
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
		audio.PlayDelayed(0.8f);
	}
}
