using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    Transform player;

    //Ragdoll
    private Rigidbody[] ragdollRigidbodies;
    private Collider[]  ragdollCol;

    //Animation
    Animator anim;
    Transform hipTransform;


    //AI System
    float timer;
    [HideInInspector]
    public bool move = false;
    private Transform target;
    bool startTimer;
    [HideInInspector]
    public bool first;

    public enum states {
        Walking,
        Ragdoll,
        Death,
        Sell
    } 

    public states actualState = states.Walking;

    NavMeshAgent navMesh;

    //AI Config
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject[] wayPoints;

    private Spawner spawner;

  
    void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollCol = GetComponentsInChildren<Collider>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hipTransform = anim.GetBoneTransform(HumanBodyBones.Hips);
        navMesh.speed = speed;
    }

    void Start(){
        wayPoints = GameObject.FindGameObjectsWithTag("Waypoints");
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        ToggleRagdoll(false);
        ChangeWaypoint();
    }

    // Update is called once per frame
    void Update()
    {

        switch(actualState){
            case states.Walking:
            WalkState();
            break;

            case states.Ragdoll:
            RagdollState();
            break;

            case states.Death:
            DeathState();
            break;

            case states.Sell:
            SellState();
            break;
        }

       if(Distance() < 1.5f && actualState == states.Walking){
        GetPunched();
       }

       if(startTimer){
        timer += Time.deltaTime;
       }

       if(timer > 3){
        player.GetComponent<BackpackScript>().AddBody(gameObject);
        transform.position = hipTransform.position;
        actualState = states.Death;
        timer = 0;
        startTimer = false;
       }        
    }
    public void SetTarget(Transform t){
        target = t;
    }

    void GetPunched(){
            player.gameObject.GetComponent<CharacterControl>().Punch();
            actualState = states.Ragdoll;
            spawner.mansAlive--;
        }

    void WalkState(){
        anim.SetBool("Walking", true);
        navMesh.enabled = true;
        if(navMesh.remainingDistance <= navMesh.stoppingDistance){
            ChangeWaypoint();
        }
    }

    void RagdollState(){
        anim.SetBool("Walking", false);
        anim.SetBool("Dead", true);

        ToggleRagdoll(true);
        navMesh.enabled = false;
    }

    void DeathState(){
        
        anim.SetBool("Dead", true);
        if(move){
            transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x, target.position.y + 0.5f,target.position.z), Time.deltaTime * 15);
            if(first)
                transform.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y + 90, target.eulerAngles.z);
            else
                transform.eulerAngles = target.eulerAngles;

        }
    }

    void SellState(){
        ragdollCol[0].enabled = true;
         if(move){
            transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x, target.position.y + 0.5f,target.position.z), Time.deltaTime * 10);
            if(first)
                transform.eulerAngles = new Vector3(target.eulerAngles.x, target.eulerAngles.y + 90, target.eulerAngles.z);
            else
                transform.eulerAngles = target.eulerAngles;

        }
    }

    public void ToggleRagdoll(bool state){
        anim.enabled = !state;
        startTimer = state;
        
        foreach(Rigidbody  rb in ragdollRigidbodies){
            rb.isKinematic = !state;
        }

        foreach(Collider col in ragdollCol){
            col.enabled = state;
        }
    }

    float Distance(){
        return Vector3.Distance(transform.position,player.position);
    }

    void ChangeWaypoint(){
        navMesh.SetDestination(wayPoints[Random.Range(0,wayPoints.Length - 1)].transform.position);
    }

    void OnTriggerStay(Collider col){
       
        if(col.CompareTag("Seller")){
            if(actualState == states.Sell)
                Destroy(gameObject, 3);
        }
    }
}
