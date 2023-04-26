using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackScript : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> bodys;
    public int maxBodys = 3;
    [SerializeField]
    Transform reference;

    Transform actualReference;

    public int money;
    void Start()
    {
        actualReference = reference;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBody(GameObject obj){
         NPC npc = obj.GetComponent<NPC>();
        if(bodys.Count < maxBodys){
            npc.SetTarget(actualReference);
            npc.move = true;
            npc.ToggleRagdoll(false);
            npc.first = IsFirst();
            bodys.Add(obj);
            actualReference = obj.transform;
        }else{
        Destroy(obj);
        }
    }

    IEnumerator SellAllBodies(Transform target){
        while(bodys.Count > 0){
            bodys[bodys.Count - 1].GetComponent<NPC>().SetTarget(target);
            bodys[bodys.Count - 1].GetComponent<NPC>().actualState = NPC.states.Sell;
            bodys.RemoveAt(bodys.Count - 1);
            money += 15;
            actualReference = reference;
            yield return new WaitForSeconds(2);

        }
    }

    public bool IsFirst(){
        return bodys.Count == 0;
    }

    void OnTriggerEnter(Collider col){
        if(col.CompareTag("Seller")){
            StartCoroutine(SellAllBodies(col.transform));
        }
    }
}
