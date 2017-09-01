using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour {

    GameObject[] button;
    public float RandValue = 0;
    int patternnum = 1;
    public Slider friendlyslider;
    public Slider statusslider;
    public GameObject Cat;
    GameObject Pattern;
    GameObject food;
    Cat catScript;
    Animator anim;

    void RandomValue()
    {
        RandValue = Random.Range(0, 101);
    }

    // Use this for initialization
    void Start () {
        //Pattern = GameObject.FindGameObjectWithTag("Pattern");
        //털 패턴 적용
        //patternnum = Pattern.GetComponent<Pattern>().patternnum;
        GameObject.Find("cu_cat2_mesh").GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
        catScript = Cat.GetComponent<Cat>();
        anim = Cat.GetComponent<Animator>();
        button = GameObject.FindGameObjectsWithTag("Button");

        for(int i = 0; i < 6; i++)
        {
            Button btn = button[i].GetComponent<Button>();
            int code = i;
            btn.onClick.AddListener(() => PressButton(code));
        }
    }

    // Update is called once per frame
    void Update () {
        friendlyslider.value = catScript.friendly;
        statusslider.value = catScript.status;
    }
    
    public void PressButton(int Code)
    {
        Debug.Log(button[Code].name);
    }
}
