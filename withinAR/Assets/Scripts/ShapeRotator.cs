using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRotator : MonoBehaviour
{
    private List<GameObject> shapes;
    private bool isActive = false;
    public Transform rotationAxis;
    private Vector3 axis;
    public float rotationSpeed;
    private FigureCreator creator;

    private void Awake()
    {
        creator = GameObject.FindObjectOfType<FigureCreator>();
    }

    public void AddShape(GameObject shape)
    {
        shapes.Add(shape);
    }

    public void Rotate()
    {
        shapes = creator.GetShapes();
    }

    private void Update()
    {
        foreach(var shape in shapes)
        {
            shape.transform.RotateAround(rotationAxis.transform.position, Vector3.up, rotationSpeed*Time.deltaTime);
        }
    }
}
