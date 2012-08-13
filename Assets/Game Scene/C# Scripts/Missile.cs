using UnityEngine;
using System;
using System.Collections;

public class Missile : MonoBehaviour{
	// 弾の座標
	// transform.position
	// 自機の座標
	// Player.position
	// 弾速
	// missile_speed
	// 目標までの距離
	// enemy_player_distance
	//
	// ロックオンした対象の敵を代入
	GameObject rockon_enemy_object;
	// プレイヤーを代入
	GameObject player;
	
	// ミサイルの速度
	Vector3 missile_speed_vec;
	// ミサイルの速さ
 	float missile_speed;

	// 旋回角度の上限
	float missile_rotation_max;
	
	// 敵と、ミサイルの角度
	float missile_enemy_rotation;

	//どれくらい飛んだのかをカウントする変数
	int count;

	// 距離を代入
	float enemy_player_distance;

	// ミサイル座標のバックアップ
	Transform rockon_enemy_backup;

	void Awake(){
		//countを初期化
		count = 0;
		//ロックオンした対象の敵を代入
		rockon_enemy_object = Player.enemy[Player.i_num];
		player = GameObject.Find("Player");
	}

	// Use this for initialization
	void Start () {
		missile_speed = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * いつか使うかもしれない。
		double lenght1 = Math.Sqrt(
				(transform.rotation.x * transform.rotation.x) + 
				(transform.rotation.y * transform.rotation.y) + 
				(transform.rotation.z * transform.rotation.z)
				);

		if(lenght1 != 0.0){
			lenght1 = 1.0 / lenght1;
			transform.position = new Vector3(
					transform.position.x * (float)lenght1,
					transform.position.y * (float)lenght1,
					transform.position.z * (float)lenght1
					);
		}

		double lenght2 = Math.Sqrt(
				(rockon_enemy_object.transform.rotation.x * rockon_enemy_object.transform.rotation.x) + 
				(rockon_enemy_object.transform.rotation.y * rockon_enemy_object.transform.rotation.y) + 
				(rockon_enemy_object.transform.rotation.z * rockon_enemy_object.transform.rotation.z)
				);
		
		if(lenght2 != 0.0){
			lenght2 = 1.0 / lenght2;
			rockon_enemy_object.transform.position = new Vector3(
					rockon_enemy_object.transform.position.x * (float)lenght2,
					rockon_enemy_object.transform.position.y * (float)lenght2,
					rockon_enemy_object.transform.position.z * (float)lenght2
					);
		}


		Debug.Log("rockon_enemy_object.transform.rotation.x = " + rockon_enemy_object.transform.rotation.x);
		Debug.Log("transform.rotation.x = " + transform.rotation.x);
		Debug.Log("rockon_enemy_object.transform.rotation.y = " + rockon_enemy_object.transform.rotation.y);
		Debug.Log("transform.rotation.y = " + transform.rotation.y);
		Debug.Log("rockon_enemy_object.transform.rotation.z = " + rockon_enemy_object.transform.rotation.z);
		Debug.Log("transform.rotation.z = " + transform.rotation.z);

		Debug.Log("Math.PI = " + Math.PI);
		Debug.Log("Math.Acos = " + Math.Acos(
			     (transform.rotation.x * rockon_enemy_object.transform.rotation.x) + 
				(transform.rotation.y * rockon_enemy_object.transform.rotation.y) + 
				(transform.rotation.z * rockon_enemy_object.transform.rotation.z)
				));
		Debug.Log("Math.Acos * 180.0 / Math.PI = " + Math.Acos(
			     (transform.rotation.x * rockon_enemy_object.transform.rotation.x) + 
				(transform.rotation.y * rockon_enemy_object.transform.rotation.y) + 
				(transform.rotation.z * rockon_enemy_object.transform.rotation.z)
				) * 180.0 / Math.PI);

				*/

		/*
		 *
		 * 誘導ミサイルのロジック
		 * ミサイルのローカル座標で考える。
		 * ミサイルを常に敵座標に向かせてる。
		 */


		//飛んだ距離を増やす
		count++;
		
		//もし、1000km以上飛んだら
		if(count >= 1000){
			//ミサイルを破壊する
			Destroy(gameObject);
		}

		try{
			rockon_enemy_backup = rockon_enemy_object.transform;			
			// 常に敵の方向を向かせる
			transform.LookAt(rockon_enemy_object.transform);
		}catch(MissingReferenceException){
			transform.Translate(0,0,1.5f);

		}

		// 移動
		transform.Translate(0,0,1.5f);
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "Enemy(Clone)"){
			Destroy(gameObject);
		}
	}
}
