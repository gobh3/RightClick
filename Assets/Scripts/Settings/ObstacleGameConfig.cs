using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class ObstacleConfig
{
    public Dictionary<ObstacleType, float> proportions;
    //public List<Dictionary<ObstacleType, int>> patterns;
}

[Serializable]
public class ObstacleGameConfig
{
    public ObstacleConfig defaultConfig;
    public Dictionary<int, ObstacleConfig> levels;
}
