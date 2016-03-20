using UnityEngine;
using System.Collections;
using UnityEngine.UI;//uGUI用

public class UI_Blink : MonoBehaviour {

    //テキストを全部表示したことを示すアイコン

    public GameObject Target;//ちかちかさせたいもの
    public float blinkTime;//点滅周期
    private bool isBlink = false;

	// Use this for initialization
	void Start () {

        StartCoroutine(Blink());

    }
	
    void OnEnable()//OnEnable関数　Objectが有効になったときに呼ばれる
    {

        isBlink = false;
        StartCoroutine(Blink());

    }

    IEnumerator Blink()
    {

        if (isBlink) yield break;
        isBlink = true;

        while (true)
        {
            
            Target.GetComponent<Image>().enabled = !Target.GetComponent<Image>().enabled;

            yield return new WaitForSeconds(blinkTime);

        }

    }

}
