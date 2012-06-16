//Enemy(敵)のスクリプト

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	GameObject enemy;
	// Use this for initialization
	void Start () {
		//オブジェクトを削除するために必要なおまじない
		collider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("f")){
			//GameObject変数"bullet"の初期位置を、playerの場所に指定
			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		//テストコード
		print("test1");
		//もし、Missaileか、Gunがあたったならば、
		if(col.gameObject.CompareTag("Missaile") || col.gameObject.CompareTag("Gun")){
			//テストコード
			print("test2");
			//敵を削除
			Destroy(this);
		}
	}
}
