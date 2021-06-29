using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private item[] items;
    private GameObject player;
    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = player.GetComponent<PlayerController>();

        PlayerController.PlayerDeathHandler += PlayerController_PlayerDeathHandler;
    }

    public void PlayerController_PlayerDeathHandler()
    {
        if (playerControllerScript.playerLifeCount >= 0)
            StartCoroutine(SetPlayerActive());
    }

    public void DropItemIfLucky(Vector2 myPosition, Quaternion myRotation)
    {
        int luckyNum = 7;
        int luckyItemNum;
        int luckyDrawNum = Random.Range(1, 20);

        if (luckyDrawNum == luckyNum)
        {
            luckyItemNum = Random.Range(0, items.Length);
            if (items[luckyItemNum].name == "CoinX5")
            {
                if(Random.Range(0,3) == 1)
                {
                    luckyItemNum = 0;//CoinX1 item
                }
                else
                {
                    luckyItemNum = 1;// CoinX5 item
                } 
            }
            GameObject itemCreated = Instantiate(items[luckyItemNum].itemObject, myPosition, myRotation);
        }
    }

    IEnumerator SetPlayerActive()
    {
        yield return new WaitForSeconds(1);
        player.gameObject.SetActive(true);
        yield return null;
    }

    private void OnDisable()
    {
        PlayerController.PlayerDeathHandler -= PlayerController_PlayerDeathHandler;
    }
}

[System.Serializable]
public class item
{
    public string name;
    public GameObject itemObject;
}
