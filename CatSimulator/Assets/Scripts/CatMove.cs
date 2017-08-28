using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;

public class CatMove : MonoBehaviour {

    NavMeshAgent agent;
    public GameObject cat;
    GameObject[] EndLinks;
    GameObject[] StartLinks;
    public GameObject food;
    Vector3 RandVec;
    Animator animator;
    public Event p_event;
    public int state = 0;
    Transform movePoint;
    float startTime, nextTime;
    bool refillstatus = false;
    bool hungry = false;
    bool poling = false;
    bool washing = false;
    bool stopping = false;
    public float status = 100.0f;
    public float hunger = 100.0f;
    public float friendly = 0.0f;
    public float funny = 100.0f;

    public int SetState()
    {
        state = (int)Random.Range(0, 2);

        return state;
    }

    //랜덤 좌표
    void RandomVector()
    {
        RandVec = new Vector3(Random.Range(-1800, 1901) * (float)0.01, Random.Range(0, 600) * (float)0.01, Random.Range(-3000, 1001) * (float)0.01);
    }

    // Use this for initialization
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        movePoint = GameObject.FindGameObjectWithTag("MovePoint").transform;
        EndLinks = GameObject.FindGameObjectsWithTag("EndLink");
        StartLinks = GameObject.FindGameObjectsWithTag("StartLink");
        food = GameObject.Find("cu_cat2_food_a_mesh");

        RandomVector();
        SetState();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;

        //배고프면 먹이 앞, 아니면 랜덤
        if (hungry)
        {
            pos = GameObject.Find("prop_feed_model").transform.position;
            if (cat.transform.position.x >= -17.5f)
            {
                pos.z -= 0.5f;
                pos.x += 0.5f;
            }else if(cat.transform.position.x < -17.5f)
            {
                pos.z -= 0.5f;
            }
        }
        else if (poling)
        {
            pos = GameObject.Find("prop_pole_model").transform.position;
            pos.z -= 1.0f;
        }
        else
        {
            pos = RandVec;
        }
        //배고픔 지수 낮을 경우 배고픔을 true
        if (hunger <= 0.0f)
        {
            hungry = true;
            
        }
        else if(hunger >= 100.0f)
        {
            hungry = false;
            animator.SetBool("Eat", false);

        }
        if(funny <= 0.0f)
        {
            poling = true;
        }else if(funny >= 100.0f)
        {
            poling = false;
            animator.SetBool("Poling", false);
        }
        //pos값으로 목적지 설정
        movePoint.position = pos;
        movePoint.gameObject.SetActive(true);
        agent.SetDestination(pos);

        if (animator.GetBool("Stop") == true)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Run", false);

            pos = cat.transform.position;
            agent.SetDestination(pos);

            stopping = true;

            nextTime = Time.time;
            if (nextTime - startTime > 5.0f && washing == true)
            {
                status += 0.5f;
                startTime = Time.time;
                animator.SetBool("Stop", false);
                animator.SetBool("Wash", false);
                washing = false;
            }
            else if (nextTime - startTime > 0.5f)
            {
                startTime = Time.time;
                animator.SetBool("Wash", true);
                washing = true;
            }
        }
        else if (animator.GetBool("Stop") == false)
        {
            if (stopping)
            {
                nextTime = Time.time;
                if (nextTime - startTime <= 2.0f)
                {
                    agent.SetDestination(cat.transform.position);
                }
                else
                {
                    agent.SetDestination(pos);
                    stopping = false;
                }
            }
            //점프할지 말지 position 값으로 판단함
            for (int i = 0; i < EndLinks.Length; i++)
            {
                if (EndLinks[i].GetComponent<OffMeshLink>().occupied && Vector3.Distance(EndLinks[i].transform.position, cat.transform.position) <= 0.1f)
                {
                    animator.SetTrigger("Jump");
                }
                else if (StartLinks[i].GetComponent<OffMeshLink>().occupied && Vector3.Distance(StartLinks[i].transform.position, cat.transform.position) <= 0.1f)
                {
                    animator.SetTrigger("JumpDown");
                }
            }
            //남은 거리가 0일때
            if (agent.remainingDistance == 0.0f)
            {
                //스테이터스가 0이상이고 배고프지 않으면 랜덤값
                cat.GetComponent<AudioSource>().clip = Resources.Load("Sounds/Cat-meow-sound-2") as AudioClip;
                if (status > 0.0f && hungry == false && stopping == false) RandomVector();
                movePoint.gameObject.SetActive(false);
                animator.SetBool("Move", false);
                animator.SetBool("Run", false);
                //배고픈데 남은 거리가 0인 경우 먹이 앞이라고 판단, 먹는 모션 활성화
                if (hungry)
                {
                    if (food.gameObject.activeSelf)
                    {
                        animator.SetBool("Eat", true);
                        hunger += 0.3f;
                        friendly += 0.01f;
                        food.transform.localScale -= new Vector3(0.003f, 0.003f, 0.003f);

                        if (food.transform.localScale.x <= 0.0f)
                        {
                            food.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        Debug.Log("밥 없음!");
                    }
                }
                if (poling)
                {
                    animator.SetBool("Poling", true);
                    funny += 0.7f;
                }
                //고양이 울음소리 재생
                cat.GetComponent<AudioSource>().Play();
            }
            else
            {
                if (refillstatus == false)
                {
                    if (status > 0.0f)
                    {
                        animator.SetBool("Move", false);
                        animator.SetBool("Run", true);
                        agent.speed = 3.5f;
                        status -= 0.1f;
                        hunger -= 0.05f;
                    }
                    else { refillstatus = true; }   //스테이터스 값이 0 밑이면 채워야 한다고 알려줌
                }
                //스테이터스 채우는 중이면 천천히 걷도록 함
                else
                {
                    animator.SetBool("Run", false);
                    animator.SetBool("Move", true);
                    agent.speed = 1.5f;
                    refillstatus = true;    //스테이터스 채워야 함!
                    hunger -= 0.03f;
                    nextTime = Time.time;
                    if (nextTime - startTime > 0.1f)    //0.1초에 0.5씩 채움
                    {
                        status += 0.5f;
                        startTime = Time.time;
                        if (status >= 100.0f) refillstatus = false;
                    }
                }
            }
        }
    }
}