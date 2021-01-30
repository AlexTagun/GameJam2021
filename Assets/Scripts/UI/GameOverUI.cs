using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel = null;
    [SerializeField] private GameObject losePanel = null;
    //[SerializeField] private GameObject totalLosePanel = null;

    void Start()
    {
        victoryPanel.SetActive(false);
        losePanel.SetActive(false);
        GameController.Instance.OnGameOver += ShowGameOver;
    }

    public void ShowGameOver(bool b)
    {
        if (b) ShowVictory();
        else ShowLose();
    }


    public void ShowVictory()
    {
        victoryPanel.SetActive(true);

    }
    public void ShowLose()
    {
        losePanel.SetActive(true);

    }

}
