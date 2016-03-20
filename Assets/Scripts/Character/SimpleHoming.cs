﻿using UnityEngine;
using System.Collections;

public class SimpleHoming : MonoBehaviour {

    //注目してなくても若干ホーミングするよう
    private GameObject Target;
    private GameObject oldTarget;//ちょっと前のターゲット　基本こいつを追うことにする
    private bool flag_Stop = false;//高さだけにする
    private Vision vision;
    private Attack_Parameter At;//一緒にくっついてるやつ。

    /// 旋回速度
    public float _rotSpeed = 1.0f;

    /// 現在の角度がこれで求まる
    float Direction
    {

        get { return Mathf.Atan2(this.GetComponent<Rigidbody>().velocity.z, this.GetComponent<Rigidbody>().velocity.x) * Mathf.Rad2Deg; }

    }

    // Use this for initialization
    void Start()
    {

        vision = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Vision>();
        At = GetComponent<Attack_Parameter>();
        oldTarget = vision.nearTarget;

    }

    // Update is called once per frame
    void Update()
    {

        Target = vision.nearTarget;

        //注目対象が別のになったらホーミングを切る
        if (oldTarget != Target) this.enabled = false;

        //oldTargetのほうを使う
        if (oldTarget != null)
        {

            Vector3 next = oldTarget.transform.position;
            Vector3 now = transform.position;
            // 目的となる角度を取得する
            var d = next - now;
            var targetAngle = Mathf.Atan2(d.z, d.x) * Mathf.Rad2Deg;
            // 角度差を求める
            var deltaAngle = Mathf.DeltaAngle(Direction, targetAngle);
            var newAngle = Direction;
            if (Mathf.Abs(deltaAngle) < _rotSpeed)
            {

                // 旋回速度を下回る角度差なので何もしない

            }
            else if (deltaAngle > 0)
            {

                // 左回り
                newAngle += _rotSpeed;

            }
            else
            {

                // 右回り
                newAngle -= _rotSpeed;

            }

            // 新しい速度を設定する
            SetVelocity(newAngle, At.speed);

        }
    }

    public void TargetSet(GameObject Target)
    {

        this.Target = Target;

    }

    void SetVelocity(float direction, float speed)
    {

        var vx = Mathf.Cos(Mathf.Deg2Rad * direction) * speed;
        var vz = Mathf.Sin(Mathf.Deg2Rad * direction) * speed;

        if (Mathf.Abs(Target.transform.position.x - transform.position.x) < 0.5f && Mathf.Abs(Target.transform.position.z - transform.position.z) < 0.5f)
        {

            flag_Stop = true;

        }

        //高さは別に設定
        var vy = (Target.transform.position.y - transform.position.y) * speed / (Target.transform.position - transform.position).magnitude;//高さを後に合わせたほうが見栄えがいい

        if (flag_Stop) { vx = 0; vz = 0; flag_Stop = false; }

        this.GetComponent<Rigidbody>().velocity = new Vector3(vx, vy, vz);

    }

}
