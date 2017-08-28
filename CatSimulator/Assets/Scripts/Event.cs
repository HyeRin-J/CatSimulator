using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour {

    public float RandValue = 0;
    int patternnum;
    public Slider friendlyslider;
    public Slider statusslider;
    public GameObject Cat;
    GameObject Pattern;
    GameObject food;
    CatMove catmove;

    void RandomValue()
    {
        RandValue = Random.Range(0, 101);
    }

    // Use this for initialization
    void Start () {
        Pattern = GameObject.FindGameObjectWithTag("Pattern");
        catmove = GameObject.Find("cu_cat2_model").GetComponent<CatMove>();
        food = catmove.food;
        //털 패턴 적용
        patternnum = Pattern.GetComponent<Pattern>().patternnum;
        Cat.GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
    }

    // Update is called once per frame
    void Update () {
        statusslider.value = catmove.status * 0.01f;
        friendlyslider.value = catmove.friendly * 0.01f;
    }

    private void OnGUI()
    {
        AudioClip clip;

        int w = Screen.width / 2;
        int h = Screen.height / 2;
        if (GUI.Button(new Rect(w * 0.5f - 50, h * 1.4f, 120, 50), "쓰다듬기"))
        {
            catmove.cat.GetComponent<Animator>().SetBool("Stop", true);
            catmove.friendly += 0.5f;
            RandomValue();
            if(RandValue <= catmove.friendly)
            {
                clip = Resources.Load<AudioClip>("Sounds/Purring Cat-Sound");
                catmove.cat.GetComponent<AudioSource>().PlayOneShot(clip);
            }
            else
            {
                clip = Resources.Load<AudioClip>("Sounds/Angry_Cat");
                catmove.cat.GetComponent<AudioSource>().PlayOneShot(clip);
            }
        }
        if (GUI.Button(new Rect(w * 1.5f - 50, h * 1.4f, 120, 50), "놀아주기"))
        {
            catmove.cat.GetComponent<Animator>().SetBool("Stop", true);
            RandomValue();

            catmove.friendly += 5.0f;
            catmove.status -= 1.0f;
        }
        if (GUI.Button(new Rect(w - 50, h * 1.4f, 120, 50), "밥주기"))
        {
            food.gameObject.SetActive(true);
            food.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    
}
