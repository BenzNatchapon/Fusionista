using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	public string sceneName;

	void OnTriggerEnter2D(Collider2D obj)
	{
		Debug.Log("GameObject1 collided with 1");

		if (obj.tag == "PlayerBody") {
			Debug.Log("GameObject1 collided with 2");
			SceneManager.LoadScene (sceneName);
			//Destroy(gameObject);
		}

	}

}
