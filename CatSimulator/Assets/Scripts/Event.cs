﻿using System.Collections;
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
    GameObject Box1;
    GameObject Box2;

    int RandomValue()
    {
        int RandValue = Random.Range(0, 101);

        return RandValue;
    }

    // Use this for initialization
    void Start () {
        //Pattern = GameObject.FindGameObjectWithTag("Pattern");
        //털 패턴 적용, Title 화면부터 동작하지 않으면 오류 나기때문에 일단 비활성화.
        //patternnum = Pattern.GetComponent<Pattern>().patternnum;
        Box1 = GameObject.Find("cardboardBox_01");
        Box2 = GameObject.Find("cardboardBox_02");
        GameObject.Find("cardboardBox_02").SetActive(false);
        GameObject.Find("cu_cat2_mesh").GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
        catScript = Cat.GetComponent<Cat>();
        anim = Cat.GetComponent<Animator>();
        button = GameObject.FindGameObjectsWithTag("Button");
        //버튼 onClick에 함수 등록, 버튼 6개
        for(int i = 0; i < 6; i++)
        {
            Button btn = button[i].GetComponent<Button>();
            int code = i;
            btn.onClick.AddListener(() => PressButton(code));
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        friendlyslider.value = catScript.friendly;
        statusslider.value = catScript.status;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePoisition = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (mousePoisition.x >= 0.2 && mousePoisition.x <= 0.3)
            {
                if (mousePoisition.y >= 0.1 && mousePoisition.y <= 0.4)
                {   
                    if (Box1.activeSelf)
                    {
                        Box1.SetActive(false);
                        Box2.SetActive(true);
                    }
                    else if (Box2.activeSelf)
                    {
                        Box1.SetActive(true);
                        Box2.SetActive(false);
                    }
                }
                if(mousePoisition.y >= 0.7 && mousePoisition.y <= 0.8)
                {
                    Debug.Log("전등");
                }
            }
            if (mousePoisition.x >= 0.7 && mousePoisition.x <= 0.8 && mousePoisition.y >= 0.1 && mousePoisition.y <= 0.2)
            {
                catScript.agent.SetDestination(GameObject.Find("Sphere").transform.position);
                catScript.agent.stoppingDistance = 2.0f;
            }
        }
    }

    //버튼 누르면 호출, Code 값이 계속 바뀌는 거 같음. Code로 구분하는 게 아니라 이름으로 구분해야 할듯.
    public void PressButton(int Code)
    {
        int result = catFriendly();
        catScript.agent.SetDestination(new Vector3(0.3f, 1.21f, -0.264f));
        catScript.agent.isStopped = true;
        
        if (button[Code].name == "긍정1")
        {
            switch (result)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if(button[Code].name == "긍정2")
        {
            switch (result)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if(button[Code].name == "긍정3")
        {
            switch (result)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if(button[Code].name == "긍정4")
        {
            switch (result)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if(button[Code].name == "부정1")
        {
            switch (result)
            {
                case 0:
                    anim.SetBool("Nag1", true);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if (button[Code].name == "부정2")
        {
            switch (result)
            {
                case 0:
                    anim.SetBool("Nag1", true);
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
        else if (button[Code].name == "동작X"){
            Debug.Log("랜덤행동");
        }
        else
        {
            Debug.Log("??");
        }
        Debug.Log(Code + ", " + result);
    }
    // 0이 부정적 반응, 1~3은 긍정적 반응, 숫자 클 수록 반응 정도가 달라짐
    int catFriendly()
    {
        rand = RandomValue();

        if(catScript.friendly <= 30)
        {
            if(rand <= catScript.friendly)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        if(catScript.friendly > 30 && catScript.friendly <= 60)
        {
            if(rand <= 30)
            {
                return 1;
            }
            else if(rand <= catScript.friendly)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
		if(catScript.friendly > 60 && catScript.friendly <= 90)
        {
            if(rand <= 30)
            {
                return 1;
            }
            else if(rand <= 60)
            {
                return 2;
            }
            else if(rand <= catScript.friendly)
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
    }
}
