using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    public float highlightSize = 1.15f;
    public float highlightSpeed = 2f;

    float CurrentFloat;
    Material mat;
    Coroutine routineTemp;

    public void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    public void TurnOnHighlights()
    {
        if(routineTemp != null)
            StopCoroutine(routineTemp);
        routineTemp = StartCoroutine(LerpHighlights(true));
    }

    IEnumerator LerpHighlights(bool onBool)
    {
        float targetFloat = highlightSize;
        float StartFloat = CurrentFloat;
        if(!onBool)
        {
            targetFloat = 1;
            StartFloat = CurrentFloat;
        }
        float ElapsedTime = 0;
        while(ElapsedTime < 1)
        {
            ElapsedTime += Time.deltaTime * highlightSpeed;
            CurrentFloat = Mathf.Lerp(StartFloat, targetFloat, ElapsedTime);
            mat.SetFloat("_Outline", CurrentFloat);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void TurnOffHighlights()
    {
        if (routineTemp != null)
            StopCoroutine(routineTemp);
        routineTemp = StartCoroutine(LerpHighlights(false));
    }
}
