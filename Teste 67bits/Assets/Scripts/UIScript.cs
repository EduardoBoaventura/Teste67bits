using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScript : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI maxBodys;

    [SerializeField]
     TextMeshProUGUI moneyText;

    BackpackScript backpack;

    int spacePrice;
    [SerializeField]
    TextMeshProUGUI spacePriceText;
    [SerializeField]
    TextMeshProUGUI spaceAvailable;

    [SerializeField]
    int colorPrice = 30;

    [SerializeField]
    GameObject panelShop;

    bool shopIsOpen;

    [SerializeField]
    GameObject insufficientMoney;

    [SerializeField]
    Material playerMaterial;


    void Start()
    {
        Time.timeScale = 1;
        backpack = GameObject.FindWithTag("Player").GetComponent<BackpackScript>();
        spacePrice = backpack.maxBodys * 10;
        spacePriceText.text = "$ " + spacePrice;
        spaceAvailable.text = backpack.maxBodys + "  >  " + (backpack.maxBodys + 1);
        playerMaterial = GameObject.Find("Character_Woman").GetComponent<SkinnedMeshRenderer>().material;
        maxBodys.text = backpack.bodys.Count + " / " + backpack.maxBodys;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$ " + backpack.money;
        maxBodys.text = backpack.bodys.Count + " / " + backpack.maxBodys;

    }

    public void BuyBackpackSpace(){
        if(backpack.money >= spacePrice){
            backpack.maxBodys++;
            backpack.money -= spacePrice;
            spacePrice = backpack.maxBodys * 10;
            spacePriceText.text = "$ " + spacePrice;
            spaceAvailable.text = backpack.maxBodys + "  >  " + (backpack.maxBodys + 1);

        }else{
            StartCoroutine(Notification());

        }
    }

    public void BuyRandomColor(){
        if(backpack.money >= colorPrice){
            Color32 color = new Color32((byte)Random.Range(0,255),(byte)Random.Range(0,255),(byte)Random.Range(0,255),255);
            playerMaterial.SetColor("_Color",color);
            backpack.money -= colorPrice;
        }else{
            StartCoroutine(Notification());
        }
    }

    public void OpenShop(){
        shopIsOpen = !shopIsOpen;
        if(shopIsOpen){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
        panelShop.SetActive(shopIsOpen);
    }

    IEnumerator Notification(){
        insufficientMoney.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        insufficientMoney.SetActive(false);

    }
}
