using UnityEngine;
using System.Collections;

public class PlayerRockon : MonoBehaviour {		// Use this for initialization
	[SerializeField]	
	/*
	 * 敵に関する変数
	 */

	/*
	 * カメラに関する変数
	 */
	//TPSカメラをいれる変数
	GameObject tps_camera;
	//FPSカメラを入れる変数
	GameObject fps_camera;

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

	/*
	 * テクスチャに関する変数
	 */
	//ミサイルの照準を代入
	Texture2D mark;
	// 敵につくマークを作成
	Texture2D enemy_mark;
	//ロックオン時敵につくマーク
	Texture2D rockon;


	void Awake(){
		/*
		 * カメラを代入
		 */
		//tpsカメラを探す
		tps_camera = GameObject.Find("TPS Camera");
		//fpsカメラを探す
		fps_camera = GameObject.Find("FPS Camera");

		// ミサイルの照準をロード
		mark = (Texture2D)Resources.Load("Image/Sighting");
		// 敵につくマークをロード
		enemy_mark = (Texture2D)Resources.Load("Image/EnemyMark");
		// ロックオンマークをロード
		rockon = (Texture2D)Resources.Load("Image/Rockon");
	}
	void Start () {
		/*
		 * tpsカメラを有効化して、fpsカメラを無効化
		 */
		//tpsカメラを有効化
		tps_camera.camera.enabled = true;
		//fpsカメラを無効化
		fps_camera.camera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
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

		/*
		 * ロックオンに関する処理
		 */
		//ロックオン切り替え
		if(Input.GetKeyDown("f")){
			// ロックオンする対象がいるのかの判定
			// false:いない
			bool rockon_enemys = false;

			//ループ　
			for(int i = 0;i < Player.enemy_num;i++){
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
					Player.i_num = i;
					break;
				}
			}

			// 配列が最後に来たので
			if(!rockon_enemys){
				//テストコード
				//Debug.Log("Player.cs:in f Key(if)");
				// ループしてロックオン可能な物体を探す
				for(int i = 0;i < Player.enemy_num;i++){
					// ロックオン可能ならば
					if(enemy_can_rockon[i]){
						//ロックオン可能にする。
						enemy_rockon[i] = true;
						Player.i_num = i;
						break;
					}

				}
			}
		}
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
	}

	void OnGUI(){
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
		for(int i = 0; i < Player.enemy_num;i++){
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
						coordinate_enemy[i] = tps_camera.camera.WorldToScreenPoint(Player.enemy[i].transform.position);
					}catch{
						break;
					}
				}else{
					try{
						// fpsカメラ上のスクリーン座標を取得する
						coordinate_enemy[i] = fps_camera.camera.WorldToScreenPoint(Player.enemy[i].transform.position);
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
		 		between_distance[i] = Vector3.Distance(transform.position,Player.enemy[i].transform.position);
				
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
		for(int i = 0;i <= Player.enemy_num;i++){
			// 4回目に、
			if(i == Player.enemy_num){
				// ロックオンできる物体を探し
				for(int j = 0;j < Player.enemy_num;j++){
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
		Player.speed = float.Parse(GUI.TextField(new Rect(Screen.width - 50, 0, 200, 20),Player.speed.ToString(),7));
	}
}
