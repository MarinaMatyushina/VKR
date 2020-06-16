using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Properties : MonoBehaviour
{
    public List<Color> colors = new List<Color>();

    public AudioSource winSound;
    public AudioSource loseSound;

    public int maxTimerValue;
    public int maxLevel;
    public int pauseBetweenLevels;

    public string[] endLevelLoseText;
    public string[] endLevelWinText;

    public Color winEndLevelTextColor;
    public Color loseEndLevelTextColor;
    public Color levelStageCellColor;

    public float initialScaleDelta = 0.5f;
    public float decreaseDeltaValue;
    public void DecreasceCurrentDelta()
    {
        if(initialScaleDelta > 0.1f)
        {
            initialScaleDelta -= decreaseDeltaValue;
        }
    }
    public float maxScale;
    public float minScale = 2;
    public int shapesCount = 5;

    public int GetButtonsCount()
    {
        return shapesCount;
    }

    private void Awake()
    {
        colors.Clear();
        for(int i = 0; i < shapesCount; i++)
        {
            colors.Add(Random.ColorHSV());
        }
    }
}
