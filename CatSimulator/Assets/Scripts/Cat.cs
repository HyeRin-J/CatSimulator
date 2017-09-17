using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CatState {idle, walk, run, play}

public class Cat : MonoBehaviour {
    int rand = 0;
    public float status = 100.0f;
    public float friendly = 50.0f;
    public Animator anim;
    float StartTime, EndTime, AnimationTime, UserInputTime;
    public NavMeshAgent agent;
    public GameObject emo;
	public bool jump;
	private OffMeshLinkData offMeshLinkData;
	Vector3 jumpStartPos = Vector3.zero;
	float jumpSpeed = 2.0f;
	float jumpHeight = -0.8f;
	float jumpDistance = 0.0f;
	float jumpTotalTime = 0.0f;
	float jumpDeltaTime = 0.0f;

    int RandomValue()
    {
        int result = 0;

        result = Random.Range(0, 4);
        
        return result;
    }

	// Use this for initialization
	void Start () {
        rand = RandomValue();
        anim = GetComponent<Animator>();
        StartTime = Time.time;
		UserInputTime = 0.0f;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
	{
		EndTime = Time.time;

		if (anim.GetBool ("UserInput")) {
			agent.SetDestination (new Vector3 (1.19f, 1.3f, -0.46f));
			if (Vector3.Distance (new Vector3 (1.19f, 1.3f, -0.46f), transform.position) <= 0.3f) {
				agent.isStopped = true;
				anim.SetBool ("B_idle", true);
				GameObject.Find ("Cylinder").GetComponent<OffMeshLink> ().activated = false;
			} else if (Vector3.Distance (new Vector3 (1.19f, 1.3f, -0.46f), transform.position) > 0.3f) {
				anim.SetBool ("Run", true);
			}
			if (EndTime - UserInputTime >= 60.0f) {
				anim.SetBool ("UserInput", false);
				anim.SetBool ("B_idle", false);
				rand = RandomValue ();
				UserInputTime = 0.0f;
				GameObject.Find ("Cylinder").GetComponent<OffMeshLink> ().activated = true;
			}
		} else {
			GameObject.Find ("Cylinder").GetComponent<OffMeshLink> ().activated = true;
		}			

		if (agent.isOnOffMeshLink) {
			if (!jump) {
				offMeshLinkData = agent.currentOffMeshLinkData;
				jump = true;

				jumpStartPos = transform.position;

				Vector3 dirToJump = offMeshLinkData.endPos - jumpStartPos;

				jumpDistance = dirToJump.magnitude;

				jumpTotalTime = jumpDistance / jumpSpeed;

				jumpDeltaTime = 0.0f;

				dirToJump.y = 0.0f;
				dirToJump *= 1.0f / jumpDistance;

				agent.isStopped = true;

				if(Vector3.Distance(offMeshLinkData.startPos, GameObject.Find("Cylinder").transform.position) <= 0.2f){
					anim.SetBool ("JumpDown", true);
					anim.SetBool ("Jump", false);
				}else if(Vector3.Distance(offMeshLinkData.startPos, GameObject.Find("Cylinder (1)").transform.position) <= 0.3f){
					anim.SetBool ("Jump", true);
					anim.SetBool ("JumpDown", false);
				}

				} else {
					jumpDeltaTime += Time.deltaTime;

					float factor = jumpDeltaTime / jumpTotalTime;

					if (factor >= 1.0f) {
						transform.position = offMeshLinkData.endPos;

						agent.CompleteOffMeshLink ();

						agent.isStopped = false;

						jump = false;

						anim.SetBool ("Jump", false);
						anim.SetBool ("JumpDown", false);
					} else {
					Vector3 pos = Vector3.zero;
					if (anim.GetBool ("JumpDown")) {
						jumpHeight = -0.8f;
						pos = Vector3.Lerp (jumpStartPos, offMeshLinkData.endPos, factor);
						pos.y -= Mathf.Sin (Mathf.PI * factor) * jumpHeight;
					} else if (anim.GetBool ("Jump")) {
						jumpHeight = 0.8f;
						pos = Vector3.Lerp (jumpStartPos, offMeshLinkData.endPos, factor);
						pos.y += Mathf.Sin (Mathf.PI * factor) * jumpHeight;
					}
					transform.position = pos;
				}
			}

		}					

        if (EndTime - StartTime > 5.0f)
        {
            friendly++;
            StartTime = Time.time;
        }
        if (agent.remainingDistance == 0)
        {
			if (!agent.pathPending) {
				rand = RandomValue ();
				agent.SetDestination (new Vector3 (Random.Range (-600, 700) * 0.01f, Random.Range (0, 200) * 0.01f, Random.Range (-200, 500) * 0.01f));
			}
        }

        if (rand == (int)CatState.idle)
        {
			if (!agent.pathPending) {
				agent.velocity = new Vector3(0, 0, 0);
				agent.isStopped = true;

				anim.SetBool ("Run", false);
				anim.SetBool ("Walk", false);
				anim.SetBool ("Play", false);
			}
			if (EndTime - AnimationTime > 5.0f)
			{
				rand = RandomValue();
				agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));

				anim.SetBool("Run", false);
				anim.SetBool("Walk", false);
				anim.SetBool("Play", false);
				anim.SetBool("Cry", false);
			}
        }

