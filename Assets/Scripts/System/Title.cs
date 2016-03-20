using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

    // タイトル画面でのシステム

    private GameObject Manager;
    public GameObject[] Button;

	// Use this for initialization
	void Start () {

        Manager = GameObject.FindGameObjectWithTag("Manager");
        
        if (Manager.GetComponent<Static>().count_Start == 0)
        {

            Button[0].SetActive(true);

        }
        else
        {

            Button[1].SetActive(true);

        }

	}

    //はじめから
    public void First()
    {

        //セーブデータを初期化
        Manager.GetComponent<Static>().day = 1;
        Manager.GetComponent<Static>().count_Start = 0;
        Manager.GetComponent<Static>().H_Point = 100;
        Manager.GetComponent<Static>().M_Point = 100;
        Manager.GetComponent<Static>().lank_P = 0;
        Manager.GetComponent<Static>().bonus_P = 0;

        Manager.GetComponent<SceneManager>().GameStart();//ホームへ

    }

    //続きから
    public void Continue()
    {

        Manager.GetComponent<SceneManager>().GameStart();//ホームへ

    }

}
