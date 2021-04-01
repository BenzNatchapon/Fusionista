using UnityEngine;
using System.Collections;

// AI ของ Enemy B-TYPE
public class EnemyMain_B : EnemyMain {

    // === พารามิเตอร์สำคัญที่จะเอาไปใช้กำหนดค่าต่างๆ ของ Enemy ======================
    // ความหมายก็ตามชื่อที่เขียนเลยนะครับ ลองปรับเล่นดู
    public int aiIfRUNTOPLAYER 			= 30;
	public int aiIfESCAPE 				= 20;
	public int aiIfRETURNTODOGPILE 		= 10;

	public int damageAttack_A 			= 1;
	public int damageAttack_B 			= 2;

    // ฟังก์ชันนี้จะเขียนทับ FixedUpdateAI() ของคลาสแม่ตามหลักการของ OOP 
    public override void FixedUpdateAI () {
		// สถานะของ AI
		switch (aiState) {
		case ENEMYAISTS.ACTIONSELECT	: // สถานะเริ่มต้น 
			// เลือก action 
			int n = SelectRandomAIState();
			if (n < aiIfRUNTOPLAYER) {
				SetAIState(ENEMYAISTS.RUNTOPLAYER,3.0f);
			} else
			if (n < aiIfRUNTOPLAYER + aiIfESCAPE) {
				SetAIState(ENEMYAISTS.ESCAPE,Random.Range(2.0f,5.0f));
			} else
			if (n < aiIfRUNTOPLAYER + aiIfESCAPE + aiIfRETURNTODOGPILE) {
				if (dogPile != null) {
					SetAIState(ENEMYAISTS.RETURNTODOGPILE,3.0f);
				}
			} else {
				SetAIState(ENEMYAISTS.WAIT,1.0f + Random.Range(0.0f,1.0f));
			}
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.WAIT			: // พัก
			enemyCtrl.ActionLookup(player,0.1f);
			enemyCtrl.ActionMove (0.0f);
			break;

		case ENEMYAISTS.RUNTOPLAYER		: // เข้าไปใกล้
			if (GetDistanePlayerY() < 3.0f) {
				if (!enemyCtrl.ActionMoveToNear(player,2.0f)) {
					Attack_A();
				}
			} else {
				if (GetDistanePlayerX() > 3.0f && !enemyCtrl.ActionMoveToNear(player,5.0f)) {
					Attack_A();
				}
			}
			break;

		case ENEMYAISTS.ESCAPE			: // หนีห่างออกไป
			if (!enemyCtrl.ActionMoveToFar(player,4.0f)) {
				Attack_B();
			}
			break;
		
		case ENEMYAISTS.RETURNTODOGPILE	: // กลับไปที่ dogpile
			if (!enemyCtrl.ActionMoveToNear(dogPile,2.0f)) {
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
		enemyCtrl.attackNockBackVector = new Vector2(500.0f,2000.0f);
		SetAIState(ENEMYAISTS.WAIT,3.0f);
	}

	void Attack_B() {
		enemyCtrl.ActionLookup(player,0.1f);
		enemyCtrl.ActionMove (0.0f);
		enemyCtrl.ActionAttack("Attack_B",damageAttack_B);
		enemyCtrl.attackNockBackVector = new Vector2(500.0f,1000.0f);
		SetAIState(ENEMYAISTS.FREEZ,5.0f);
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
}
