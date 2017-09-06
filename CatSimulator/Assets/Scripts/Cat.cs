using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CatState {idle, walk, run, play}

public class Cat : MonoBehaviour {
    int rand = 0;
    public float status = 100.0f;
    public float friendly = 0;
    Animator anim;
    float StartTime, EndTime, AnimationTime;
    NavMeshAgent agent;

    int RandomValue()
    {
        int result = 0;

        result = Random.Range(0, 5);

        return result;
    }

	// Use this for initialization
	void Start () {
        rand = RandomValue();
        anim = GetComponent<Animator>();
        StartTime = Time.time;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        EndTime = Time.time;

        if (EndTime - StartTime > 5.0f)
        {
            friendly++;
            StartTime = Time.time;
        }
        if (agent.remainingDistance == 0)
        {
            rand = RandomValue();
            agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
        }

        if (rand == (int)CatState.idle)
        {
            agent.isStopped = true;

            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            anim.SetBool("Play", false);

            if (EndTime - AnimationTime > 5.0f)
            {
                rand = RandomValue();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
            }
        }

        else if (rand == (int)CatState.walk)
        {
            if (agent.remainingDistance == 0)
            {
                rand = RandomValue();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
            }
            agent.speed = 1;
            anim.SetBool("Run", false);
            anim.SetBool("Walk", true);
            anim.SetBool("Play", false);
        }
        else if (rand == (int)CatState.run)
        {
            if (agent.remainingDistance == 0)
            {
                rand = RandomValue();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
            }
            agent.speed = 2;
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
            anim.SetBool("Play", false);
        }   
        else
        {
            agent.isStopped = true;

            anim.SetBool("Sleep", true);
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            anim.SetBool("Play", false);

            if(EndTime - AnimationTime > 5.0f)
            {
                anim.SetBool("Sleep", false);
                rand = RandomValue();
            }
        }

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);   //현재 애니메이션 상태
        AnimatorTransitionInfo info2 = anim.GetAnimatorTransitionInfo(0);   //현재 트랜지션 상태
        //anim.runtimeAnimatorController = Resources.Load("") as RuntimeAnimatorController; //애니메이터 변경
        if(info2.IsName("CtoA -> A_idle") || info2.IsName("A_idle -> A_run") || info2.IsName("A_idle -> A_walk"))
        {
            agent.isStopped = false;
        }
        if(info.IsName("A_walk") || info.IsName("A_run"))
        {
            agent.isStopped = false;
        }
        if (info2.IsName("C_idle -> C_sleep") || info2.IsName("A_walk -> A_idle") || info2.IsName("A_run -> A_idle"))
        {
            AnimationTime = Time.time;
        }
    }
}