        else if (rand == (int)CatState.walk)
        {
			if (agent.remainingDistance <= 0.1f)
            {
                rand = RandomValue();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
            }
			if (!agent.pathPending) {
				agent.speed = 1;
				anim.SetBool ("Run", false);
				anim.SetBool ("Walk", true);
				anim.SetBool ("Play", false);
			}
        }
        else if (rand == (int)CatState.run)
        {
            if (agent.remainingDistance <= 0.1f)
            {
                rand = RandomValue();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
            }
			if (!agent.pathPending) {
				agent.speed = 2;
				anim.SetBool ("Run", true);
				anim.SetBool ("Walk", false);
				anim.SetBool ("Play", false);
			}
        }   
        else
        {
			if (!agent.pathPending) {
				agent.velocity = new Vector3(0, 0, 0);
				agent.isStopped = true;

				anim.SetBool ("Sleep", true);
				anim.SetBool ("Run", false);
				anim.SetBool ("Walk", false);
				anim.SetBool ("Play", false);
			}
			if (EndTime - AnimationTime > 10.0f)
			{
				anim.SetBool("Sleep", false);
				rand = RandomValue();
			}
        }

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);   //현재 애니메이션 상태
        AnimatorTransitionInfo info2 = anim.GetAnimatorTransitionInfo(0);   //현재 트랜지션 상태
        //anim.runtimeAnimatorController = Resources.Load("") as RuntimeAnimatorController; //애니메이터 변경

        if (info.IsName("E_idle"))
        {
            agent.isStopped = true;
        }
        if (info.IsName("C_wiggle") || info.IsName("B_idle") || info.IsName("B_cry") || info.IsName("D_chodai"))
        {
			transform.LookAt (new Vector3 (GameObject.Find ("Main Camera").transform.position.x, GameObject.Find ("Main Camera").transform.position.y - 2.0f, GameObject.Find ("Main Camera").transform.position.z));
            agent.isStopped = true;
        }
		if (info2.IsName ("AnyState -> B_idle")) {
			if (UserInputTime == 0.0f) {
				UserInputTime = Time.time;
			}
        }
        if(info.IsName("A_idle") || info.IsName("C_sleep"))
        {
            emo.SetActive(false);
        }
        if (info.IsName("A_walk") || info.IsName("A_run"))
        {
            emo.SetActive(false);
            agent.isStopped = false;
        }
        if (info2.IsName("C_idle -> C_sleep") || info2.IsName("A_walk -> A_idle") || info2.IsName("A_run -> A_idle"))
        {
            AnimationTime = Time.time;
        }
		if (info.IsName ("A_jump_down") && transform.position.y <= 0.05f) {
			anim.SetBool ("JumpDown", false);
		}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("Sphere"))
        {
            Debug.Log("?");
        }
    }
}
