using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickProcessor : MonoBehaviour
{
    private GameObject levelCube;
    private GameController controller;
    private GameObject[] allSpheres;

    public void Start()
    {
        controller = GameObject.FindObjectOfType<GameController>();
    }

    public void processClick()
    {
        levelCube = GameObject.FindGameObjectWithTag("Cube");
        bool findSphere = false;
        var cubeScale = levelCube.transform.localScale;
        allSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        MoveToCube result = null;
        Color buttonColor = gameObject.GetComponent<Image>().color;
        foreach (var sphere in allSpheres)
        {
            Color sphereColor = sphere.GetComponent<Renderer>().material.color;
            if (sphereColor == buttonColor)
            {
                if (sphere.transform.localScale == cubeScale)
                {
                    findSphere = true;
                    sphere.GetComponent<MoveToCube>().SetEndPoint(levelCube.transform.localPosition);
                    sphere.GetComponent<MoveToCube>().Move();
                    sphere.GetComponent<MoveToCube>().state = GameController.LEVEL_STATE.WIN;
                    result = sphere.GetComponent<MoveToCube>();
                    controller.EndLevel(GameController.LEVEL_STATE.WIN, result);
                    break;
                }
                else
                {
                    sphere.GetComponent<MoveToCube>().SetEndPoint(levelCube.transform.localPosition);
                    sphere.GetComponent<MoveToCube>().Move();
                    sphere.GetComponent<MoveToCube>().state = GameController.LEVEL_STATE.LOSE;
                    result = sphere.GetComponent<MoveToCube>();
                    break;
                }
            }
        }
        
        if(!findSphere) controller.EndLevel(GameController.LEVEL_STATE.LOSE, result);
    }
}
