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
    bool jumpdown = false;
    public GameObject emo;

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
        
        if (anim.GetBool("UserInput"))
		{
			agent.SetDestination(new Vector3(1.06f, 1.14f, -0.46f));

			if (Vector3.Distance (new Vector3 (1.06f, 1.14f, -0.46f), transform.position) <= 0.3f) {
				agent.isStopped = true;
				anim.SetBool ("B_idle", true);
			} else if (Vector3.Distance (new Vector3 (1.06f, 1.14f, -0.46f), transform.position) > 0.3f) {
				anim.SetBool ("Run", true);
			}
			else if (EndTime - UserInputTime >= 60.0f) {
				anim.SetBool ("UserInput", false);
				anim.SetBool ("B_idle", false);
				rand = RandomValue ();
				UserInputTime = 0.0f;
			}
		}

		if (GameObject.Find("Cylinder").GetComponent<OffMeshLink>().occupied && Vector3.Distance(transform.position, GameObject.Find("Cylinder").transform.position) <= 0.5f)
        {
            anim.SetTrigger("JumpDown");
        }
		if (GameObject.Find("Cylinder (1)").GetComponent<OffMeshLink>().occupied && Vector3.Distance(transform.position, GameObject.Find("Cylinder (1)").transform.position) <= 0.8f)
        {
			anim.SetTrigger("Jump");
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
        if (info2.IsName("AnyState -> A_jump_down"))
        {
            jumpdown = true;
        }
        if (info2.IsName("A_jump_down -> A_idle"))
        {
            jumpdown = false;
        }
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
    }
}
