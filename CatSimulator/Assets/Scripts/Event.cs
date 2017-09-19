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
<<<<<<< HEAD
=======
    private bool actionTrigger = false;
    public Slider FriendlySlier, HungerSlider, StatusSlider;

>>>>>>> be458df26ff1c83a90dd18995d3984c94a9d932b
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
<<<<<<< HEAD
    int RandomValue()
=======
    // Use this for initialization
    void Start()
>>>>>>> be458df26ff1c83a90dd18995d3984c94a9d932b
    {

<<<<<<< HEAD
        return RandValue;
    }
    
    // Use this for initialization
	void Start () {
        //Pattern = GameObject.FindGameObjectWithTag("Pattern");
        //털 패턴 적용, Title 화면부터 동작하지 않으면 오류 나기때문에 일단 비활성화.
        //patternnum = Pattern.GetComponent<Pattern>().patternnum;
        GameObject.Find("cardboardBox_02").SetActive(false);
=======
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
>>>>>>> be458df26ff1c83a90dd18995d3984c94a9d932b
        GameObject.Find("cu_cat2_mesh").GetComponent<Renderer>().material.mainTexture = Resources.Load("cu_cat2_" + patternnum) as Texture2D;
        catScript = Cat.GetComponent<Cat>();
        anim = Cat.GetComponent<Animator>();
        emo = GameObject.Find("Emoticon");
        emo.SetActive(false);
        audio = GameObject.Find("cu_cat2_model").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        //friendlyslider.value = catScript.friendly;
        //statusslider.value = catScript.status;
=======
        FriendlySlier.value = catScript.friendly / 100;
        StatusSlider.value = catScript.status / 100;
        HungerSlider.value = catScript.hungry / 100;

        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
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

>>>>>>> be458df26ff1c83a90dd18995d3984c94a9d932b
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
            //쓰다듬기
            if (b == 1)
            {
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
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                        break;
                }
            }
            //박수치기
            else if (b == 2)
            {
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
                        audio.clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;

                        break;
                }
            }
            //밀어내기
            else if (b == 3)
            {
                switch (result)
                {
                    case 0:
                        anim.SetTrigger("Nag1");
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
            actionTrigger = true;
        }
        // 밥주기
        else if (b == 4)
        {

        }
    }
    // 0이 부정적 반응, 1~2은 긍정적 반응, 숫자 클 수록 반응 정도가 달라짐
    int catFriendly()
    {
        rand = Random.Range(0, 101);

        if (catScript.friendly <= 35)
        {
            if (rand <= catScript.friendly)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        if (catScript.friendly > 35 && catScript.friendly <= 70)
        {
            if (rand <= 35)
            {
                return 1;
            }
            else if (rand <= catScript.friendly)
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
            if (rand <= 35)
            {
                return 1;
            }
            else if (rand <= 70)
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