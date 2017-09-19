using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CatState {sleep, idle, wash, walk, run, play, pole}

public class Cat : MonoBehaviour
{
    public float status = 0.0f;
    public float hungry = 100.0f;
    public float happiness = 0.0f;
    public float friendly = 50.0f;
    public Animator anim;
    float FriendlyTimer, CurrentTime, AnimationTime, UserInputTime, PlayTime;
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
    private int select_behavior = 0;
    private bool onTriggerYarn = false, onTriggerScratter = false;
    GameObject YarnBall, Scratter;

    //행동 결정
    void RandomBehaviorSelector()
    {
        happiness = friendly * 0.2f + hungry * 0.3f + Random.Range(0, 100) * 0.1f - status * 0.4f;

        if (status >= 100.0f)
        {
            select_behavior = 0;
        }
        else if (status >= 60.0f)
        {
            if (happiness >= 60.0f)
            {
                select_behavior = Random.Range(0, 5);
            }
            else if (happiness >= 30.0f)
            {
                select_behavior = Random.Range(0, 4);
            }
            else
            { // happiness < 30.0f
                select_behavior = Random.Range(0, 3);
            }
        }
        else if (status >= 30.0f)
        {
            if (happiness >= 50.0f)
            {
                select_behavior = Random.Range(0, 7);
            }
            else if (happiness >= 30.0f)
            {
                select_behavior = Random.Range(0, 5);
            }
            else
            {   //happiness < 30.0f
                select_behavior = Random.Range(0, 3);
            }
        }
        else
        {   //status < 30.0f
            if (happiness >= 30.0f)
            {
                select_behavior = Random.Range(0, 7);
            }
            else if (happiness >= 10.0f)
            {
                select_behavior = Random.Range(0, 5);
            }
            else
            {   //happiness < 10.0f
                select_behavior = Random.Range(0, 4);
            }
        }
        Debug.Log("S_B : " + select_behavior);
    }

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        YarnBall = GameObject.FindGameObjectWithTag("YarnBall");
        Scratter = GameObject.Find("cu_cat2_pole_mesh");
        FriendlyTimer = CurrentTime = AnimationTime = UserInputTime = PlayTime = 0.0f;
        FriendlyTimer = Time.time;
        UserInputTime = 0.0f;
        agent = GetComponent<NavMeshAgent>();
        friendly = 50.0f;
        hungry = 100.0f;
        status = 0.0f;
        RandomBehaviorSelector();
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime = Time.time;
        
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);   //현재 애니메이션 상태
        AnimatorTransitionInfo info2 = anim.GetAnimatorTransitionInfo(0);   //현재 트랜지션 상태
        //anim.runtimeAnimatorController = Resources.Load("") as RuntimeAnimatorController; //애니메이터 변경

        KinectManager kinectManager = KinectManager.Instance;
        bool ready = Server.readyToUser;
        //유저입력대기
        if (anim.GetBool("UserInput"))
        {
            agent.SetDestination(new Vector3(1.0f, 1.17f, -0.2f));
            if (Vector3.Distance(new Vector3(1.0f, 1.17f, -0.2f), transform.position) <= 0.3f)
            {
                agent.isStopped = true;
                anim.SetBool("B_idle", true);
                GameObject.Find("Sofa").GetComponent<OffMeshLink>().activated = false;
            }
            else if (Vector3.Distance(new Vector3(1.0f, 1.17f, -0.2f), transform.position) > 0.3f)
            {
                anim.SetBool("Run", true);
            }
            if (CurrentTime - UserInputTime >= 60.0f && !kinectManager.IsUserDetected())
            {
                anim.SetBool("UserInput", false);
                anim.SetBool("B_idle", false);
                RandomBehaviorSelector();
                UserInputTime = 0.0f;
                GameObject.Find("Sofa").GetComponent<OffMeshLink>().activated = true;
            }
        }
        else
        {
            GameObject.Find("Sofa").GetComponent<OffMeshLink>().activated = true;
        }
        //점프
        if (agent.isOnOffMeshLink)
        {
            if (!jump)
            {
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

                if (Vector3.Distance(offMeshLinkData.startPos, GameObject.Find("Sofa").transform.position) <= 0.2f)
                {
                    anim.SetBool("JumpDown", true);
                    anim.SetBool("Jump", false);
                }
                else if (Vector3.Distance(offMeshLinkData.startPos, GameObject.Find("Plane (1)").transform.position) <= 0.3f)
                {
                    anim.SetBool("Jump", true);
                    anim.SetBool("JumpDown", false);
                }
            }
            else
            {
                jumpDeltaTime += Time.deltaTime;
                float factor = jumpDeltaTime / jumpTotalTime;
                bool isOffMeshLinkComplete = false;

                if (anim.GetBool("Jump"))
                {
                    if (factor > 0.5f)
                        isOffMeshLinkComplete = true;
                }
                else if (anim.GetBool("JumpDown"))
                {
                    if (factor > 1.0f)
                        isOffMeshLinkComplete = true;
                }

                if (isOffMeshLinkComplete)
                {
                    transform.position = offMeshLinkData.endPos;

                    agent.CompleteOffMeshLink();
                    agent.isStopped = false;

                    jump = false;

                    anim.SetBool("Jump", false);
                    anim.SetBool("JumpDown", false);
                }
                else
                {
                    Vector3 pos = Vector3.Lerp(jumpStartPos, offMeshLinkData.endPos, factor);
                    GameObject target = null;
                    Vector3 vec = Vector3.zero;
                    if (anim.GetBool("JumpDown"))
                    {
                        jumpHeight = -0.8f;
                        pos.y -= Mathf.Sin(Mathf.PI * factor) * jumpHeight;
                        target = GameObject.Find("Plane (1)");
                    }
                    else if (anim.GetBool("Jump"))
                    {
                        jumpHeight = 0.8f;
                        pos.y += Mathf.Sin(Mathf.PI * factor) * jumpHeight;
                        target = GameObject.Find("Sofa");
                    }
                    transform.position = pos;
                    vec = (target.transform.position - transform.position).normalized;
                    Quaternion toQuaternion = Quaternion.LookRotation(vec);
                    transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, 3.0f * Time.deltaTime);
                }
            }
        }

        if (hungry >= 0.02f)
        {
            hungry -= 0.02f;
        }

        //5초마다 호감도 1씩 상승
        if (CurrentTime - FriendlyTimer > 5.0f)
        {
            if (friendly < 100.0f)
            {
                friendly += 1.0f;
            }
            if (status <= 98.0f)
            {
                status += 2.0f;
            }
            if (hungry <= 0.1)
            {
                if (friendly >= 3.0f)
                    friendly -= 3.0f;
            }
            FriendlyTimer = Time.time;
        }

        if ((info.IsName("A_walk") || info.IsName("A_run")) && agent.remainingDistance <= 0.1f)
        {
            if (!agent.pathPending)
            {
                RandomBehaviorSelector();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, Random.Range(0, 200) * 0.01f, Random.Range(-200, 500) * 0.01f));
            }
        }
        if (select_behavior == (int)CatState.sleep)
        {
            YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
            Scratter.GetComponent<NavMeshObstacle>().enabled = true;
            if (!agent.pathPending)
            {
                anim.SetBool("Sleep", true);
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Play", false);
            }
            if (CurrentTime - AnimationTime > 10.0f)
            {
                anim.SetBool("Sleep", false);
                RandomBehaviorSelector();
            }
        }
        else if (select_behavior == (int)CatState.idle)
        {
            YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
            Scratter.GetComponent<NavMeshObstacle>().enabled = true;
            if (!agent.pathPending)
            {
                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Play", false);
                anim.SetBool("Sleep", false);
            }
            if (CurrentTime - AnimationTime > 5.0f)
            {
                RandomBehaviorSelector();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));

                anim.SetBool("Run", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Play", false);
                anim.SetBool("Sleep", false);
            }
        }
        else if(select_behavior == (int)CatState.wash)
        {
            YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
            Scratter.GetComponent<NavMeshObstacle>().enabled = true;
            agent.velocity = new Vector3(0, 0, 0);
            agent.speed = 0;
            agent.isStopped = true;
            anim.SetBool("Run", false);
            anim.SetBool("Walk", false);
            anim.SetBool("Play", false);
            anim.SetBool("Sleep", false);
            anim.SetBool("Wash", true);
        }
        else if (select_behavior == (int)CatState.walk)
        {
            YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
            Scratter.GetComponent<NavMeshObstacle>().enabled = true;
            
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= 0.1f)
                {
                    RandomBehaviorSelector();
                    agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
                }
                agent.speed = 1;
                anim.SetBool("Run", false);
                anim.SetBool("Walk", true);
                anim.SetBool("Play", false);
                anim.SetBool("Sleep", false);
            }
        }
        else if (select_behavior == (int)CatState.run)
        {
            YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
            Scratter.GetComponent<NavMeshObstacle>().enabled = true;
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= 0.1f)
                {
                    RandomBehaviorSelector();
                    agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
                }
                agent.speed = 2;
                anim.SetBool("Run", true);
                anim.SetBool("Walk", false);
                anim.SetBool("Play", false);
                anim.SetBool("Sleep", false);
            }
        }
        else if(select_behavior == (int)CatState.play)
        {
            
            agent.SetDestination(YarnBall.transform.position);
            YarnBall.GetComponent<NavMeshObstacle>().enabled = false;            
            if (onTriggerYarn)
            {
                anim.SetBool("Run", false);
                agent.velocity = new Vector3(0, 0, 0);
                agent.speed = 0;
                agent.isStopped = true;
                anim.SetBool("Play", true);
            }
            else
            {
                if (!agent.pathPending)
                {
                    agent.speed = 2;
                    anim.SetBool("Run", true);
                    anim.SetBool("Walk", false);
                    anim.SetBool("Play", false);
                    anim.SetBool("Sleep", false);
                }
            }
            if (info.IsName("A_punch_R") && info.normalizedTime >= 0.4)
            {
                YarnBall.transform.Translate(transform.right * -0.5f * Time.deltaTime);
                YarnBall.transform.Rotate(transform.right * -100.0f * Time.deltaTime);
            }
            if (CurrentTime - PlayTime >= 3.0f && PlayTime != 0.0f)
            {                
                anim.SetBool("Play", false);
                RandomBehaviorSelector();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
                YarnBall.GetComponent<NavMeshObstacle>().enabled = true;
                PlayTime = 0.0f;
                onTriggerYarn = false;
            }
        }
        else //if(select_behavior == (int)CatState.pole)
        {
            agent.SetDestination(Scratter.transform.position);
            Scratter.GetComponent<NavMeshObstacle>().enabled = false;

            if (onTriggerScratter)
            {
                anim.SetBool("Run", false);
                agent.velocity = new Vector3(0, 0, 0);
                agent.speed = 0;
                agent.isStopped = true;
                anim.SetBool("Polling", true);
                Vector3 vec = (Scratter.transform.position - transform.position).normalized;
                Quaternion toQuaternion = Quaternion.LookRotation(vec);
                transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, 3.0f * Time.deltaTime);
            }
            else
            {
                if (!agent.pathPending)
                {
                    agent.speed = 2;
                    anim.SetBool("Run", true);
                    anim.SetBool("Walk", false);
                    anim.SetBool("Play", false);
                    anim.SetBool("Sleep", false);
                }
            }
            if (CurrentTime - PlayTime >= 10.0f && PlayTime != 0.0f)
            {
                anim.SetBool("Polling", false);
                RandomBehaviorSelector();
                agent.SetDestination(new Vector3(Random.Range(-600, 700) * 0.01f, 0, Random.Range(-200, 500) * 0.01f));
                Scratter.GetComponent<NavMeshObstacle>().enabled = true;
                PlayTime = 0.0f;
                onTriggerScratter = false;
            }
        }

        if (info.IsName("B_idle"))
        {
            transform.LookAt(new Vector3(GameObject.Find("Main Camera").transform.position.x, GameObject.Find("Main Camera").transform.position.y - 2.0f, GameObject.Find("Main Camera").transform.position.z));
            agent.isStopped = true;
        }

        if (info2.IsName("AnyState -> B_idle"))
        {
            if (UserInputTime == 0.0f)
            {
                UserInputTime = Time.time;
            }
        }

        if (info.IsName("A_idle") || info.IsName("C_sleep"))
        {
            emo.SetActive(false);
            agent.speed = 0;
            agent.velocity = new Vector3(0, 0, 0);
            agent.isStopped = true;
            if (info.IsName("A_idle") && status >= 0.005f)
            {
                status -= 0.005f;
            }
            if (info.IsName("C_sleep") && status >= 0.001f)
            {
                status -= 0.001f;
            }
        }

        if (info.IsName("A_walk") || info.IsName("A_run"))
        {
            emo.SetActive(false);
            agent.isStopped = false;
            if (jump)
                jump = false;
            if (info.IsName("A_walk") && status <= 99.995f)
            {
                status += 0.005f;
            }
            if (info.IsName("A_run") && status <= 99.99f)
            {
                status += 0.01f;
            }
        }

        if(info.IsName("A_punch_R") || info.IsName("A_pole_loop"))
        {
            if (info.IsName("A_run") && status >= 0.003f)
            {
                status -= 0.003f;
            }
        }

        if (info2.IsName("C_idle -> C_sleep") || info2.IsName("A_walk -> A_idle") || info2.IsName("A_run -> A_idle") || info2.IsName("C_idle -> A_idle"))
        {
            AnimationTime = Time.time;
        }

        if (info.IsName("A_jump_down") && transform.position.y <= 0.05f)
        {
            anim.SetBool("JumpDown", false);
        }

        if(info2.IsName("A_idle -> A_punch_R") || info2.IsName("AnyState -> A_pole_start"))
        {
            PlayTime = Time.time;
        }

        if (info2.IsName("B_picks -> B_idle 0"))
        {
            anim.SetBool("Wash", false);
            RandomBehaviorSelector();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("YarnBall") && select_behavior == (int)CatState.play)
        {
            onTriggerYarn = true;
        }
        if (other.tag.Equals("Scratter") && select_behavior == (int)CatState.pole)
        {
            onTriggerScratter = true;
            anim.SetTrigger("PoleStart");
        }
    }
}
