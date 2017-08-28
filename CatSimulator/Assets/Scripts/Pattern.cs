using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {
    public int patternnum;
    GameObject EventSystem;

	// Use this for initialization
	void Start () {
        EventSystem = GameObject.FindGameObjectWithTag("EventSystem");
        //다음씬으로 넘어가도 오브젝트 유지하게 해줌
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update () {
    }

    public void SelectPattern()
    {
        patternnum = EventSystem.GetComponent<SubTitle>().patternNum;
    }
}
