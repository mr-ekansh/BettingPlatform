using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaneMotion : MonoBehaviour
{
    [SerializeField]
    Transform transformBegin;
    [SerializeField]
    Transform transformEnd;
    [SerializeField]
    Transform LowPoint;
    [SerializeField]
    Transform lastPoint;

    void Start()
    {
        StartCoroutine(flyplane());
    }

    IEnumerator flyplane()
    {
        while(true)
        {
            transformBegin.position = Vector3.MoveTowards(transformBegin.position, GetBezierPosition(0.2f), 400*Time.deltaTime);
            if(transformBegin.position.x>=transformEnd.position.x)
            {
                Debug.Log("end coroutine");
                StartCoroutine(lowfly());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator planetakeoff()
    {
        while(true)
        {
            transformBegin.position = Vector3.MoveTowards(transformBegin.position, GetBezierPositiontakeoff(0.2f), 800*Time.deltaTime);
            if(transformBegin.position.x>=lastPoint.position.x)
            {
                Debug.Log("end coroutine");
                yield break;
            }
            yield return null;
        }
    }

    public void takeOffplane()
    {
        StopAllCoroutines();
        StartCoroutine(planetakeoff());
    }

    IEnumerator lowfly()
    {  
        while(true)
        {
            transformBegin.position = Vector3.MoveTowards(transformBegin.position, LowPoint.position, 200*Time.deltaTime);
            if(transformBegin.position.y<=LowPoint.position.y)
            {
                Debug.Log("end coroutine");
                StartCoroutine(flyup());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator flyup()
    {  
        while(true)
        {
            transformBegin.position = Vector3.MoveTowards(transformBegin.position, transformEnd.position, 200*Time.deltaTime);
            if(transformBegin.position.y>=transformEnd.position.y)
            {
                Debug.Log("end coroutine");
                StartCoroutine(lowfly());
                yield break;
            }
            yield return null;
        }
    }

    Vector3 GetBezierPosition(float t)
    {
        Vector3 p0 = transformBegin.position;
        Vector3 p1 = p0+transformBegin.forward;
        Vector3 p3 = transformEnd.position;
        Vector3 p2 = p3-(transformEnd.forward*-1);
        
        return Mathf.Pow(1f-t,3f)*p0+3f*Mathf.Pow(1f-t,2f)*t*p1+3f*(1f-t)*Mathf.Pow(t,2f)*p2+Mathf.Pow(t,3f)*p3;
    }

    Vector3 GetBezierPositiontakeoff(float t)
    {
        Vector3 p0 = transformBegin.position;
        Vector3 p1 = p0+transformBegin.forward;
        Vector3 p3 = lastPoint.position;
        Vector3 p2 = p3-(lastPoint.forward*-1);
        
        // here is where the magic happens!
        return Mathf.Pow(1f-t,3f)*p0+3f*Mathf.Pow(1f-t,2f)*t*p1+3f*(1f-t)*Mathf.Pow(t,2f)*p2+Mathf.Pow(t,3f)*p3;
    }
}
