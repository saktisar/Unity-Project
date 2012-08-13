using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	// 背景
	Texture2D background;

	// 初期化処理
	void Awake(){
		// テクスチャの初期化
		background = (Texture2D)Resources.Load("Image/Menu");
	}

	// はじめにする処理
	void Start () {
		Debug.Log("Menu.cs:Menu Scene");
	}
	
	// 繰り返し処理
	void Update () {
	}

	// GUI関係の処理
	void OnGUI(){
		//ボタン処理
		if(GUI.Button(new Rect(0,(float)(Screen.height/13.5*2.3),(float)(Screen.width / 20.3 * 9.3),(float)(Screen.height / 13.5 * 1)),"")){
			Application.LoadLevel("Game16");
		}else if(GUI.Button(new Rect(0,(float)(Screen.height/13.5*4.3),(float)(Screen.width / 20.3 * 9.3),(float)(Screen.height / 13.5 * 1)),"")){
			Debug.Log("Menu.cs:Free Mission");
		}
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),background);
	}
}
