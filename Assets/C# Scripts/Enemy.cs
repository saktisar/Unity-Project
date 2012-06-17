//Enemy(敵)のスクリプト

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	GameObject enemy;

	//弾をいれる変数
	GameObject gun;

	//ミサイルをいれる変数
	GameObject missaile;
				
	//スピードを管理する変数
	float speed = 600.0f;
	
	//初期化処理
	void Awake(){
		gun = GameObject.Find("Gun");
		missaile = GameObject.Find("Missaile");
	}

	//updateの直前に起動する関数
	void start () {
		
	}
	
	// update is called once per frame
	void update () {
		if(Input.GetKeyDown("f")){
			//gameobject変数"bullet"の初期位置を、Enemyの場所に指定
			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		}
	}

	void OnGUI(){
		//テストコード・デバッグモード
		//敵の機体の位置
		GUI.Label(new Rect(  5, 60, 400, 50),"EnemyProsition:" + transform.position);
	}

	void OnColisionEnter(){
		print("test");
	}
}
