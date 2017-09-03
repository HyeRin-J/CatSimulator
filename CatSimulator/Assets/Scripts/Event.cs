using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour {

    GameObject[] button;
    int patternnum = 1;
    public Slider friendlyslider;
    public Slider statusslider;
    public GameObject Cat;
    GameObject Pattern;
    GameObject food;
    Cat catScript;
    Animator anim;
    int rand;

    int RandomValue()
    {
        int RandValue = Random.Range(0, 101);

        return RandValue;
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
        
        if(Code == 0)
        {
            
        }
        else if(Code == 1)
        {

        }
        else if(Code == 2)
        {

        }
        else if(Code == 3)
        {

        }
        else if(Code == 4)
        {


        }
        else if(Code == 5)
        {

        }
    }

    int catFriendly()
    {
        rand = RandomValue();

        if(catScript.friendly <= 30)
        {
            if(rand <= 30)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        if(catScript.friendly <= 60)
        {
            if(rand <= 30)
            {
                return 1;
            }
            else if(rand <= 60)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        if(catScript.friendly <= 90)
        {
            if(rand <= 30)
            {
                return 1;
            }
            else if(rand <= 60)
            {
                return 2;
            }
            else if(rand <= 90)
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}
