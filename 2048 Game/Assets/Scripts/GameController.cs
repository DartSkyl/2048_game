using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField]
    public GameObject MineralsList;

    public static int Points { get; private set; }
    public static bool GameStarted { get; private set; }

    [SerializeField]
    private TextMeshProUGUI gameResult;
    [SerializeField]
    private TextMeshProUGUI pointsText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        gameResult.text = "";

        SetPoints(0);
        GameStarted = true;

        Field.Instance.GenerateField();
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "You Win!";
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "You Lose!";
    }
   
    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }

    public void OpenMineralsList()
    {
        MineralsList.SetActive(true);
    }
    
    public void CloseMineralsList()
    {
        MineralsList.SetActive(false);
    }
}
