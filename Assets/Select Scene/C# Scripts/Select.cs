using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class Select : MonoBehaviour {
	
	Vector2 scrollPosition;

	bool exist_files = true;		
	// files配列にMission以下の代入	
	string[] files = new string[2];

	void Awake(){
		// print(System.IO.Directory.GetFiles(@"\Mission").Length);
		try{
			// Array.Resizeで、files変数の大きさをSystem.IO.Directory.GetFilesの大きさに編集
			Array.Resize(ref files,System.IO.Directory.GetFiles(@"\Mission").Length);		
			files = System.IO.Directory.GetFiles(@"\Mission");
		}catch(DirectoryNotFoundException){
			Environment.Exit(0);
			// exist_files = false;
		}catch{
			exist_files = false;
		}
	}

	// Use this for initialization
	void Start () {
		/*
		// もし、Missionフォルダがなければ、
		if(!Directory.Exists("Mission")){
			// Missionディレクトリを作成
			Directory.CreateDirectory("Mission");
			// 「ファイルが存在しません」を表すFalseを代入
			exist_files = false;
		// もし、フォルダがあれば、
		}
		*/
	}
	
	// Update is called once per frame
	void Update (){
	}

	void OnGUI(){
		if(exist_files == true){
			// リストの始まりを表す。
			GUILayout.BeginScrollView(new Vector2(0,0),GUILayout.Width(150), GUILayout.Height(150));
 
			for(int i = 0; i < files.Length; i++){
				// ラベル
    		GUILayout.Label(files[i]);
			}

			// リストの終わり
    	GUILayout.EndScrollView();			
		}else{
			print("Select.cs : exist_files == false");
			GUILayout.Label("Select.cs : exist_files == false");
		}
	}
}
