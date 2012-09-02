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

		/* 
		 * スピードに関する処理
		 */
		//スピードアップ(e)
		if(Input.GetKey("e")){
			speed += 0.001f;
		}
		// スピードダウン(q)
		if(Input.GetKey("q")){
			// スピードをダウン
			speed -= 0.001f;
		}

		/*
		 * 最大スピードと最小スピードを制御する処理
		 */
		/*
		//スピードが最大以上ならば、
		if(speed >= 6){
			// 最大にする
			speed = 6;
		}

		// スピードが最小以下ならば、
		if(speed < 3){
			// 最小に固定する
			speed = 3;
		}
		*/

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


