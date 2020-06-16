using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCreator : MonoBehaviour
{
    FigureCreator shapeCreator;
    GameController gameController;
    
    public GameObject center;
    public Vector3 size;
    public GameObject gameZone;

    private Properties props;
    private GameObject levelCube;
    public GameObject levelCubePrefab;
    public GameObject brokenCubePrefab;

    public GameObject getBrokenPrefab()
    {
        return brokenCubePrefab;
    }

    public GameObject GetLevelCube()
    {
        return levelCube;
    }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        shapeCreator = GameObject.FindObjectOfType<FigureCreator>();
        props = GameObject.FindObjectOfType<Properties>();
    }

    private void SetNewLevelCube()
    {
        if(levelCube != null) Destroy(levelCube);
        levelCube = Instantiate(levelCubePrefab);
        levelCube.transform.parent = gameZone.transform;
        levelCube.transform.localPosition = new Vector3(0, 0, 0);//enter.transform.position;
        levelCube.transform.rotation = new Quaternion(0, 0, 0, 0);
        Vector3 scale = shapeCreator.GetRandomShapeScale();
        levelCube.transform.localScale = scale;
        levelCube.tag = "Cube";
    }

    private Vector3 CreateRandomPosition()
    {
        Vector3 newPosition = center.transform.position + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
            );
        return newPosition;
    }

    public void CreateGameScene()
    {
        Debug.LogError("Game scene creating");
        shapeCreator.DestroyAllShapes();
        for (int i = 0; i < props.shapesCount; i++)
        {
            GameObject sphere = shapeCreator.createSphere(props.colors[i]);
            sphere.transform.position = CreateRandomPosition();
            sphere.transform.SetParent(gameZone.transform);
        }
        SetNewLevelCube();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center.transform.position, size);
    }
}
