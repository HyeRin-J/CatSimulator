using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Net;
using System.Net.Sockets;
using System.Text;

public class Event : MonoBehaviour
{
    int patternnum = 1;
    public GameObject Cat, Laserpoint;
    GameObject Pattern;
    Cat catScript;
    Animator anim;
    int rand;
    GameObject emo;
    new AudioSource audio;
    private bool actionTrigger = false;
    public Slider FriendlySlier, HungerSlider, StatusSlider;
    GameObject[] Button;
    GameObject inputButton, actionButton;
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
    // Use this for initialization
    void Start()
    {

        /*TcpClient client = new TcpClient();
      try{
         //client.Connect("172.19.89.16",9999);
         client.Connect("127.0.0.1",9999);
         Debug.Log ("Checked");
         string buffer = "HI";
         NetworkStream stream = client.GetStream ();
         byte[] sbuffer = Encoding.UTF8.GetBytes (buffer);
         stream.Write (sbuffer, 0, sbuffer.Length);
         stream.Flush ();
         client.Close ();
         Debug.Log ("Said hi");
      }catch(SocketException se){
         //return se;
         Debug.Log ("Socket Exception");
      }*/
        //Pattern = GameObject.FindGameObjectWithTag("Pattern");
        //털 패턴 적용, Title 화면부터 동작하지 않으면 오류 나기때문에 일단 비활성화.
        //patternnum = Pattern.GetComponent<Pattern>().patternnum;
        GameObject.Find("cu_cat2_mesh").GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
        catScript = Cat.GetComponent<Cat>();
        anim = Cat.GetComponent<Animator>();
        emo = GameObject.Find("Emoticon");
        emo.SetActive(false);
        audio = GameObject.Find("cu_cat2_model").GetComponent<AudioSource>();
        Button = GameObject.FindGameObjectsWithTag("UserButton");
        for (int i = 0; i < Button.Length; i++)
        {
            Button tempButton = Button[i].GetComponent<Button>();
            tempButton.onClick.AddListener(() => CatResponse(int.Parse(tempButton.name)));
        }
        inputButton = GameObject.Find("InputButton");
        actionButton = GameObject.Find("ActionButton");
        inputButton.GetComponent<Button>().onClick.AddListener(() => SetUserInput(true));
        actionButton.GetComponent<Button>().onClick.AddListener(() => SetActionTrigger());
    }

    // Update is called once per frame
    void Update()
    {
        FriendlySlier.value = catScript.friendly / 100;
        StatusSlider.value = catScript.tired / 100;
        HungerSlider.value = catScript.hungry / 100;

        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
            PlayerPrefs.DeleteKey("Friendly");
            PlayerPrefs.SetFloat("Friendly", 50.0f);
            PlayerPrefs.SetFloat("Hungry", 100.0f);
            PlayerPrefs.SetFloat("Status", 0.0f);
            catScript.friendly = PlayerPrefs.GetFloat("Friendly");
            catScript.hungry = PlayerPrefs.GetFloat("Hungry");
            catScript.tired = PlayerPrefs.GetFloat("Status");
        }

        if (Input.GetKey(KeyCode.F12))
        {
            for (int i = 0; i < Button.Length; i++)
            {
                if (Button[i].activeSelf)
                {
                    Button[i].SetActive(false);
                }
                else
                {
                    Button[i].SetActive(true);
                }
            }

            if (inputButton.activeSelf && actionButton.activeSelf)
            {
                inputButton.SetActive(false);
                actionButton.SetActive(false);

            }
            else
            {
                inputButton.SetActive(true);
                actionButton.SetActive(true);
            }
        }

        if (Input.GetKey(KeyCode.CapsLock))
        {
            if (!Laserpoint.GetComponent<LineRenderer>().enabled)
            {
                Laserpoint.GetComponent<LineRenderer>().enabled = true;
            }
            else
            {
                Laserpoint.GetComponent<LineRenderer>().enabled = false;
            }
        }

        if (Laserpoint.GetComponent<LineRenderer>().enabled)
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit Hit;

