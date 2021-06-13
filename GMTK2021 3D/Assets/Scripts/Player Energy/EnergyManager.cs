using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnergyManager : MonoBehaviour
{
    // ENERGY UI
    [SerializeField]
    private Text textEnergy;

    /*private int textEnergyno;*/

    [SerializeField]
    private Text textTimer;

    private int textTimerno;

    //ENERGY VARIABLES 
    [SerializeField]
    private int minEnergy; //maxEnergy in the vid
    [SerializeField]
    private int totalEnergy;


    // TIME
    private DateTime lastDepletedTime; //After 10 seconds depletion
    private DateTime nextEnergyTime; //Time at which one percent is depleted next. 
    
    public int restoreDuration = 10;
    public int timePenalty = 0;
    //Check for extra parts
    public bool extraComponent = false; 

    //ROGUE PARTS
    public bool rogueArmLeft = false;
    public bool rogueArmRight = false;
    public bool rogueLegRight = false;
    public bool rogueLegLeft = false; 
    

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("totalEnergy"))
        {
            PlayerPrefs.SetInt("totalEnergy", 30);
            Load();
            StartCoroutine(RestoreRoutine());
        }
        else
        {
            Load();
            StartCoroutine(RestoreRoutine());
        }
        
    }
    

    private void UpdateRestoreDuration()
    {
        if (extraComponent)
        {
            if (rogueArmLeft)
            {
                timePenalty += 1;
            }
            if (rogueArmRight)
            {
                timePenalty += 1;
            }
            if (rogueLegLeft)
            {
                timePenalty += 1;
            }
            if (rogueLegRight)
            {
                timePenalty += 1;
            }
            restoreDuration -= timePenalty;
            Debug.Log(restoreDuration);
        }
        else
        {
            timePenalty = 0;
            restoreDuration = 10;
        }
    }
    private IEnumerator RestoreRoutine()
    {
        UpdateColor();
        //UpdateRestoreDuration();
        UpdateTimer();
        UpdateEnergy();
        while (totalEnergy > minEnergy)
        {
            DateTime currentTime = DateTime.Now;
            DateTime counter = nextEnergyTime;

            bool isAdding = false;
            while (currentTime > counter)
            {
                if (totalEnergy > minEnergy)
                {
                    isAdding = true;
                    totalEnergy--;
                    DateTime timeToAdd = lastDepletedTime > counter ? lastDepletedTime : counter;
                    counter = AddDuration(timeToAdd, restoreDuration);
                }
                else
                    break;
            }

            if (isAdding)
            {
                lastDepletedTime = DateTime.Now;
                nextEnergyTime = counter; 
            }
            UpdateColor();
            //UpdateRestoreDuration();
            UpdateTimer();
            UpdateEnergy();
            Save();
            yield return null; 
        }

    }

    private void UpdateColor()
    {
        if(totalEnergy > 25)
        {
            textEnergy.color = Color.green;
        }
        else if(totalEnergy <= 25 && totalEnergy > 10)
        {
            textEnergy.color = Color.yellow;
        }
        else
        {
            textEnergy.color = Color.red;
        }

    }
    private void UpdateTimer()
    {
        if(totalEnergy <= minEnergy)
        {
            textTimer.text = "Empty";
            return;
        }
        TimeSpan t = nextEnergyTime - DateTime.Now;
        string value = string.Format("{0}:{1:02}:{2:02}",(int) t.TotalHours, t.Minutes, t.Seconds);

        textTimer.text = value; 
    }

    private void UpdateEnergy()
    {
        textEnergy.text = totalEnergy.ToString();
    }

    private DateTime AddDuration(DateTime time, int duration)
    {
        return time.AddSeconds(duration);
        //return time.AddMinutes(duration);
    }
    private void Load()
    {
        totalEnergy = PlayerPrefs.GetInt("totalEnergy");
        nextEnergyTime = StringToDate(PlayerPrefs.GetString("nextEnergyTime"));
        lastDepletedTime = StringToDate(PlayerPrefs.GetString("lastDepletedTime"));

    }

    private void Save()
    {
        PlayerPrefs.SetInt("totalEnergy", totalEnergy);
        PlayerPrefs.SetString("nextEnergyTime", nextEnergyTime.ToString());
        PlayerPrefs.SetString("lastDepletedTime", lastDepletedTime.ToString());

    }

    private DateTime StringToDate(string date)
    {
        if (String.IsNullOrEmpty(date))
        {
            return DateTime.Now.AddSeconds(restoreDuration);
        }
        else
        {
            return DateTime.Parse(date);
        }
        
    }


}
