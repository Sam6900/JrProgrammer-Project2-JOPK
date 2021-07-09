using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private Text nameText;

    public void SaveName()
    {
        MainManager.Instance.playerName = nameText.text;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
}
