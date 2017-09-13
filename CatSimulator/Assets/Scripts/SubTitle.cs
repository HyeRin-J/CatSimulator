using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubTitle : MonoBehaviour {
    public GameObject Cat;
    public int patternNum = 1;
    GameObject pattern;
    float startTime, nextTime;
    public Animator animator;

    // Use this for initialization
    void Start () {
        Cat.GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternNum) as Texture2D;
        pattern = GameObject.FindGameObjectWithTag("Pattern");
        startTime = Time.time;
        animator = GameObject.Find("cu_cat2_model").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        nextTime = Time.time;
        if (nextTime - startTime > 5.0f)
        {
            animator.SetBool("Sleep", true);
        }
        else
        {
            animator.SetBool("Sleep", false);
        }
	}

    //버튼
    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        if (GUI.Button(new Rect(w - 300, h * 1.1f, 120, 50), "<"))
        {
            PrevPattern();
            startTime = Time.time;
        }
        if (GUI.Button(new Rect(w + 100, h * 1.1f, 120, 50), ">"))
        {
            NextPattern();
            startTime = Time.time;
        }
        if (GUI.Button(new Rect(w - 100, h * 1.4f, 120, 50), "OK"))
        {
            LoadScene();
        }
        Cat.GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternNum) as Texture2D;
        pattern.GetComponent<Pattern>().SelectPattern();
    }
    //털 패턴
    public void NextPattern()
    {
        patternNum++;
        if (patternNum >= 16) patternNum = 1;
    }

    public void PrevPattern()
    {
        patternNum--;
        if (patternNum <= 0) patternNum = 15;
    }
    //다음 씬 로딩
    public void LoadScene()
    {
        SceneManager.LoadScene("Main");
    }
}
