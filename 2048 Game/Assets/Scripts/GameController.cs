using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public static int Points { get; private set; }
    public static bool GameStarted { get; private set; }

    [SerializeField]
    private TextMeshProUGUI gameResult;
    [SerializeField]
    private TextMeshProUGUI pointsText;  
    [SerializeField]
    private TextMeshProUGUI recordText;


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
        recordText.text = PlayerPrefs.GetInt("Score").ToString();

        SetPoints(0);
        GameStarted = true;

        Field.Instance.GenerateField();
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "Вы победили";
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "Вы проиграли!";
    }
   
    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }

    private void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
        if (PlayerPrefs.GetInt("Score") < Points)
            PlayerRecord(Points);
        
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayerRecord(int recordPoints)
    {
        PlayerPrefs.SetInt("Score", recordPoints);
        recordText.text = recordPoints.ToString();
    }

    public void MaxCell(int maxCell)
    {
        PlayerPrefs.SetInt("maxCell", maxCell);
    }
}
