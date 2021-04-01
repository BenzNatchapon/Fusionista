using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockBehavior : MonoBehaviour
{
	public Camera camera;
	public string tag;

	private Transform previousTarget;

	private Transform trackingBehavior;

	private bool isLocked = false;

	// Use this for initialization
	void Start()
	{
		trackingBehavior = camera.GetComponent<Transform>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == tag && !isLocked)
		{
			isLocked = true;
			PushTarget(other.transform);
		}
		//Test Shake
		//CameraShake2.Shake(1.0f, 0.5f);
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == tag && isLocked)
		{
			isLocked = false;
			PopTarget();
		}
	}

	private void PushTarget(Transform newTarget)
	{
		previousTarget = trackingBehavior;
		trackingBehavior = newTarget;
	}

	private void PopTarget()
	{
		trackingBehavior = previousTarget;
	}
}
