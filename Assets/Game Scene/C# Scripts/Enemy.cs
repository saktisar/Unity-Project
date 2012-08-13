//Enemy(敵)のスクリプト

using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour {
	//弾をいれる変数
	public GameObject gun;

	//ミサイルをいれる変数
	public GameObject missaile;
				
	//スピードを管理する変数
	float speed = 600.0f;
	
	GameObject player;

	GameObject enemy_sign;

	//初期化処理
	void Awake(){
		print("Enemy.cs:In Awake");
		player = GameObject.Find("Player");
	}

	//updateの直前に起動する関数
	void Start() {
		//enemy_mark.transform.localPosition.x
		print("Enemy.cs:In start");
		//場所の初期化
		//transform.position = new Vector3(0,0,-20);
		//transform.Rotate(0, 180, 0);
	}
	
	// update is called once per frame
	void Update (){
		transform.Translate(0.1f,0,1f);
	}

	void OnGUI(){
		//テストコード・デバッグモード
		//敵の機体の位置
		//GUI.Label(new Rect(  0, 15, 400, 50),"EnemyProsition:" + transform.position);
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "Missile(Clone)" || col.gameObject.name == "Gun(Clone)"){
			Destroy(gameObject);
		}
	}
}