            if (Physics.Raycast(cameraRay, out Hit, Mathf.Infinity))
            {
                Laserpoint.transform.position = Hit.point;
                Laserpoint.GetComponent<LineRenderer>().SetPosition(0, Laserpoint.transform.position);
                Laserpoint.GetComponent<LineRenderer>().SetPosition(1, Camera.main.transform.position);
            }
            catScript.agent.SetDestination(Laserpoint.transform.position);
        }

        /*
		KinectManager kinectManager = KinectManager.Instance;
		if ((!kinectManager || !kinectManager.IsInitialized () || !kinectManager.IsUserDetected ())) {
			SetUserInput (false);
			//anim.SetBool ("UserInput", false);
			//Debug.Log ("Kinect dead.");
		}else{
			SetUserInput (true);
			//Debug.Log ("Kinect is Detecting");
			uint userId = kinectManager.GetPlayer1ID ();
			kinectManager.DetectGesture(userId, KinectGestures.Gestures.RaiseRightHand);
			kinectManager.DetectGesture(userId, KinectGestures.Gestures.RaiseLeftHand);
			string t = Server.returnValue; // "123.456" -> 123.456 
			if (t != "") {
				float a = float.Parse(t); // 0.05 + 0.5 = 0.55
				float test = a + (float)0.5;
				int b = (int)test;
				actionTrigger = true;
				CatResponse (b); 
			}
		}
        */
        GameObject light = GameObject.Find("Directional Light");
        light.transform.Rotate(0, 5 * Time.deltaTime, 0);
        if (light.transform.rotation.x >= 0.6f || light.transform.rotation.x <= -0.6)
        {
            light.GetComponent<Light>().intensity = 0;
        }
        else
        {
            light.GetComponent<Light>().intensity = 1;
        }
    }

    public void SetActionTrigger()
    {
        actionTrigger = true;
    }

    public void SetUserInput(bool t)
    {
        anim.SetBool("UserInput", t);
    }

    public void CatResponse(int b)
    {
        int result = 0;
        if (anim.GetBool("UserInput") && actionTrigger)
        {
            actionTrigger = false;
            result = catFriendly();
            emo.SetActive(true);
            anim.SetBool("B_idle", false);
            anim.SetBool("Hungry", false);
            //쓰다듬기
            if (b == 1)
            {
                switch (result)
                {
                    case 0:
                        anim.SetTrigger("Nagative");
                        catScript.friendly -= 4.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("angry") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-hissing-sound") as AudioClip;
                        audio.Play();
                        break;
                    case 1:
                        anim.SetBool("Run", true);
                        catScript.friendly -= 3.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("hate") as Texture2D;
                        catScript.agent.SetDestination(new Vector3(4.57f, 0.0f, -1.6f));
                        audio.clip = Resources.Load("Sounds/Angry-cat") as AudioClip;
                        audio.Play();
                        break;
                    case 2:
                        anim.SetBool("Positive", true);
                        catScript.friendly += 2.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("happy") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        audio.Play();
                        break;
                    case 3:
                        anim.SetTrigger("EarStart");
                        anim.SetBool("EarDown", true);
                        anim.SetBool("EarUp", true);
                        catScript.friendly += 3.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("love") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Meow-sound-3") as AudioClip;
                        break;
                }
            }
            //박수치기
            else if (b == 2)
            {
                switch (result)
                {
                    case 0:
                        anim.SetTrigger("Nagative");
                        catScript.friendly -= 4.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("angry") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-hissing-sound") as AudioClip;
                        audio.Play();
                        break;
                    case 1:
                        anim.SetBool("UserInput", false);
                        anim.SetBool("B_idle", false);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("embarrass") as Texture2D;
                        break;
                    case 2:
                        anim.SetBool("Positive", true);
                        catScript.friendly += 2.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("happy") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        audio.Play();
                        break;
                    case 3:
                        anim.SetTrigger("Charm");
                        catScript.friendly += 3.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("love") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Meow-sound-3") as AudioClip;

                        break;
                }
            }
            //밀어내기
            else if (b == 3)
            {
                switch (result)
                {
                    case 0:
                        anim.SetTrigger("Nagative");
                        catScript.friendly -= 2.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("angry") as Texture;
                        audio.clip = Resources.Load("Sounds/Cat-hissing-sound") as AudioClip;
                        audio.Play();
                        break;
                    case 1:
                        anim.SetTrigger("Nagative");
                        catScript.friendly -= 3.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("sad") as Texture;
                        audio.clip = Resources.Load("Sounds/Sad-cat") as AudioClip;
                        audio.Play();
                        break;
                    case 2:
                        anim.SetBool("Positive", true);
                        catScript.friendly += 2.0f;
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("happy") as Texture2D;
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        audio.Play();
                        break;
                    case 3:
                        anim.SetBool("walk", true);
                        emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("surprise") as Texture2D;
                        catScript.agent.SetDestination(Camera.main.transform.position);
                        audio.clip = Resources.Load("Sounds/Cat-meowing-sound") as AudioClip;
                        audio.Play();
                        break;
                }
            }
            // 밥주기
            else if (b == 4)
            {
                if (catScript.hungry <= 80)
                {
                    anim.SetTrigger("Eat");
                    catScript.hungry += 50.0f;
                    emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("yammy") as Texture;
                }
                else
                {
                    anim.SetBool("UserInput", false);
                    emo.GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("embarrass") as Texture2D;
                }
            }
            actionTrigger = true;
            SetUserInput(false);
        }
    }
    // 0 : 공용 부정, 1 : 전용 부정, 2 : 공용 긍정, 3 : 전용 긍정
    // 친밀도가 높아도 피로도가 낮으면 긍정 반응을 보기가 힘듬
    int catFriendly()
    {
        rand = Random.Range(0, 101);

        if (catScript.tired <= 50)
        {
            if (catScript.friendly <= 20)
            {
                if (rand <= catScript.friendly)
                {
                    int posRand = Random.Range(0, 2);
                    if (posRand == 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else if (catScript.friendly <= 50)
            {
                if (rand <= 20)
                {
                    return 2;
                }
                else if (rand <= catScript.friendly)
                {
                    return 3;
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else if (catScript.friendly <= 80)
            {
                if (rand <= 40)
                {
                    return 2;
                }
                else if (rand <= catScript.friendly)
                {
                    return 3;
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                if (rand <= 40)
                {
                    return 2;
                }
                else if (rand <= 80)
                {
                    return 3;
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }
        else
        {
            if (catScript.friendly <= 50)
            {
                if (rand <= 30)
                {
                    int posRand = Random.Range(0, 2);
                    if (posRand == 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else if (catScript.friendly <= 80)
            {
                if (rand <= 20)
                {
                    return 2;
                }
                else if (rand <= 40)
                {
                    return 3;
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            else
            {
                if (rand <= 30)
                {
                    return 2;
                }
                else if (rand <= 50)
                {
                    return 3;
                }
                else
                {
                    int nagRand = Random.Range(0, 2);
                    if (nagRand == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
        }
    }
}