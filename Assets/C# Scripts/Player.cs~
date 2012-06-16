//プレイヤー（機体）につけるスクリプト

using UnityEngine;

public class Player : MonoBehaviour{
	//弾をいれる変数
	GameObject gun;

	//ミサイルをいれる変数
	GameObject missaile;
		
	//TPSカメラをいれる変数
	GameObject tps_camera;
		
	//FPSカメラを入れる変数
	GameObject fps_camera;

	//スピードを管理する変数
	float speed = 600.0f;

	//戦闘時に得た金を管理する変数
	int gold = 0;

	//敵を倒した時に得られる金の変数
	int enemy_gold = 0;

	//変数を初期化する変数？
	void Awake(){
		tps_camera = GameObject.Find("TPS Camera");
		fps_camera = GameObject.Find("FPS Camera");
	}

	void Start()
	{
		//カメラの設定
		tps_camera.camera.enabled = true;
		fps_camera.camera.enabled = false;
		//テストコード
		//スピードを0に初期化
		speed = 0.0f;
		//playerを0,0,0に場所を初期化
		transform.position = Vector3.zero;
	}

	void Update()
	{	
		//playerを移動
		transform.Translate(0.0f,0.0f,speed/600.0f);
		//テストコード
		//カメラの移動
		//tps_camera.transform.Translate(0.0f,0.0f,speed);
		
		//もし、"i"を押し続けていれば、
		if(Input.GetKey("i")){
			//GameObject変数"bullet"に、Object"Bullet"を代入。
			gun = Object.Instantiate(GameObject.Find("Gun")) as GameObject;
			//GameObject変数"bullet"の初期位置を、playerの場所に指定
			gun.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		}
		//もし、"l"を押していれば、
		if(Input.GetKeyDown("l")){
			//GameObject変数"missaile"に、Object"Missaile"を代入。
			missaile = Object.Instantiate(GameObject.Find("Missaile")) as GameObject;
			//GameObject変数"missaile"の初期位置を、playerの場所に指定
			missaile.transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		}

		//移動の設定
		//上移動(w)
		if(Input.GetKey("w")){
			//上へ移動
			transform.Translate(0.0f,1.0f,0.0f);
		}

		//下移動(s)
		if(Input.GetKey("s")){
			//下へ移動
			transform.Translate(0.0f,-1.0f,0.0f);
		}
		
		//右移動(d)
		if(Input.GetKey("d")){
			//右へ移動
			transform.Translate(1.0f,0.0f,0.0f);
		}
		
		//左移動(a)
		if(Input.GetKey("a")){
			//左へ移動
			transform.Translate(-1.0f,0.0f,0.0f);
		}

		//スピードアップ(e)
		if(Input.GetKey("e")){
			//スピードをアップ
			speed += 1.0f;
			//スピードが3488以上ならば、
			if(speed >= 3488){
				//3488に固定する
				speed = 3488;
			}
		}
		//スピードダウン(q)
		if(Input.GetKey("q")){
			//スピードをダウン
			speed -= 1.0f;
			//スピードを351以下ならば、
			if(speed <= 351){
				//351に固定する
				speed = 351;
			}
		}
		
		//テストコード
		//場所とスピードの初期化(r)
		if(Input.GetKeyDown("r")){
			//スピードを0に初期化
			speed = 0.0f;
			//playerを0,0,0に場所を初期化
			transform.position = Vector3.zero;
		}
	}

	void OnGUI()
	{
		//スピードメーター
		GUI.Label(new Rect(5, 0, 400, 50),"Speed:" + speed.ToString());
		//戦闘時に得た金
		GUI.Label(new Rect(5,15, 400, 50),"Gold:" + gold.ToString());
		//敵を倒したときに得られる金
		GUI.Label(new Rect(5,30, 400, 50),"EnemyGold:" + enemy_gold.ToString());
		//テストコード・デバッグモード
		//自分の機体の位置
		GUI.Label(new Rect(5,45, 400, 50),"Position:" + transform.position);
	}
}


