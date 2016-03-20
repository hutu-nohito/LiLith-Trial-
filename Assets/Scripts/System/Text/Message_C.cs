using UnityEngine;
using System.Collections;

public class Message_C : MonoBehaviour {

    //メッセージを表示するかどうか

	private GameObject Player;
	public GameObject Message;
	public GameObject Panel;
	private Text_C text_C; 

	//パラメタ//
	public int MaxDisplay = 1023;
	private int PlayCounter = 0;
	private int i = 0;
	
	//flag//	
	private bool flag_display = false;//メッセージウィンドウを表示するかどうか
	public bool flag_Sign = true;//看板にするかどうか
	
	// Use this for initialization
	void Start ()
    {

		text_C = Message.GetComponent<Text_C>();
		Player = GameObject.Find("Player");

		if(!flag_Sign)
        {
			
			MaxDisplay = 1;
			
		}
	}
	
	void Update ()
    {
		
		if (flag_display)
        {
            //文字送り
            if (Input.GetMouseButtonDown (0))
            {
				
				i++;

				if (i >= text_C.text.Length)
                {

					Panel.SetActive(false);
					Player.SendMessage ("Key");
					flag_display = false;
					i = 0;
				}
			}
		}
		
	}
	
	void OnTriggerStay (Collider col)
    {

		if (col.tag == "Player")
        {
			if (Input.GetMouseButtonDown (0))
            {
				if (!flag_display)
                {
					if (PlayCounter < MaxDisplay)
                    {

						Panel.SetActive(true);
						Player.SendMessage ("Key");
						flag_display = true;
						PlayCounter++;
						
					}
				}
			}
		}
	}
	
	void Display ()
    {
		
		if (PlayCounter < MaxDisplay)
        {

			Player.SendMessage ("Key");
			flag_display = true;
			PlayCounter++;
			i = 1;

		}
		
	}

}
