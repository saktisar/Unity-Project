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

	/*
	 * 弾、ミサイル、パーティクル等に関する変数
	 */
	//弾をいれる変数
	public GameObject gun;
	//ミサイルをいれる変数
	public GameObject missaile;
	// 銃を打つ際のパーティクルをいれる変数
	public GameObject gun_particle;
	// 銃の連射力に関する変数
	// 数値が高いほど遅い
	const int gun_rapid = 10;
	// 銃の連射を管理する変数
	int gun_rapid_num = -1;

	void Awake (){
		// gun_particleを無効化
		gun_particle.active = false;
	}

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
		 * ミサイルと、銃のスクリプト
		 */
		//もし、"i"を押し続けていれば、
		if(Input.GetKey("i") && gun_rapid_num == -1){
			//gunオブジェクトのクローンを生成
			Instantiate(gun,transform.position,transform.rotation);
			// 銃の連射速度を決定
			gun_rapid_num = gun_rapid;
		}
		//もし、"l"を押していれば、
		if(Input.GetKeyDown("l")){
			//missaileオブジェクトのクローンを生成
			Instantiate(missaile,transform.position,transform.rotation);
		}
		if(gun_rapid_num != -1){
			gun_particle.active = true;
			gun_rapid_num --;
			// 銃の処理の終了
			if(gun_rapid_num == 0){
				// 銃を打ってない事にする
				gun_rapid_num = -1;
				gun_particle.active = false;
			}
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


