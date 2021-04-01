using UnityEngine;
using System.Collections;

public class StageObject_DogPile : MonoBehaviour {

	public GameObject[] enemyList;
	public GameObject[] destroyObjectList;
	
	void Start () {
		InvokeRepeating ("CheckEnemy",0.0f, 1.0f);
	}

	void CheckEnemy () {

        // เช็คว่าตอนนี้ enemy ยังมีชีวิตอยู่หรือไม่ โดยเช็คจาก list ที่ลงทะเบียนไว้
        //  โดยเช็คทุกๆ 1 วินาทีก็พอไม่ต้องถี่กว่านั้น 

        bool flag = true;
		foreach (GameObject enemy in enemyList) {
			if (enemy != null) {
				flag = false;
			}
		}

        // enemy โดนเล่นงานหมดแล้วหรือยัง
        if (flag) {
          
            // ลบ Game Object ออกจาก destoryObjectList
            foreach (GameObject destroyObject in destroyObjectList) {
				Destroy(destroyObject,1.0f);
			}
			CancelInvoke("CheckEnemy");
            // หยุดการเรียกใช้ฟังก์ชัน CheckEnemy() 
            // ตรงนี้น่าสนใจจำพวกนี้เอาไว้นะครับ มันเป็น pattern
        }
    }
}
