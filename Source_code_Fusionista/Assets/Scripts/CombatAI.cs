using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatAI : MonoBehaviour {

	
	public int freeAIMax 		= 3;
	public int blockAttackAIMax = 10;

	
	void FixedUpdate () {
		var activeEnemyMainList = new List<EnemyMain>();

		// ค้นหา enemy ที่อยู่หน้ากล้อง (มองเห็น) แล้วบันทึกลงใน enemyList 
		GameObject[] enemyList = GameObject.FindGameObjectsWithTag ("Enemy");
		if (enemyList == null) {
			return;
		}
		foreach (GameObject enemy in enemyList) {
			EnemyMain enemyMain = enemy.GetComponent<EnemyMain> ();
			if (enemyMain != null) {
				if (enemyMain.combatAIOerder && enemyMain.cameraEnabled) {
					activeEnemyMainList.Add (enemyMain);
				}
			} else {
				Debug.LogWarning(string.Format("CombatAI : EnemyMain null : {0} {1}",enemy.name,enemy.transform.position));
			}
		}

		// คอนโทรล enemy แต่ละตัวที่กำลังโจมตีอยู่ 
		int i = 0;
		foreach (EnemyMain enemyMain in activeEnemyMainList) {
			if (i < freeAIMax) {
				// ปล่อยไปตามโปรแกรม
			} else
			if (i < freeAIMax + blockAttackAIMax) {
				// กลุ่มนี้คุมหน่อย
				if (enemyMain.aiState == ENEMYAISTS.RUNTOPLAYER) {
					enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
				}
			} else {
				// ส่วนที่เหลือให้หยุดเลย
				if (enemyMain.aiState != ENEMYAISTS.WAIT) {
					enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
				}
			}
			i ++;
		}

		//Debug.Log(string.Format(">>> Combat AI {0}",i));
	}
}
