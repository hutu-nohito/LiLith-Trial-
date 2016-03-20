﻿using UnityEngine;
using System.Collections;

public class Z_Camera : MonoBehaviour {

    /*
    カメラ側の機能
    */
    
    public float RotSpeed = 20;//注目時の回転移動速度の調整用
    public float tiltAngle = 2;//カメラティルト調整用の角度
    public float coolTime = 0.0f;//注目できないようにする時間

    public float length;//注目してる敵との距離
	public GameObject Target;//ここに注目対象を格納すればいい
	private GameObject Player;
	private Player_ControllerZ pcZ;
    public GameObject nearMarker;//今注目できる敵のマーカ
    public GameObject targetMarker;//今注目してる敵のマーカ

    //演出
    //マーカを上下に動かす
    public Vector3 QuakeMagnitude = new Vector3(0, 1, 0);
    private Vector3 Offset = Vector3.zero;
    private float quaketime = 0;
    private float elapsedquakeTime = 1.0f;
    
    // Use this for initialization
    void Start ()
    {
		
		Player = GameObject.FindGameObjectWithTag("Player");
        pcZ = Player.GetComponent<Player_ControllerZ>();
        nearMarker = GameObject.Find("nearMarker");
        targetMarker = GameObject.Find("targetMarker");

        //初期化
        nearMarker.SetActive(false);
        targetMarker.SetActive(false);
        QuakeMagnitude /= 1000;//正規化
		
	}

	//ちらちらしそうだったらLateUpdateに変更
	// Update is called once per frame
	void LateUpdate ()
    {
        
        //とりあえず右クリックで注目
		if(Input.GetMouseButton(1))
        {
            //ターゲットがいたら
            if (Target != null)
            { 

                pcZ.Set_Watch();//注目している

                //Playerの方向 = (最初の方向,向けたい方向,向けたい速度)
                Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, Quaternion.LookRotation(Target.transform.position - Player.transform.position), 0.1f);//Playerをターゲットのほうにゆっくり向ける
                Player.transform.rotation = Quaternion.Euler(0, Player.transform.eulerAngles.y, 0);//Playerのx,zの回転を直す。回転嫌い。全部Eulerにしてしまえばよい
                
                //注目時のマーカー
                targetMarker.SetActive(true);
                targetMarker.transform.position = Target.transform.position + new Vector3(0, Target.transform.localScale.y + 1, 0) + Offset;
                nearMarker.SetActive(false);

            }
            else
            {

                targetMarker.SetActive(false);

            }
        }
        else
        {

            targetMarker.SetActive(false);

        }
        
        if (Input.GetMouseButtonUp(1)){

            pcZ.Release_Watch();//注目解除

		}

        //ターゲットがいなければ黄色いマーカを出さない
        if(Target == null)nearMarker.SetActive(false);

        //coolTimeが0.2以下の時注目できないようにする
        if (!pcZ.GetF_Watch()) {

            if(coolTime < 0.2f)
            coolTime += Time.deltaTime;//注目してなかったら足しとく

        }
        else
        {

            coolTime = 0.0f;

        }

        //揺らすよう
        Offset += QuakeMagnitude;
        quaketime += Time.deltaTime;
        if (quaketime > elapsedquakeTime)
        {

            QuakeMagnitude = -QuakeMagnitude;
            quaketime = 0;

        }
    }

    public void SetTarget(GameObject Target,float near)
    {

        if (Target == null)
        { 
            
            coolTime = 0.0f;
        
        }
        //nullのときは注目を外す
        else if (!pcZ.GetF_Watch())
        {
            //注目中はターゲットは変えない
            if (coolTime > 0.2f)
            {

                this.Target = Target;
                nearMarker.SetActive(true);
                nearMarker.transform.position = Target.transform.position + new Vector3(0, Target.transform.localScale.y + 1, 0) + Offset;

            }
            
        }

        length = near;
        if (length >= 20)
        {

            this.Target = null;
            pcZ.Release_Watch();//注目解除

        }
    }

}
