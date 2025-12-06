using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
[Serializable]
public class PlayerSettings// : ScriptableObject
{
    public int BaseHealth;
    public int BonusHealthPerGame;
    public int BonusHealthPerLevel;
}
