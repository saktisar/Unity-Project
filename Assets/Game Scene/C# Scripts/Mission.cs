using UnityEngine;
using System.IO;
using System.Collections;

public class Mission : MonoBehaviour {
	// ファイルのディレクトリ
	string file = @"\Mission\test.txt";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < Player.enemy_num;i++){
			if(Player.enemy[i] == null){
				for(int j = (i + 1) ; j < Player.enemy_num ; j++){
					Player.enemy[j-1] = Player.enemy[j];
				}
				Player.enemy_num -= 1;
			}
		}
	}
}
