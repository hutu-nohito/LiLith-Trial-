﻿using UnityEngine;
using System.Collections;

public class Debug_pcZ : Character_Manager
{

    /*
    プレイヤーの操作用
    Jump()//ジャンプ
    Move()//移動

    */

    //使うもの
    private CharacterController playerController;//キャラクタコントローラで動かす場合
    private Animator animator;//アニメーション設定用
    private Static save;

    //初期パラメタ(邪魔なのでインスペクタに表示しない)
    [System.NonSerialized]
    public int max_HP, max_MP, base_Pow, base_Def;
    [System.NonSerialized]
    public float base_Sp, base_Ju;

    //坂判定用
    private RaycastHit slideHit;
    public float slideSpeed = 0.1f;//滑るスピード
    private bool isSliding = false;//滑ってるかどうか

    void Start()
    {
        playerController = GetComponent<CharacterController>();//rigidbodyを使う場合は外す
        animator = GetComponentInChildren<Animator>();//アニメータを使うとき
        save = GameObject.FindGameObjectWithTag("Manager").GetComponent<Static>();

        //HPとMPの引継ぎ
        H_point = save.H_Point;
        M_point = save.M_Point;

        //初期パラメタを保存
        max_HP = H_point;
        max_MP = M_point;
        base_Pow = power;
        base_Def = def;
        base_Sp = speed;
        base_Ju = jump;

    }

    void Update()
    {
        //HPがなくなった時の処理
        if (H_point <= 0)
        {

            Application.LoadLevel(Application.loadedLevel);

        }

        //アニメーションリセット///////////////////////////////////////////////////////////
        move_direction = new Vector3(0.0f, move_direction.y, 0.0f);
        //キーボードからの入力読み込み/////////////////////////////////////////////////////

        float InputX = 0.0f;
        float InputY = 0.0f;

        Vector3 inputDirection = Vector3.zero;//入力された方向

        if (flag_move)
        {

            InputX = Input.GetAxis("Horizontal");
            InputY = Input.GetAxis("Vertical");
            inputDirection = new Vector3(InputX, 0, InputY);//入力された方向

        }

        //ジャンプ
        if (playerController.isGrounded)
        {
            flag_jump = true;
            move_direction = Vector3.zero;
            animator.SetBool("Jump", false);
            if (Input.GetButtonDown("Jump"))
            {
                if (flag_jump) { Jump(); }

            }

        }

        //キャラクタの方向回転
        //this.transform.Rotate(0,Input.GetAxis("Mouse X") * character_parameter.now_speed * Time.deltaTime * 90,0);

        if (!GetF_Watch()) this.transform.Rotate(0, InputX * speed * Time.deltaTime * 12, 0);//注目してたら回さない

        //キャラクタの方向を取得
        direction = transform.TransformDirection(Vector3.forward);

        //キャラクタ移動処理
        if (inputDirection.magnitude > 0.1)
        {

            _Move();
            animator.SetFloat("Speed", move_direction.magnitude);

        }
        else
        {

            animator.SetFloat("Speed", 0);

        }

        //坂に立ってたら滑らす
        if (isSliding)
        {//滑るフラグが立ってたら
            Vector3 hitNormal = slideHit.normal;
            move_direction.x = hitNormal.x * slideSpeed;
            move_direction.z = hitNormal.z * slideSpeed;
            isSliding = false;//ここでリセットしとく
        }

        //キャラにかかる重力決定（少しふわふわさせてる）
        if (move_direction.y > -2)
        {


            move_direction.y += Physics.gravity.y * Time.deltaTime;



        }

        //キャラの移動実行（動かす方向＊動かすスピード＊補正）
        playerController.Move(move_direction * speed * Time.deltaTime);

    }

    //ジャンプ
    void Jump()
    {
        flag_jump = false;
        move_direction.y = jump;
        animator.SetBool("Jump", true);

    }

    //実際に動かす方向決定
    void _Move()
    {

        if (Input.GetKey(KeyCode.W))
        {
            move_direction.x += direction.x;
            move_direction.z += direction.z;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move_direction.x -= direction.x / 2;
            move_direction.z -= direction.z / 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move_direction.x -= direction.z / 10;
            move_direction.z += direction.x / 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move_direction.x += direction.z / 10;
            move_direction.z -= direction.x / 10;
        }
    }

    //地面に立ってないとき下が坂じゃないか確認
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!playerController.isGrounded)
        {
            //キャラクターの位置から下方向にRayを飛ばす（指定レイヤー限定※この場合は地面コリジョンのレイヤー）
            //RayLengthは、Rayを飛ばす距離。私の場合は地面の位置すれすれまで飛ばしてます（地面の高さは固定な前提）
            //レイヤーマスクは⇒で指定 int layerMask =1 << LayerMask.NameToLayer("レイヤー名");
            if (Physics.Raycast(transform.position, Vector3.down, out slideHit, 50))
            {
                //衝突した際の面の角度とが滑らせたい角度以上かどうかを調べます。
                if (Vector3.Angle(slideHit.normal, Vector3.up) > playerController.slopeLimit)
                {
                    isSliding = true;
                }
            }
        }
    }
}