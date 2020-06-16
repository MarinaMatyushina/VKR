using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int currentLevel = 0;
    private int difficultyLevel = 1;
    private int difficultyForUILevel = 1;
    private bool isLevelEnd;

    private FigureCreator shapeCreator;
    private UIController UI;
    private GameSceneCreator gameSceneCreator;
    private GameTimer timer;
    private Properties props;
    private AudioSource player;
    private ShapeRotator rotator;
    private GameObject broken;
    public enum LEVEL_STATE
    {
        WIN, LOSE
    }

    #region Start functions
    private void CreateGameScene()
    {
        gameSceneCreator.CreateGameScene();
    }
    private void StartTimer()
    {
        timer.StartTick();
    }
    private void InitializeUI()
    {
        UI.InitializeUI(currentLevel);
    }
    private void GetAllNecessaryGameObjects()
    {
        props = GameObject.FindObjectOfType<Properties>();
        UI = GameObject.FindObjectOfType<UIController>();
        gameSceneCreator = GameObject.FindObjectOfType<GameSceneCreator>();
        timer = GameObject.FindObjectOfType<GameTimer>();
        player = GameObject.FindObjectOfType<AudioSource>();
        rotator = GameObject.FindObjectOfType<ShapeRotator>();
    }
    private void SetGameIsNotOver()
    {
        isLevelEnd = false;
    }
    #endregion
    public void StartGame()
    {
        GetAllNecessaryGameObjects();
        SetGameIsNotOver();
        CreateGameScene();
        StartTimer();
        InitializeUI();
        RotateShapesAroundCube();
    }

    private void RotateShapesAroundCube()
    {        
        rotator.Rotate();
    }
    

    #region Next level functions
    private void NextLevel()
    {
        Destroy(broken);
        broken = null;
        isLevelEnd = false;
        if (currentLevel >= props.maxLevel)
        {
            difficultyLevel++;
            difficultyForUILevel++;
            timer.maxTimerValue = timer.GetMaxTime() - difficultyLevel * 3;
            props.DecreasceCurrentDelta();
            currentLevel = 0;
        }
        timer.ResetTimer(timer.GetMaxTime());
        timer.StartTick();
        UI.InitializeUI(currentLevel);
        gameSceneCreator.CreateGameScene();
        RotateShapesAroundCube();
        timeout = false;
    }
    IEnumerator NextLevelCoroutine()
    {
        Debug.LogError("Start next level creating");
        timer.StopTick();
        yield return new WaitForSeconds(props.pauseBetweenLevels);
        NextLevel();
    }
    #endregion
    private void StartNextLevel()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    #region End level actions
    private void IncreaseCurrentLevelStage()
    {
        currentLevel++;
    }

    private Color SetupEndLevelTextColor(LEVEL_STATE state)
    {
        Color color = props.winEndLevelTextColor;
        if (state != LEVEL_STATE.WIN) color = props.loseEndLevelTextColor;
        return color;
    }

    private string SetupEndLevelText(LEVEL_STATE state)
    {
        string endLevelText = GetEndLevelText(props.endLevelWinText);
        if (state != LEVEL_STATE.WIN) endLevelText = GetEndLevelText(props.endLevelLoseText);
        return endLevelText;
    }

    private string GetEndLevelText(string[] endLevelTextArray)
    {
        return endLevelTextArray[Random.Range(0, endLevelTextArray.Length)];
    }

    private void PlayEndLevelSound(LEVEL_STATE state)
    {
        if (state != LEVEL_STATE.WIN)
        {
            props.loseSound.Play();
        }
        else
        {
            props.winSound.Play();
        }
    }
    #endregion
    private MoveToCube choosen;
    LEVEL_STATE endLevelState;
    public void EndLevel(LEVEL_STATE state, MoveToCube choosenOne)
    {
        timer.StopTick();
        choosen = choosenOne;
        endLevelState = state;       
    }

    public string GetCurrentLevel()
    {
        return string.Format("{0}-{1}", difficultyForUILevel, currentLevel);
    }

    private void ChangeCubeColor(Color color)
    {
        GameObject cube = gameSceneCreator.GetLevelCube();
        cube.GetComponent<Renderer>().material.color = color;
        Component[] t = cube.transform.GetComponentsInChildren<Renderer>();
        foreach(Renderer a in t)
        {
            a.material.color = color;
        }

    }

    bool timeout = false;
    private void Update()
    {
        if (timer == null) return;

        if (choosen != null && !isLevelEnd)
        {
            if (!choosen.IsOver()) return;
            if (endLevelState == LEVEL_STATE.WIN)
            {
                ChangeCubeColor(new Color(0, 255, 0, 0.5f));
            }
            if (endLevelState == LEVEL_STATE.LOSE)
            {
                
                float shapeScale = choosen.transform.localScale.x;
                float cubeScale = gameSceneCreator.GetLevelCube().transform.localScale.x;
                if(shapeScale > cubeScale)
                {
                    GameObject cube = gameSceneCreator.GetLevelCube();
                    broken = Instantiate(gameSceneCreator.brokenCubePrefab);
                    broken.transform.position = cube.transform.position;
                    broken.transform.localScale = cube.transform.localScale;
                    Destroy(cube);
                }
                if(shapeScale < cubeScale)
                {
                    ChangeCubeColor(new Color(255, 0, 0, 0.5f));
                }

            }

            Debug.Log("Is over: " + choosen.IsOver());
            isLevelEnd = true;
            PlayEndLevelSound(endLevelState);
            if (endLevelState == LEVEL_STATE.WIN)
            {
                IncreaseCurrentLevelStage();
            }
            else
            {
                currentLevel = 0;
            }
            UI.ActiveteEndLevelUI(SetupEndLevelText(endLevelState), SetupEndLevelTextColor(endLevelState));
            StartNextLevel();
            choosen = null;
        }
        else
        {
            if(!timeout)
            {
                if (timer.GetCurrentTime() <= 0 && !isLevelEnd)
                {
                    timeout = true;
                    EndLevel(LEVEL_STATE.LOSE, null);
                    currentLevel = 0;
                    UI.ActiveteEndLevelUI(SetupEndLevelText(endLevelState), SetupEndLevelTextColor(endLevelState));
                    StartNextLevel();
                    choosen = null;
                }
            }
        }
    }
}