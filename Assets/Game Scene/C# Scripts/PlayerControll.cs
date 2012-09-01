/*
 * PlayerControl.cs
 *
 */
using UnityEngine;
using System.Collections;

public class PlayerControll : MonoBehaviour {
	[SerializeField]
	float speed = 4;
	
	float roll = 0;
	float pitch = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * 移動の処理
		 */
		transform.position = transform.position + (transform.forward * speed) * Time.deltaTime;
		transform.localRotation = transform.localRotation * new Quaternion(pitch, 0, 0, 1);
		transform.localRotation = transform.localRotation * new Quaternion(0, 0, roll, 1);
		roll *= 0.9f;
		pitch *= 0.9f;
	}

	public void Roll(float power)
	{
		roll += power * 0.001f;
	}
	
	public void Pitch(float power)
	{
		pitch += power* 0.001f;
	}
}


