using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class Title : MonoBehaviour {
	// 背景のテクスチャ
	Texture2D[] background = new Texture2D[7];

	// テクスチャ管理番号
	float texture_num;

	int touch = 0;

	// 初期化
	void Awake(){
		//テクスチャの初期化
		for(int i = 1; i < 8 ;i++){
			background[i - 1] = (Texture2D)Resources.Load("Image/Title_"+ i);
		}
		// texture_numを
		texture_num = 0;
	}

	// Use this for initialization
	void Start () {
		// ログ
		Debug.Log("Title.cs:Title Scene");

	}

	// Update is called once per frame
	void Update () {
	}
	
	// よくわからないけどここで処理
	void LateUpdate(){
		if(Input.GetMouseButtonDown(0)){
			Debug.Log("Title.cs:Left Click");
			Application.LoadLevel("Select");
		}
	}

	//GUIに関わる処理を行う
	void OnGUI(){
		if(texture_num < 6){
			texture_num +=(float) 3 / 60;
			GUI.DrawTextureWithTexCoords(new Rect(0, 0,Screen.width,Screen.height),background[(int)texture_num],new Rect(0,0,1,1));
		}else{
			GUI.DrawTextureWithTexCoords(new Rect(0, 0,Screen.width,Screen.height),background[6],new Rect(0,0,1,1));
			GUI.Label(new Rect(0,0,Screen.width / 6 * 4,Screen.height / 5 / 5 * 3),(Texture2D)Resources.Load("Image/Title"));
			GUI.Label(new Rect(0,Screen.height / 5 / 5 * 3,Screen.width / 6 * 4,Screen.height / 5 / 5 * 2),(Texture2D)Resources.Load("Image/SubTitle"));										
			if (touch <= 60){
				GUI.Label(new Rect(Screen.width / 8 * 3, Screen.height / 6 * 4,Screen.width,Screen.height),"Please Touch to Start.");					
			}else if(touch >= 120){
				touch = 0;
			}
			touch++;
		}

	}
}

