using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderPointTracker : MonoBehaviour
{
    public List<Vector2> Radius_FallOff;
    public float RadiusGrowSpeed;
    [Header("Can Use Function SetRadius to change the size of radius")]
    public int RadiusIndex;

    Vector4 tempVec;
    Coroutine RadiusCoroutine;
    float previousIndex;

    public float CurrentRadius = 0;
    float CurrentFallOff = 0;

    private void Start()
    {
        previousIndex = RadiusIndex + 1;
    }

    private void Update()
    {
        tempVec = transform.position;
        Shader.SetGlobalVector("GLOBAL_WorldPos", tempVec);
        if (RadiusCoroutine == null && previousIndex != RadiusIndex)
        {
            RadiusIndex = Mathf.Clamp(RadiusIndex, 0, Radius_FallOff.Count - 1);
            previousIndex = RadiusIndex;
            RadiusCoroutine = StartCoroutine(RadiusChange());
        }
    }

    public void SetRadius(int i)
    {
        RadiusIndex = i;
    }

    IEnumerator RadiusChange()
    {
        float targetRadius = Radius_FallOff[RadiusIndex].x;
        float StartRadius = CurrentRadius;
        float targetFallOff = Radius_FallOff[RadiusIndex].y;
        float StartFallOff = CurrentFallOff;
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime * RadiusGrowSpeed;
            CurrentRadius = Mathf.Lerp(StartRadius, targetRadius, elapsedTime);
            CurrentFallOff = Mathf.Lerp(StartFallOff, targetFallOff, elapsedTime);
            Shader.SetGlobalFloat("GLOBAL_Radius", CurrentRadius);
            Shader.SetGlobalFloat("GLOBAL_FallOff", CurrentFallOff);
            if (previousIndex != RadiusIndex)
                break;
            yield return new WaitForEndOfFrame();
        }

        RadiusCoroutine = null;
        yield return null;
    }

}
