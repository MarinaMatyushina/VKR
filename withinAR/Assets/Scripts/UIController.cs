using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject timer;
    public GameObject timerPanel;
    public GameObject levelProgressBar;
    public GameObject answerPanel;
    public GameObject endLevelTextObject;
    public GameObject[] levelProgressObjects;
    public GameObject currentLevelText;
    public GameObject currentLevelPanel;

    public GameObject leftButtonsPanel;
    public GameObject rightButtonsPanel;
    private int buttonsCount;

    private GameController gameController;
    private GameTimer gameTimer;

    private Properties props;

    private List<GameObject> buttons = new List<GameObject>();
    public GameObject buttonPrefab;

    public GameObject endLevelpanel;

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameTimer = FindObjectOfType<GameTimer>();
        props = FindObjectOfType<Properties>();
        buttonsCount = props.GetButtonsCount();
    }

    private void CreateButtons()
    {
        for(int i = 0; i < buttonsCount; i++)
        {
            GameObject button = CreateButton(i); 
            // Генерация кнопок только справа
            //if (i % 2 == 0)
            //{
            //    button.transform.SetParent(leftButtonsPanel.transform);
            //}
            //else
            button.transform.SetParent(rightButtonsPanel.transform);
            buttons.Add(button);
        }
    }

    private GameObject CreateButton(int buttonNumber)
    {
        GameObject button = Instantiate(buttonPrefab);
        button.GetComponent<Image>().color = props.colors[buttonNumber];
        button.transform.position = new Vector3(0, 0, 0);
        return button;
    }

    private void DestroyButtons()
    {
        foreach(var button in buttons)
        {
            Destroy(button);
        }
    }

    public void InitializeUI(int currentLevelProgress)
    {
        DestroyButtons();
        CreateButtons();
        //ResetButtonsColor();
        UpdateLevelProgress(currentLevelProgress);
        UpdateTimer(gameTimer.GetMaxTime());
        timer.SetActive(true);
        levelProgressBar.SetActive(true);
        answerPanel.SetActive(true);
        endLevelTextObject.SetActive(false);
        endLevelpanel.SetActive(false);
        timerPanel.SetActive(true);
    }

    public void UpdateTimer(int timeValue)
    {
        timer.GetComponent<Text>().text = timeValue.ToString();
    }

    private void ResetLevelProgress()
    {
        foreach(var level in levelProgressObjects)
        {
            level.GetComponent<Image>().color = props.levelStageCellColor;
            
        }
    }

    private void UpdateLevelProgress(int currentLevelProgress)
    {
        ResetLevelProgress();
        for (int i = 0; i < currentLevelProgress; i++)
        {
            levelProgressObjects[i].GetComponent<Image>().color = Color.green;
        }
    }

    public void ActiveteEndLevelUI(string endLevelText, Color textColor)
    {
        timer.SetActive(false);
        levelProgressBar.SetActive(false);
        answerPanel.SetActive(false);
        endLevelTextObject.GetComponent<Text>().text = endLevelText;
        endLevelTextObject.GetComponent<Text>().color = textColor;
        endLevelTextObject.SetActive(true);
        endLevelpanel.SetActive(true);
        timerPanel.SetActive(false);
    }

    private void ShuffleColors()
    {
        for (int t = 0; t < props.colors.Count; t++)
        {
            Color tmp = props.colors[t];
            int r = Random.Range(t, props.colors.Count);
            props.colors[t] = props.colors[r];
            props.colors[r] = tmp;
        }
    }

    private void UpdateCurrentLevel()
    {
        currentLevelText.GetComponent<Text>().text = string.Format("Level: {0}", gameController.GetCurrentLevel());
    }

    public void ResetButtonsColor()
    {
        ShuffleColors();
    }

    private void Update()
    {
        UpdateTimer(gameTimer.GetCurrentTime());
        UpdateCurrentLevel();
    }
}
