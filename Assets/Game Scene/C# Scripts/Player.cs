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
//
// スピードの加減値は存在しない(スピードの加減は関数で行う)
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour{
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
	 * カメラに関する変数
	 */
	//TPSカメラをいれる変数
	GameObject tps_camera;
	//FPSカメラを入れる変数
	GameObject fps_camera;

	/*
	 * 敵に関する変数
	 */
	//enemyオブジェクトを入れる変数
	public static GameObject[] enemy = new GameObject[8];
	// 敵の数を保持する
	public static int enemy_num;

	/*
	 * 高度に関する処理
	 *
	 */
	// 高度
	int altitude;
	// 最大高度 - ストール(自機が落ちる)する値の設定
	int altitude_maximum;
	// スピードを下げる高度
	int speed_down_altitude;
	//
	// 最小高度 - いらない、最小高度は、0だから
	// float altitude_minimum;

	/*
	 * スピードに関する変数
	 */
	// 総合スピード値
	float speed = 600.0f;
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
	
	/*
	 * 敵の金に関する変数
	 */
	//戦闘時に得た金を管理する変数
	int gold = 0;
	//敵を倒した時に得られる金の変数
	int enemy_gold = 0;

	/*
	 * テクスチャに関する変数
	 */
	//ミサイルの照準を代入
	Texture2D mark;
	// 敵につくマークを作成
	Texture2D enemy_mark;
	//ロックオン時敵につくマーク
	Texture2D rockon;

	/*
	 * 座標に関する変数
	 */
	// 追加
	// 相手のスクリーン座標
	Vector3[] coordinate_enemy = new Vector3[8];
	// 敵オブジェクトスクリーン座標バックアップ
	Vector3[] coordinate_enemy_backup = new Vector3[8];
	// 自分と相手の距離
	float[] between_distance = new float[8];
	// プレイヤーがどちらの方向を向いているか
	// + = true
	// - = false
	// 座標を求めるのに使用する
	bool player_direction;
	// 可視状態の敵を判別する
	bool[] enemy_can_display = new bool[8];
	// ロックオンできる物体を代入
	bool[] enemy_can_rockon = new bool[8];
	// ロックオンしている物体を代入
	bool[] enemy_rockon = new bool[8];
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
		// ミサイルの照準をロード
		mark = (Texture2D)Resources.Load("Image/Sighting");
		// 敵につくマークをロード
		enemy_mark = (Texture2D)Resources.Load("Image/EnemyMark");
		// ロックオンマークをロード
		rockon = (Texture2D)Resources.Load("Image/Rockon");
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
		
		/*
		 * カメラを代入
		 */
		//tpsカメラを探す
		tps_camera = GameObject.Find("TPS Camera");
		//fpsカメラを探す
		fps_camera = GameObject.Find("FPS Camera");
	
		/* gun,missaile,enemyオブジェクトは、Unityではりつけ。*/
		
		/*
		 * スピードに関する変数
		 */
		//テストコード
		//スピードを0に初期化
		// true_speed = 0.0f;

		/*
		 * 画面上の敵の初期化
		 */
		// 敵ベクトルバックアップ初期化
		for(int i = 0;i < 8;i++){
			// 初期化
			// 敵のスクリーン座標
		 	coordinate_enemy[i] = new Vector3(0.0f,0.0f,0.0f);
			// 非可視状態で、初期化
			enemy_can_display[i] = false;
			// ロックオンできないで、固定
			enemy_can_rockon[i] = false;
		}

		// ロックオンできる数
		// can_rockon = 1;

		// ロックオン
		// look_num = 0;

		// 敵の数は４体です。
		enemy_num = 4;
		
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
		speed_down_altitude = 7000;
		
		// gun_particleを無効化
		gun_particle.active = false;
	}

	void Start()
	{
		/*
		 * tpsカメラを有効化して、fpsカメラを無効化
		 */
		//tpsカメラを有効化
		tps_camera.camera.enabled = true;
		//fpsカメラを無効化
		fps_camera.camera.enabled = false;

		// 照準をリサイズ
		// mark.Resize(128,128);
		// mark.Apply();
	
		// テストコード
		// playerを0,0,0に場所を初期化
		// transform.position = Vector3.zero;
		
		// スピード
		speed = 600.0f;
	}

	void Update()
	{	
		/*
		 *
		 * 高度に関する処理
		 *
		 */
		// 高度の決定
		altitude = ((int)transform.position.y) * 100;

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

		/*
		 * 最大スピードと最小スピードを制御する処理
		 */
		//スピードが最大以上ならば、
		if(speed >= speed_maximum){
			// 最大にする
			speed = speed_maximum;
		}

		// スピードが最小以下ならば、
		if(speed < speed_minimum){
			// 最小に固定する
			speed = speed_minimum;
		}

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
		/*
		 * 移動の処理
		 */
		//上移動(w)
		if(Input.GetKey("w")){
			//上へ移動
			//transform.Translate(0.0f,1.0f,0.0f);
			//上へ少し回転β
			transform.Rotate( 1, 0, 0);
			camera_height += 0.01f;
		
		}
		//下移動(s)
		if(Input.GetKey("s")){
			//下へ移動
			//transform.Translate(0.0f,-1.0f,0.0f);
			//下に少し回転β
			transform.Rotate( -1, 0, 0);
			camera_height += 0.01f;			
		}
		//右移動(d)
		if(Input.GetKey("d")){
			//右へ移動
			transform.Translate(1.0f,0.0f,0.0f);
			//右に少し回転β
			transform.Rotate( 0, 0, 1);
			camera_width -= 0.01f;
		}
		//左移動(a)
		if(Input.GetKey("a")){
			//左へ移動
			transform.Translate(-1.0f,0.0f,0.0f);
			//左に少し回転β
			transform.Rotate( 0, 0, -1);	
			camera_width += 0.01f;
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
			speed = speed + 1.0f;
		}
		// スピードダウン(q)
		if(Input.GetKey("q")){
			// スピードをダウン
			speed = speed - 1.0f;
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
		 * カメラの切り替えに関する処理
		 */
		//カメラの変更(Space)
		if(Input.GetKeyDown(KeyCode.Space)){
			//tps_cameraがONならば、
			if(tps_camera.camera.enabled == true){
				//tpsカメラをOFF、
				tps_camera.camera.enabled = false;
				//fpsカメラをONにする。
				fps_camera.camera.enabled = true;
			//tps_cameraがOFFならば、
			}else if(tps_camera.camera.enabled == false){
				//tpsカメラをON、
				tps_camera.camera.enabled = true;
				//fpsカメラをOFFにする。
				fps_camera.camera.enabled = false;
			}
		}

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

		/*
		 * ロックオンに関する処理
		 */
		//ロックオン切り替え
		if(Input.GetKeyDown("f")){
			// ロックオンする対象がいるのかの判定
			// false:いない
			bool rockon_enemys = false;

			//ループ　
			for(int i = 0;i < enemy_num;i++){
				//ロックオンした対象を検索
				if(enemy_rockon[i]){
					// ロックオンを解除
					enemy_rockon[i] = false;
					// ループして、enemy_can_rockon == trueを探す
					for(int j = (i+1);j < 4; j++){
						// enemy_can_rockonがtrueなら
						if(enemy_can_rockon[j]){
							//enemy_rockon[i+1(j)]をtrueにする
							enemy_rockon[j] = true;
							// テストコード
							// Debug.Log("Player.cs:in f Key(for)");
							// Debug.Log(j);
							break;
						}
					}
					//対象がいたので真にする
					rockon_enemys = true;
					i_num = i;
					break;
				}
			}

			// 配列が最後に来たので
			if(!rockon_enemys){
				//テストコード
				//Debug.Log("Player.cs:in f Key(if)");
				// ループしてロックオン可能な物体を探す
				for(int i = 0;i < enemy_num;i++){
					// ロックオン可能ならば
					if(enemy_can_rockon[i]){
						//ロックオン可能にする。
						enemy_rockon[i] = true;
						i_num = i;
						break;
					}

				}
			}
		}
	}

	void OnGUI()
	{	
		/*
		 * ロックオンに関する処理
		 */
		//+方向に傾いている場合
		if(transform.rotation.x >= 270 && transform.rotation.x <= 90){
			player_direction = true;
		//-方向に傾いている場合
		}else{
			player_direction = false;
		}
		//敵機につくマークとロックオンに関するコード
		for(int i = 0; i < enemy_num;i++){
			// カメラ内オブジェクトの座標に関する処理
			// 自分と敵の座標の差を代入
			// WorldToScre1enPoint:スクリーン座標を受け取る。
			// ここで、2時間
			// スクリーン上の座標 = fpsカメラから見たenemy[i]の場所を算出する。

			// ※変更
			//Debug.Log("Enemy = "+ enemy[i].transform.position);
			//Debug.Log("WorldToScreenPoint =" + fps_camera.camera.WorldToScreenPoint(enemy[i].transform.position));
		
				// tpsカメラがオンならば
				if(tps_camera.camera.enabled == true){
					try{
						// tpsカメラ上のスクリーン座標を取得する
						coordinate_enemy[i] = tps_camera.camera.WorldToScreenPoint(enemy[i].transform.position);
					}catch{
						break;
					}
				}else{
					try{
						// fpsカメラ上のスクリーン座標を取得する
						coordinate_enemy[i] = fps_camera.camera.WorldToScreenPoint(enemy[i].transform.position);
					}catch{
						break;
					}
				}

				//GUI.Label(new Rect(coordinate_enemy[i].x, coordinate_enemy[i].y, 200, 50),coordinate_enemy[i].x + "," + coordinate_enemy[i].y); 
		
				//ロックオンマークを合わせる為、0.5マイナス(仮)
				//coordinate_enemy[i].x -= 23.7f; 
				//coordinate_enemy[i].y -= 10.5f;
				//一応
				//between_distance_coordinate[i].z -= 0.5f;
				//coordintate_enemy[i] = Vector3.right * 180;

				// 距離を求める
		 		between_distance[i] = Vector3.Distance(transform.position,enemy[i].transform.position);
				
				//	
				//
				//
				// Danger：このコードは理解していません。
				// 	 ：大変危険なコードです。なぜか動きました。
				// 	 ：追記　バグりました。
				// 	 ：追記　なおりました。
				//

				// playerが+の方向に向かっている場合
				if(player_direction){
					// z軸の差が座標が大きくなっている場合
					if(coordinate_enemy_backup[i].z >= coordinate_enemy[i].z){
						//視認可能
						enemy_can_display[i] = true;
					}else{
						enemy_can_display[i] = false;
					}
				//playerが-の方向に向かっている場合
				}else{
					// z軸の差が座標が小さくなっている場合
					if(coordinate_enemy_backup[i].z <= coordinate_enemy[i].z){
						// 視認可能
						enemy_can_display[i] = true;
					}else{
						enemy_can_display[i] = false;
					}
				}

				//仮の数値
				//enemyを視認できるかつ距離が10000(仮)以下の場合
				if(enemy_can_display[i] && (between_distance[i] <= 10000)){
					enemy_can_rockon[i] = true;
				}else{
					enemy_can_rockon[i] = false;
				}
	
				// 敵がロックオンできる＆ロックオンマークがついている場合
				if(enemy_can_display[i] && enemy_rockon[i]){
					// なぜかこれで出来た
					// ロックオンマークを描画
					GUI.DrawTextureWithTexCoords(new Rect(coordinate_enemy[i].x - 25,Screen.height - coordinate_enemy[i].y - 25,50,50),rockon,new Rect(0,0,1,1));
					// 速度を描画
					GUI.Label(new Rect(coordinate_enemy[i].x + 15,Screen.height - coordinate_enemy[i].y - 25,
								((int)between_distance[i]).ToString().Length * 10.0f,20.0f),((int)between_distance[i]).ToString());
				//敵がロックオンできる場合
				}else if(enemy_can_display[i]){
					// なぜかこれで出来た
					// 敵機にマークを描画
					GUI.DrawTextureWithTexCoords(new Rect(coordinate_enemy[i].x - 25,Screen.height - coordinate_enemy[i].y - 25,50,50),enemy_mark,new Rect(0,0,1,1));
					// 速度を描画
					GUI.Label(new Rect(coordinate_enemy[i].x + 15,Screen.height - coordinate_enemy[i].y - 25,
								((int)between_distance[i]).ToString().Length * 10.0f,20.0f),((int)between_distance[i]).ToString());
				}		
			// わけわからん
			//coordinate_enemy_backup[i] = coordinate_enemy[i];
			//GUI.DrawTextureWithTexCoords(new Rect(coordinate_enemy[i].x - 25,Screen.height - coordinate_enemy[i].y - 25,50,50),enemy_mark,new Rect(0,0,1,1));
		}
		// ループ
		for(int i = 0;i <= enemy_num;i++){
			// 4回目に、
			if(i == enemy_num){
				// ロックオンできる物体を探し
				for(int j = 0;j < enemy_num;j++){
					// ロックオンできる物体が見つかれば、
					if(enemy_can_rockon[j]){
						// ロックオン
						enemy_rockon[j] = true;
						break;
					}
				}
				break;
			}

			//ロックオンした対象を検索
			if(enemy_rockon[i]){
				break;
			}
		}

		/*
		 * GUI描画に関する処理
		 */
		//照準を描画
		GUI.DrawTextureWithTexCoords(new Rect(Screen.width / 2 - 128 / 2,Screen.height / 2 - 128 / 2 ,128,128),mark,new Rect(0,0,1,1));
		//テストコード
		// スピードに関する処理
		GUI.Label(new Rect(Screen.width - 100,0, 400, 50),"Speed:");
		// speed = 600.0f;
		speed = float.Parse(GUI.TextField(new Rect(Screen.width - 50, 0, 200, 20),speed.ToString(),7));
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
