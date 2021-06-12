using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHP : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float HP;
    public Image coloredPortion;

    // here, show the variables that are only specific to type

    public void Update()
    {
        if (coloredPortion != null)
        {
            coloredPortion.fillAmount = HP / 100;
        }
        if (GetComponent<IconHP>() != null)
        {
            GetComponent<IconHP>().fillAmount = HP / 100;
        }
    }
}