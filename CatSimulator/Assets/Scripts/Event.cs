using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Event : MonoBehaviour{

    GameObject[] button;
    int patternnum = 1;
    public Slider friendlyslider;
    public Slider statusslider;
    public GameObject Cat;
    GameObject Pattern;
    GameObject food;
	GameObject Light;
    Cat catScript;
    Animator anim;
    int rand;
    GameObject Box1;
	GameObject Box2;
    GameObject emo;
    AudioSource audio;
    /*
	public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, 
		float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		// don't do anything here
	}

	public bool GestureCompleted (uint userId, int userIndex, KinectGestures.Gestures gesture, 
		KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
	{
		return true;
	}

	public bool GestureCancelled (uint userId, int userIndex, KinectGestures.Gestures gesture, 
		KinectWrapper.NuiSkeletonPositionIndex joint)
	{
		// don't do anything here, just reset the gesture state
		return true;
	}
    */
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
		Light = GameObject.Find("Point light");
        GameObject.Find("cardboardBox_02").SetActive(false);
        GameObject.Find("cu_cat2_mesh").GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
        catScript = Cat.GetComponent<Cat>();
        anim = Cat.GetComponent<Animator>();
        button = GameObject.FindGameObjectsWithTag("Button");
        emo = GameObject.Find("Emoticon");
        emo.SetActive(false);
        //버튼 onClick에 함수 등록, 버튼 6개
		for(int i = 0; i < button.Length; i++)
        {
            Button btn = button[i].GetComponent<Button>();
            int code = i;
            btn.onClick.AddListener(() => PressButton(code));
        }
        audio = GameObject.Find("cu_cat2_model").GetComponent<AudioSource>();
		GameObject.Find ("입력").GetComponent<Button> ().onClick.AddListener (() => SetUserInput());
		GameObject.Find("초기화").GetComponent<Button>().onClick.AddListener(() => Initallize());
    }

    // Update is called once per frame
	void Update()
	{
        //friendlyslider.value = catScript.friendly;
        //statusslider.value = catScript.status;
        /*
		KinectManager kinectManager = KinectManager.Instance;
		if ((!kinectManager || !kinectManager.IsInitialized () || !kinectManager.IsUserDetected ())) {
			anim.SetBool ("UserInput", false);
			//Debug.Log ("Kinect dead.");
		}else{
			anim.SetBool ("UserInput", true);
			//Debug.Log ("Kinect is Detecting");
			uint userId = kinectManager.GetPlayer1ID ();
			kinectManager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
			kinectManager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
		}
        */
        GameObject light = GameObject.Find("Directional Light");
        light.transform.Rotate(0, 5 * Time.deltaTime, 0);
        Debug.Log(light.transform.rotation);
        if (light.transform.rotation.x >= 0.6f || light.transform.rotation.x <= -0.6)
        {
            light.GetComponent<Light>().intensity = 0;
        }
        else
        {
            light.GetComponent<Light>().intensity = 1;
        }
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
            }
			if(mousePoisition.x >= 0.0 && mousePoisition.x <= 0.1 && mousePoisition.y >= 0.7 && mousePoisition.y <= 0.8){
				Light.GetComponent<Light> ().enabled = !Light.GetComponent<Light> ().enabled;
			}
            if (mousePoisition.x >= 0.7 && mousePoisition.x <= 0.8 && mousePoisition.y >= 0.1 && mousePoisition.y <= 0.2)
            {
                catScript.agent.SetDestination(GameObject.Find("Sphere").transform.position);
                catScript.agent.stoppingDistance = 2.0f;
            }
        }
    }

	public void SetUserInput(){
		anim.SetBool ("UserInput", true);
	}

	public void Initallize(){
		SceneManager.LoadScene ("Main");
	}

    //버튼 누르면 호출, Code 값이 계속 바뀌는 거 같음. Code로 구분하는 게 아니라 이름으로 구분해야 할듯.
    public void PressButton(int Code)
	{
		int result = 0;
		if (anim.GetBool ("UserInput")) {
			result = catFriendly ();
            emo.SetActive(true);
			if (button [Code].name == "긍정1") {
                switch (result) {
                    case 0:
                        anim.SetTrigger ("Nag1");
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("angry") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Angry_Cat") as AudioClip;
                        audio.Play();
                        break;
				    case 1:
					    anim.SetBool ("Pos1", true);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("happy") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        audio.Play();
                        break;
				    case 2:
					    anim.SetBool ("Pos2", true);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("love") as Texture2D;
                        break;
				    }
			} else if (button [Code].name == "긍정2") {
                switch (result)
                {
                    case 0:
                        anim.SetTrigger("Nag1");
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("angry") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Angry_Cat") as AudioClip;
                        audio.Play();
                        break;
                    case 1:
                        anim.SetBool("Pos1", true);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("happy") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        audio.Play();
                        break;
                    case 2:
                        anim.SetBool("Pos2", true);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("love") as Texture2D;
                        break;
                }
            }
		/*
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
        }*/
        else if (button [Code].name == "부정1") {
				switch (result) {
				    case 0:
					    anim.SetTrigger ("Nag1");
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("sad") as Texture;
                        audio.clip = Resources.Load("Sounds/Cat-noises") as AudioClip;
                        audio.Play();
                        break;
				    case 1:
					    break;
				    case 2:
					    break;
				    case 3:
					    break;
				}
			}
		/*
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
        }*/
        else if (button [Code].name == "동작X") {
				Debug.Log ("랜덤행동");
			} else {
				Debug.Log ("??");
				anim.SetTrigger ("Cry");
			}
			catScript.agent.isStopped = true;
			anim.SetBool ("UserInput", false);
			anim.SetBool ("B_idle", false);
		}
    }
    // 0이 부정적 반응, 1~3은 긍정적 반응, 숫자 클 수록 반응 정도가 달라짐
    int catFriendly()
    {
        rand = RandomValue();

        if(catScript.friendly <= 35)
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
        if(catScript.friendly > 35 && catScript.friendly <= 70)
        {
            if(rand <= 35)
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
        else
        {
			if(rand <= 35)
			{
				return 1;
			}
			else if(rand <= 70)
			{
				return 2;
			}
			else
			{
				return 0;
			}
        }
    }
}
