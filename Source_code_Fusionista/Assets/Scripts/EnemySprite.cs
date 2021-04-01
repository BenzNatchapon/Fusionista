using UnityEngine;
using System.Collections;

public class EnemySprite : MonoBehaviour {

	EnemyMain enemyMain;

	void Awake () {
		// เปิดมาปุ๊บค้นหาว่ามีไฟล์ (-->component) ชื่อ EnemyMain แปะอยู่บน Object ที่เป็น parent หรือไม่
		enemyMain = GetComponentInParent<EnemyMain> ();
	}
	
	void OnBecameVisible()
	{
		// ถ้ามองเห็นให้ทำอะไรบางอย่าง
	}
	void OnBecameInvisible()
	{
		// ในกรณีที่ออกจากหน้ากล้อง (มองไม่เห็น) ให้ทำอะไรบางอย่าง
	}

	void OnWillRenderObject() {
		if (Camera.current.tag == "MainCamera") {
			// ถ้า tag => MainCamera ก็ให้เงื่อนไขต่อไปนี้เป็นจริง 
			enemyMain.cameraEnabled = true;
		}
	}

}
