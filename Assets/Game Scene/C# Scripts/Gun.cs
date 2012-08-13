using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	int count;
	void Awake(){
		count = 0;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		count++;
		transform.Translate(0.0f,-0.02f,2.0f);
		if(count > 600){
			//ミサイルを破壊する
			Destroy(gameObject);
		}

	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "Enemy(Clone)"){
			Destroy(gameObject);
		}
	}

}
