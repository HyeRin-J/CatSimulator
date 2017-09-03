using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatState {idle, walk, run, play}

public class Cat : MonoBehaviour {
    int rand = 0;
    public float status = 100.0f;
    public float friendly = 0;
    Animator anim;
    float StartTime, EndTime;

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
    }

    // Update is called once per frame
    void Update () {
        EndTime = Time.time;

        if(EndTime - StartTime > 5.0f)
        {
            friendly++;
            StartTime = Time.time;
        }

        if (rand == (int)CatState.idle)
        {

        }
        else if(rand == (int)CatState.walk)
        {

        }
        else if(rand == (int)CatState.run)
        {

        }
        else if(rand == (int)CatState.play)
        {

        }
        else
        {
            anim.SetBool("Sleep", true);
        }

        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);   //현재 애니메이션 상태
        AnimatorTransitionInfo info2 = anim.GetAnimatorTransitionInfo(0);   //현재 트랜지션 상태

        if (info2.IsName("F_idle -> F_sleep"))
        {
            Debug.Log("전환");
        }
    }
}
