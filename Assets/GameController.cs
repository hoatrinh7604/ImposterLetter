using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this.gameObject)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public int firstChoosingRowIndex;
    public int firstChoosingColIndex;

    [SerializeField] ContentController contentController;

    [SerializeField] int[] list = {1,1,2,2,1,2};
    [SerializeField] int[] listControl = {1,1,2,2,1,2};
    [SerializeField] List<int> listId = new List<int>();

    private int row = 6;
    private int col = 6;
    [SerializeField] int[] rowLevel = {8, 10, 12};
    [SerializeField] int[] colLevel = {6, 8, 10 };

    [SerializeField] float[] timeLevel = { 200, 250, 300 };
    [SerializeField] float time = 200;
    [SerializeField] float timeOfGame = 200;

    private int remainingCouple;
    private int currentNumber;

    private int shuffeRemaining;

    private float timeNotice = 0;

    private int score;
    private int highScore;

    public string alphabets = "abcdefghijklmnopqrstuvqxyz";

    // Start is called before the first frame update
    void Start()
    {
        ListMaker();
        StartGame();
    }

    private void Update()
    {
        time -= Time.deltaTime;
        timeNotice += Time.deltaTime;
        UpdateSliderValue(time);
        if(time < 0)
        {
            GameOver(false);
        }

        if(timeNotice > 10)
        {
            timeNotice = -100;
            Notice();
        }
    }

    public void Notice()
    {
        for(int i = 0; i < list.Length; i++)
        {
            int rowT = i / col;
            int colT = i - (rowT * col);
            if(list[i] == currentNumber)
            {
                contentController.Notice(rowT, colT);
            }
        }
    }

    public void StartGame()
    {
        if (PlayerPrefs.GetInt("level") < 1)
            PlayerPrefs.SetInt("level", 1);

        score = 0;
        GetComponent<UIController>().UpdateScore(score);
        highScore = PlayerPrefs.GetInt("score");
        GetComponent<UIController>().UpdateHighScore(highScore);

        row = rowLevel[PlayerPrefs.GetInt("level")-1];
        col = colLevel[PlayerPrefs.GetInt("level")-1];
        timeOfGame = timeLevel[PlayerPrefs.GetInt("level") - 1];

        contentController.SpawItems(col, row, currentNumber);

        time = timeOfGame;
        SetSliderValue(time);
        currentNumber = Random.Range(0,10);
        StartNewTurn();
        Time.timeScale = 1;
    }

    public void StartNewTurn()
    {
        timeNotice = 0;
        time = timeOfGame;
        currentNumber = Random.Range(0,10);
        GetComponent<UIController>().UpdateCurrentNumber(currentNumber);
        contentController.UpdateItems(row, col, currentNumber);
    }

    public void UpdateScore()
    {
        score++;
        GetComponent<UIController>().UpdateScore(score);
        if (highScore < score)
        {
            highScore = score;
            PlayerPrefs.SetInt("score", highScore);
            GetComponent<UIController>().UpdateHighScore(highScore);
        }
    }

    public void UserChooseItem(string idItem, int row, int col)
    {
        if (idItem == (alphabets[currentNumber] +""))
        {
            UpdateScore();
            StartNewTurn();
        }
        else
        {
            GameOver(false);
        }
    }

    public void ListMaker()
    {
        int[] temp = new int[row * col];

        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j< col; j++)
            {
                int index = i * col + j;
                temp[index] = 0;
            }
        }

        list = temp;
        listControl = temp;
    }

    public void GameOver(bool isWin)
    {
        GetComponent<UIController>().GameOver(isWin);
    }

    public void UpdateRemainingShuffe()
    {
        
    }

    public void UpdateSliderValue(float value)
    {
        GetComponent<UIController>().UpdateSliderValue(value);
    }

    public void SetSliderValue(float value)
    {
        GetComponent<UIController>().SetSlider(value);
    }
}
