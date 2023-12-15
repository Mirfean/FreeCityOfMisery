using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrentScore : MonoBehaviour
{
    [SerializeField] List<scoreCounter> ScoreCounterList;
    [SerializeField] Dictionary<PowerType, scoreCounter> ScoreUpdater;
    // Start is called before the first frame update
    void Start()
    {
        ScoreUpdater = new Dictionary<PowerType, scoreCounter>();
        foreach (var score in ScoreCounterList)
        {
            ScoreUpdater.Add(score.powerType, score);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(Dictionary<int, List<PowerType>> item2)
    {
        foreach (KeyValuePair<int, List<PowerType>> keyValues in item2)
        {
            if (keyValues.Value.Count > 0) AddFromSide(keyValues.Value);
        }
    }

    void AddFromSide(List<PowerType> powers)
    {
            foreach (PowerType powerType in powers)
            {
                AddToCounter(powerType);
            }

    }

    public void AddToCounter(PowerType power, int value = 1)
    {
        ScoreUpdater[power].AddToScore(value);
        if (ScoreUpdater[power].value < 0) ScoreUpdater[power].SetScoreValue(0);
    }

    public Dictionary<PowerType, int> GetAllPoints()
    {
        var result = new Dictionary<PowerType, int>();
        foreach (var power in ScoreUpdater)
        {
            result.Add(power.Key, ScoreUpdater[power.Key].value);
        }

        return result;
    }

    public int GetSum()
    {
        var scores = GetAllPoints();
        int sum = 0;
        foreach(var score in scores)
        {
            sum += score.Value;
        }

        return sum;
    }
}
