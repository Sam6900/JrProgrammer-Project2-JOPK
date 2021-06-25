using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] item[] items;
    int luckyNum = 7;
    int luckyItemNum;

    public TextMeshProUGUI playerLifeCountText;
    private GameObject player;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = player.GetComponent<PlayerController>();

        PlayerController.PlayerDeathHandler += PlayerController_PlayerDeathHandler;
    }

    private void PlayerController_PlayerDeathHandler()
    {
        if (playerControllerScript.playerLifeCount >= 0)
            StartCoroutine(SetPlayerActive());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void DropItemIfLucky(Vector2 myPosition, Quaternion myRotation)
    {
        int luckyDrawNum = Random.Range(1, 20);
        if (luckyDrawNum == luckyNum)
        {
            luckyItemNum = Random.Range(0, items.Length);
            if (items[luckyItemNum].name == "CoinX5")
            {
                if(Random.Range(0,3) == 1)
                {
                    luckyItemNum = 0;
                }
                else
                {
                    luckyItemNum = 1;// CoinX5 item
                } 
            }
            GameObject itemCreated = Instantiate(items[luckyItemNum].itemObject, myPosition, myRotation);
        }
    }

    public void UpdatePlayerLifeCount(int count)
    {
        playerLifeCountText.text = "x " + count;
    }

    IEnumerator SetPlayerActive()
    {
        yield return new WaitForSeconds(1);
        player.gameObject.SetActive(true);
        yield return null;
    }
    
}

[System.Serializable]
public class item
{
    public string name;
    public GameObject itemObject;
}
