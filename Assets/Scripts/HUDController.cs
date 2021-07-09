using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLifeCountText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text endGameText;
    private int playerNameWaitTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Instance != null)
        {
            StartCoroutine(ShowPlayerName());
        }
        TimeBar.TimeBarFinished += TimeBar_TimeBarFinished;
    }

    private void TimeBar_TimeBarFinished()
    {
        StartCoroutine(CheckForEnemiesDead());
    }

    public void UpdatePlayerLifeCount(int count)
    {
        playerLifeCountText.text = "x " + count;
    }

    private IEnumerator ShowPlayerName()
    {
        playerNameText.text = "Can you survive this apocalypse: " + MainManager.Instance.playerName;
        playerNameText.gameObject.SetActive(true);
        yield return new WaitForSeconds(playerNameWaitTime);
        Destroy(playerNameText);
    }

    private IEnumerator CheckForEnemiesDead()
    {
        while (Enemy.enemyCount > 0)
        {
            yield return new WaitForSeconds(1);
            if (PlayerController.isPlayerAlive)
            {
                endGameText.gameObject.SetActive(true);
                yield return null;
            }
        }
    }

    private void OnDisable()
    {
        TimeBar.TimeBarFinished -= TimeBar_TimeBarFinished;
    }
}
