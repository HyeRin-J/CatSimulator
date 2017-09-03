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
        //털 패턴 적용, Title 화면부터 동작하지 않으면 오류 나기때문에 일단 비활성화.
        //patternnum = Pattern.GetComponent<Pattern>().patternnum;
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
    void Update () {
        friendlyslider.value = catScript.friendly;
        statusslider.value = catScript.status;
    }

    //버튼 누르면 호출, Code 값이 계속 바뀌는 거 같음. Code로 구분하는 게 아니라 이름으로 구분해야 할듯.
    public void PressButton(int Code)
    {
        int result = catFriendly();

        if(Code == 0)
        {
            if(result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else if(Code == 1)
        {
            if (result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else if(Code == 2)
        {
            if (result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else if(Code == 3)
        {
            if (result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else if(Code == 4)
        {
            if (result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else if(Code == 5)
        {
            if (result == 0)
            {

            }
            if (result == 1)
            {

            }
            if (result == 2)
            {

            }
            if (result == 3)
            {

            }
        }
        else
        {
            Debug.Log("??");
        }
        Debug.Log(Code + ", " + result);
    }
    // 0이 부정적 반응, 1~3은 긍정적 반응
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
        if(catScript.friendly <= 60)
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
            return 0;
        }
    }
}
