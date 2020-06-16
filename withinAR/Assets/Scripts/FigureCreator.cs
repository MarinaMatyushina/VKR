using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureCreator : MonoBehaviour
{
    public Vector3 minValues;
    public Vector3 maxValues;
    public GameObject gameZone;
    private float currentScale;

    private List<GameObject> spheres = new List<GameObject>();
    private Properties props;
    public GameObject spherePrefab;
    private void Awake()
    {
        props = FindObjectOfType<Properties>();
        currentScale = props.minScale;
    }

    public float GetCurrentScale()
    {
        return currentScale;
    }

    // Генрирует сферы разного диаметра и цвета
    public GameObject createSphere(Color color)
    {
        GameObject sphere = Instantiate(spherePrefab);
        sphere.tag = "Sphere";
        sphere.GetComponent<Renderer>().material.SetColor("_Color", color);
        sphere.transform.SetParent(gameZone.transform);        
        sphere.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
        spheres.Add(sphere);

        if (currentScale >= props.maxScale)
        {
            currentScale = props.minScale + props.initialScaleDelta;
        }
        else
        {
            currentScale += props.initialScaleDelta;
        }
        return sphere;
    }

    public List<GameObject> GetShapes()
    {
        return spheres;
    }

    public Vector3 GetRandomShapeScale()
    {
        int shapeIndex = Random.Range(0, spheres.Count);
        GameObject shape = spheres[shapeIndex];
        return shape.transform.localScale;
    }

    public void DestroyAllShapes()
    {
        foreach(GameObject sphere in spheres)
        {
            Destroy(sphere);
        }
        spheres.Clear();
    }
}
