﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SukimaCanvas : MonoBehaviour {

    /*
    ここで表示する枠の管理
    枠ごとに魔法のプリセットを決めとく
    Staticにぶん投げる
        */

    //こっちでシステム全体の管理
    //魔石運びもこっち
    
    private GameObject Manager;
    public GameObject[] SelectPallete = new GameObject[5];//パレットのオブジェクト
    private int currentPallete = 0;//現在選択中のパレット
    private int[] selectMagicID = new int[5];//選んだ魔法をStaticに送るよう

    // Use this for initialization
    void Start () {

        Manager = GameObject.FindGameObjectWithTag("Manager");

        //スキマシステムの準備/////////////////////////////////////////////
        //装備情報の読み込み
        currentPallete = Manager.GetComponent<Static>().currentPallete;
        for(int i = 0;i < selectMagicID.Length;i++)
        {

            selectMagicID[i] = Manager.GetComponent<Static>().SelectMagicID[i];

        }

        //いったん全部消してから付け直す
        for (int i = 0; i < 5; i++)
        {
            SelectPallete[i].SetActive(false);
        }

        SelectPallete[currentPallete - 1].SetActive(true);//一個ずれてる
        
    }
	
    //右ボタン
    public void Right()
    {
        if (currentPallete < 5)
        {

            currentPallete++;

        }
        else
        {

            currentPallete = 1;

        }

        //いったん全部消してから付け直す
        for (int i = 0;i < 5; i++)
        {

            SelectPallete[i].SetActive(false);

        }

        SelectPallete[currentPallete - 1].SetActive(true);//一個ずれてる

    }

    //左ボタン
    public void Left()
    {

        if (currentPallete > 1)
        {

            currentPallete--;

        }
        else
        {

            currentPallete = 4;

        }

        //いったん全部消してから付け直す
        for (int i = 0; i < 5; i++)
        {

            SelectPallete[i].SetActive(false);

        }

        SelectPallete[currentPallete - 1].SetActive(true);//一個ずれてる
    }

    //完了ボタン
    public void Finish()
    {
        //装備情報の書き出し
        Manager.GetComponent<Static>().currentPallete = currentPallete;

        switch (currentPallete)
        {
            case 1:
                selectMagicID[0] = 0;
                selectMagicID[1] = 2;
                selectMagicID[2] = 8;
                selectMagicID[3] = 11;
                selectMagicID[4] = 12;
                break;
            case 2:
                selectMagicID[0] = 0;
                selectMagicID[1] = 1;
                selectMagicID[2] = 3;
                selectMagicID[3] = 13;
                selectMagicID[4] = 9;
                break;
            case 3:
                selectMagicID[0] = 6;
                selectMagicID[1] = 4;
                selectMagicID[2] = 5;
                selectMagicID[3] = 10;
                selectMagicID[4] = 14;
                break;
            case 4:
                selectMagicID[0] = 6;
                selectMagicID[1] = 7;
                selectMagicID[2] = 8;
                selectMagicID[3] = 1;
                selectMagicID[4] = 12;
                break;
            case 5:
                selectMagicID[0] = 7;
                selectMagicID[1] = 2;
                selectMagicID[2] = 13;
                selectMagicID[3] = 9;
                selectMagicID[4] = 4;
                break;
        }

        for (int i = 0; i < selectMagicID.Length; i++)
        {

            Manager.GetComponent<Static>().SelectMagicID[i] = selectMagicID[i];

        }

        Manager.GetComponent<SceneManager>().Home();//ホームへ

    }

}
