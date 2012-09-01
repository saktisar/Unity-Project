// encoding : UTF-16

//プレイヤー（機体）につけるスクリプト
/****************************************************************************************************************************************
// ここは、メモ(役に立たなかった)
// 特殊な変数達
// スピード加減値 - 加速または、減速をすると、変わる値 - ok
// 高度値 				- 高度の数値(スピードとかにも、影響する) - ok
// 高度によるスピード値 - 高度により可変するスピード値 
// 純粋スピード値 - 純粋なスピードの値(高度は関係ない) - ok																 	
// スピード値 		- (スピード加減値 + (高度値の計算) + 純粋スピード値) で求まる実際のスピード値 	- こいつは変数を持たない(関数で返す)
******************************************************************************************************************************************/


/*****************************************************************************************************************************************
// スピードの求め方
// speed = speed基準値(600)
// speed += (スピード基本加減値 - 高高度補正値 + カメラ加減値 + エンジン加減値)
*****************************************************************************************************************************************/
//
// スピードの加減値は存在しない(スピードの加減は関数で行う)
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{
	[SerializeField]
	PlayerControll playercontroll;
	PlayerRockon playergui;	

	//enemyオブジェクトを入れる変数
	public static GameObject[] enemy = new GameObject[8];


	// 敵の数は４体です。
	// 敵の数を保持する
	public static int enemy_num = 4;
	
	/*************************************************************
	 * 変数の宣言
	 *************************************************************/
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

	/*
	 * 高度に関する処理
	 *
	 */
	// 高度
	int altitude;
	// 最大高度 - ストール(自機が落ちる)する値の設定
	float altitude_maximum;
	// スピードを下げる高度
	int speed_down_altitude;
	float high_altitude_down_speed = 0.4f;
	//
	// 最小高度 - いらない、最小高度は、0だから
	// float altitude_minimum;

	/*
	 * スピードに関する変数
	 */
	// 総合スピード値
	//
	// speed = speed基準値(600)
	//
	public static float speed = 600.0f;
	// 高度によるスピード値
	// float altitude_speed;
	// 移動処理がされたかを示す変数
	bool move_use = false;
	// 純粋スピード値 - 純粋なスピードの値(高度は関係ない)
	// float speed = 600.0f;
	// テストコード
	// float speed = 0.0f;
	// スピード値 - 変数は持たない
	// float speed = 600.0f;
	// スピードの最大値を代入する変数
	float speed_maximum;
	// スピードの最小値を代入する変数
	float speed_minimum;
	// スピードの基準値
	float speed_base;
	// スピード加減値 - 加速または、減速をすると、変わる値
	int speed_up_down;
	//
	// スピード基本加減値
	// 
	float default_speed_up_down = 1.0f;
	//
	// エンジン加減値
	//
	float engine_up_down;
	/*
	 * 敵の金に関する変数
	 */
	//戦闘時に得た金を管理する変数
	int gold = 0;
	//敵を倒した時に得られる金の変数
	int enemy_gold = 0;

	/*
	 * 座標に関する変数
	 */
	// ロックオンできる数を取得
	//int can_rockon;
	// ロックオン時のiを保持する
	public static int i_num;

	/*
	 * 移動時のカメラの移動量を代入する変数たち
	 */
	// カメラの幅を指定する変数
	float camera_width;
	// カメラの高さを指定する変数
	float camera_height;

	//変数を初期化する関数
	void Awake(){
		// 一応追加
		Debug.Log("Player.cs:In Awake");
		/*
		 * テクスチャをロードする処理
		 */			
		/* gun,missaile,enemyオブジェクトは、Unityではりつけ。*/
		
		/*
		 * スピードに関する変数
		 */
		// テストコード
		// EnemyGameObjectを代入
		enemy[0] = (GameObject)Resources.Load("Prefabs/Enemy");
		enemy[1] = (GameObject)Resources.Load("Prefabs/Enemy");
		enemy[2] = (GameObject)Resources.Load("Prefabs/Enemy");
		enemy[3] = (GameObject)Resources.Load("Prefabs/Enemy");
		//enemy[4] = (GameObject)Resources.Load("Prefabs/Enemy");
		//enemy[5] = (GameObject)Resources.Load("Prefabs/Enemy");
		//enemy[6] = (GameObject)Resources.Load("Prefabs/Enemy");
		//enemy[7] = (GameObject)Resources.Load("Prefabs/Enemy");
		// テストコード
		// 敵オブジェクトの複製
		enemy[0] = Instantiate(enemy[0],new Vector3(-5,-5,20),Quaternion.Euler(0,0,0)) as GameObject;
		enemy[1] = Instantiate(enemy[1],new Vector3(5,0,20),Quaternion.Euler(0,0,0)) as GameObject;
		enemy[2] = Instantiate(enemy[2],new Vector3(-5,0,20),Quaternion.Euler(0,0,0)) as GameObject;
		enemy[3] = Instantiate(enemy[3],new Vector3(0,5,20),Quaternion.Euler(0,0,0)) as GameObject;
		//enemy[4] = Instantiate(enemy[4],new Vector3(5,5,20),Quaternion.Euler(0,0,0)) as GameObject;
		//enemy[5] = Instantiate(enemy[5],new Vector3(-5,5,20),Quaternion.Euler(0,0,0)) as GameObject;
		//enemy[6] = Instantiate(enemy[6],new Vector3(0,-5,20),Quaternion.Euler(0,0,0)) as GameObject;
		//enemy[7] = Instantiate(enemy[7],new Vector3(5,-5,20),Quaternion.Euler(0,0,0)) as GameObject;

		//テストコード
		//スピードを0に初期化
		// true_speed = 0.0f;
		//
		// ロックオンできる数
		// can_rockon = 1;

		// ロックオン
		// look_num = 0;
	
		/*
		 * スピードに関する初期化
		 */
		// スピードの最大値を代入
		speed_maximum = 3488;
		// スピードの最小値を代入
		speed_minimum = 351;
		// スピードの基準値を代入
		speed_base = 600;
		
		/*
		 * TPSカメラの位置に関する初期化
		 */
		camera_width = 2.3f;
		camera_height = -8.3f;

		/*
		 * 高度に関する処理
		 */
		altitude_maximum = 10000;
		speed_down_altitude = 8000;
		
		// gun_particleを無効化
		gun_particle.active = false;
	}

	void Start()
	{
		// 照準をリサイズ
		// mark.Resize(128,128);
		// mark.Apply();
	
		// テストコード
		// playerを0,0,0に場所を初期化
		// transform.position = Vector3.zero;
		
		// スピード
		// speed = 600.0f;
	}

	void Update()
	{	
		// 高度の決定
		altitude = ((int)transform.position.y) * 100;
		Debug.Log(altitude);
		/*
		Debug.Log(	
				(
			 		((altitude - speed_down_altitude) > 0) ? 
					(high_altitude_down_speed * (altitude / ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude)))) : 0
				) 
			);
		Debug.Log(
			default_speed_up_down - 
			(
			 ((altitude - speed_down_altitude) > 0) ? 
			 (high_altitude_down_speed * (altitude / ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude)))) : 0
			) - 0
				);
		*/
		/*
		 *
		 * スピードに関する処理
		 *
		 */
		//
		//
		// speed += (高高度補正値 + カメラ加減値 + エンジン加減値)	
		//
		//
		/*
		if((altitude - speed_down_altitude) > 0){
			if(player_direction == true){
				// Debug.Log("player_direction == true : " + (speed -= high_altitude_down_speed * ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude))));
				// speed -= high_altitude_down_speed * ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude));
			}else{
				// Debug.Log("player_direction == false : " + (speed += high_altitude_down_speed * ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude))));
				// speed += high_altitude_down_speed * ((altitude - speed_down_altitude) / (altitude_maximum - speed_down_altitude));
			}
		}	
		*/
		/******************************************************************
		 *
		 *
		 * 高度に関する処理
		 * 移動入力がされているか否か
		 *
		 *
		 *****************************************************************/
		/*
		// 開始以降動きが一切なしの状態で、w,s,d,aのいずれかが押された場合
		if((move_use == false) && (Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("a"))){
			// 動いたと、フラグを立てる
			move_use = true;
		}

		// もし、動いている場合
		if(move_use == true){
			// ログ
			Debug.Log("Player.cs : move_use == true");
			// 高度の計算をする
			Altitude_Speed(altitude);
		}
		*/
		/******************************************************************
		 *
		 *
		 * スピードに関する処理
		 *
		 *
		 ******************************************************************/
		/*
		 * スピード自動加速・減速に関するコード
		 */
		/*
		// 機首が上に向いている場合
		if(transform.rotation.x > 0 && transform.rotation.x <= 90){
			//
			//スピードが自動であがっていく処理
			//
			// スピードが、ベースのスピードより大きければ、
			if(speed > speed_base){
				// 大きい場合
				// スピードが、小さい場合
				if(speed >= ((speed_maximum - speed_minimum) / 3 * 3)){
					print("a");
					speed -= 0.8f;
				// 中間な場合
				// 2091km以下の場合
				}else if(speed >= ((speed_maximum - speed_minimum) / 3 * 2)){
					print("b");
					speed -= 0.5f;
				// その他の小さな場合
				// スピードが、0以下の場合
				}else if(speed >= 0){
					print("c");
					speed -= 0.11f;
				}

				// スピードが、ベースのスピードより小さければ、
				if(speed <= speed_base){
					// speed を、ベースのスピードする
					speed = speed_base;
				}
			}
			//-方向に傾いている場合
		}else{
			//
			// スピードが自動で下がっていく処理
			//
			// 大きい場合
			// スピードが、(3488 - 351) / 3 * 3、つまり3137km以下の場合
			if(speed >= ((speed_maximum - speed_minimum) / 3 * 3)){
				print("d");
				speed += 0.8f;
			// 中間な場合
			// 2091km以下の場合
			}else if(speed >= ((speed_maximum - speed_minimum) / 3 * 2)){
				print("e");
				speed += 0.5f;
			// その他の小さな場合
			// スピードが、0以上の場合
			}else if(speed >= 0){
				print("f");
				speed += 0.11f;
			}
		}
		*/
		// スピードを表示
		// print(speed);
		
		/**************************************************************************************
		 *
		 *
		 * カメラに関する処理
		 *
		 *
		 **************************************************************************************/
		
		/*
		 * カメラ操作のコード
		 * スピードがアップすると、カメラを遠ざけ、スピードがダウンすると、カメラが近づく
		 */
		/*
		// スピードアップする場合
		if(Input.GetKey("e")){
			// カメラを近づける
			tps_camera.transform.localPosition = new Vector3(tps_camera.transform.localPosition.x,tps_camera.transform.localPosition.y,tps_camera.transform.localPosition.z - 0.1f);
		// スピードダウンする場合
		}else if(Input.GetKey("q")){
			// カメラを遠ざける
			tps_camera.transform.localPosition = new Vector3(tps_camera.transform.localPosition.x,tps_camera.transform.localPosition.y,tps_camera.transform.localPosition.z + 0.1f);
		// どちらもしていない場合
		}else{
			// なにもしないとカメラを遠ざける]
			tps_camera.transform.localPosition = new Vector3(tps_camera.transform.localPosition.x,tps_camera.transform.localPosition.y,tps_camera.transform.localPosition.z + 0.03f);
		}
		*/
		/*
		 * カメラの離れる事についての処理
		 * 最大・最小まで離れると、カメラを止める
		 */
		// 最大以上の場合
		// TPSカメラの、ローカル座標.zを機体のローカル座標.zを引いた数が、
		// -15以下だったら、
		//
		// 機体のローカル座標は必ず(0,0,0)なので、本当は、
		// tps_camera.transform.localPosition.z <= -15と表す事が出来るがコードが見にくくなる為、
		// (tps_camera.transform.localPosition.z - transform.localPosition.z) <= -15とする。
		//
		// 以下同様の理由で意味のない処理をしています。
		/*
		if((tps_camera.transform.localPosition.z - transform.localPosition.z) <= -15){
			// tps_camera.localPosition.zを-15.0fにする。
			tps_camera.transform.localPosition = new Vector3(tps_camera.transform.localPosition.x,tps_camera.transform.localPosition.y,-15.0f);
		//TPSカメラの、ローカル座標.zを機体のローカル座標.zを引いた数が、
		// -5以下だったら、
		}else if((tps_camera.transform.localPosition.z - transform.localPosition.z) <= -5){
			tps_camera.transform.localPosition = new Vector3(tps_camera.transform.localPosition.x,tps_camera.transform.localPosition.y,5.0f);
		}
		*/
		//テストコード
		//print("test");
		
		/*
		 * プレイヤーの移動の処理
		 */
		// playerを移動
		// playerは、1000km出ていると、1座標動く
		transform.Translate(0,0,speed / 1000.0f);		

		//テストコード
		//カメラの移動
		//tps_camera.transform.Translate(0.0f,0.0f,speed);
		
		/*
		 * 移動の処理
		 */
		// 移動に関する処理
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		playercontroll.Pitch(y);
		playercontroll.Roll(x);

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
		// gun_rapid_numが、gunより小さければ、
		if(gun_rapid_num <= gun_rapid){
			// 銃のパーティクルのオブジェクトを生成
			gun_particle.active = true;
			// 
			gun_rapid_num --;
		}
		*/
		// 銃オブジェクトの処理
		/*
		if(gun_particle.active == true){
			gun_particle.active = false;
		}
		*/

		// ロール(ミサイル回避などに使う)β
		if (Input.GetKey("2")){
			transform.Rotate(0,0,-3);
		}
		
		/* 
		 * スピードに関する処理
		 */
		//スピードアップ(e)
		if(Input.GetKey("e")){
			/*
			// 高度によるスピード値(正確にはスピード補正値)に代入
			if(altitude >= speed_down_altitude){
				// (高度 - スピードが下がる高度) : スピードが下がる高度と、高度の差がわかる
				// (高度 - スピードが下がる高度) / 1000 : 計算上の問題
				// (高度 / 1000) : 計算上の問題
				// altitudeを、10000
				// speed_down_altitudeを、7000、
				// 
				// ((10000 / 1000) - ((10000 - 7000) / 1000)) * 0.14f
				// = (10 - 3) * 0.14f
				// = 7 * 0.14f
				// = 0.98f
				Debug.Log("Player.cs : altitude >= speed_down_altitude");
				speed = speed + 1.0f + 0.1f - ((altitude / 1000) - ((speed_down_altitude) / 1000));
			}else{
				Debug.Log("Player.cs :  altitude < speed_down_altitude");
				speed = speed + 1.0f;
			}
			*/
			speed += default_speed_up_down;
		}
		// スピードダウン(q)
		if(Input.GetKey("q")){
			// スピードをダウン
			speed -= default_speed_up_down;
		}
		
		/*
		 * スピード初期化の処理
		 */
		/*
		if(Input.GetKeyDown("y")){
			true_speed = 0.0f;
		}
		*/

		/*
		//テストコード
		//場所とスピードの初期化(r)
		if(Input.GetKeyDown("r")){
			//スピードを0に初期化
			true_speed = 0.0f;
			//playerを0,0,0に場所を初期化
			transform.position = Vector3.zero;
		}
		*/

		/*
		//テストコード
		//Enemyオブジェクトの再生成
		if(Input.GetKeyDown("f")){
			//テストコード
			//Instantiate(enemy[0],new Vector3(0,0,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[1],new Vector3(5,0,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[2],new Vector3(10,0,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[3],new Vector3(0,5,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[4],new Vector3(5,5,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[5],new Vector3(10,5,20),Quaternion.Euler(0,0,0));
			//Instantiate(enemy[7],new Vector3(0,10,20),Quaternion.Euler(0,0,0));
		}
		*/

	}

	void OnGUI()
	{	
				// スピードのログを取る
		// Debug.Log("Player.cs : Speed : " + speed);
		// speed.ToString()のログを取る
		// Debug.Log(speed.ToString());

		//本番コード
		//スピードメーター
		//GUI.Label(new Rect(  5,  0, 400, 50),"true_speed:"+speed.ToString());
		//戦闘時に得た金
		GUI.Label(new Rect(Screen.width - 100, 15, 400, 50),"Gold:" + gold.ToString());
		//敵を倒したときに得られる金
		GUI.Label(new Rect(Screen.width - 100, 30, 400, 50),"EnemyGold:" + enemy_gold.ToString());
		//テストコード・デバッグモード
		//自分の機体の位置
		GUI.Label(new Rect(0, 0, 400, 50),"Position:" + transform.position);

		//テストコード
		//操作説明
		//GUI.Label(new Rect(560,  0, 400, 50),"i:Gun");
		//GUI.Label(new Rect(560, 15, 400, 50),"l:Missaile");
		//GUI.Label(new Rect(560, 30, 400, 50),"w:Down");
		//GUI.Label(new Rect(560, 45, 400, 50),"s:Up");
		//GUI.Label(new Rect(560, 60, 400, 50),"d:Right");
		//GUI.Label(new Rect(560, 75, 400, 50),"a:Left");
		//GUI.Label(new Rect(560, 90, 400, 50),"e:SpeedUp");
		//GUI.Label(new Rect(560,105, 400, 50),"q:SpeedDown");
		//GUI.Label(new Rect(560,120, 400, 50),"space:TPS/FPS");
		//GUI.Label(new Rect(5,210, 400, 50),"i:銃");
		//GUI.Label(new Rect(5,225, 400, 50),"i:銃");
		//GUI.Label(new Rect(5,240, 400, 50),"i:銃");				
	}

	void OnCollisionEnter(Collision col){
		print("On Collision");
		
		// 自機の銃か、ミサイル以外に当たると、
		if(!(col.gameObject.name == "Gun(Clone)" || col.gameObject.name == "Missaile(Clone)")){
			// 自機を破棄
			Destroy(gameObject);
			// 自分が死ぬメッセージを表示
			print("Player Died!!");
		}
	}
}
