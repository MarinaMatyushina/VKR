using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static GameController;

public class MoveToCube : MonoBehaviour
{
    private Vector3 endPoint;
    bool startMove = false;
    bool allTransparent = false;
    bool disTransparent = false;
    bool allIsOver = false;
    private Renderer renderer;
    public LEVEL_STATE state;
    private float changeColorValue = 0.01f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public bool IsOver()
    {
        return allIsOver;
    }

    public void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }

    public void Move()
    {
        startMove = true;
    }
    bool changedColor = false;

    private void Update()
    {
        if (startMove)
        {
            Color currentColor = renderer.material.color;
            startMove = false;

            Debug.LogError("Shape is full transparent");
            StartCoroutine(WaitForAnimation());
            
            if (!changedColor)
            {
                if (state == LEVEL_STATE.WIN)
                {
                    Debug.LogError("Change shape color to red");
        //                    currentColor = Color.green;
                }
                else
                {
                    Debug.LogError("Change shape color to red");
        //                    currentColor = Color.red;
                }
        
                changedColor = true;
        //                currentColor.a = 0;
                Debug.LogError("Change shape transparency to 0");
            }
        
           
        }
    }

    IEnumerator WaitForAnimation()
    {
        WaitForSeconds wait = new WaitForSeconds( 0.001f ) ;
        while (gameObject.transform.localPosition != endPoint)
        {
            gameObject.transform.localPosition = Vector3.SmoothDamp( gameObject.transform.localPosition, endPoint, ref velocity, 1F);
            
            yield return wait ;
        }
        allIsOver = true;
    }
}
