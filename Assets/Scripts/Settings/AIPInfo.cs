using System;
using UnityEngine;

public class AIPInfoManager : MonoBehaviour
{
    [Serializable]
    public class AIPInfo
    {
        public bool PLUS_2_HEARTS_PREMIUM = false;
        public bool PLUS_2_HEARTS_REGULAR = false;
        public int PLUS_HEARTS_FROM_SHARING = 0;
    }
    public string fileName = "AIPInfo";
    private ClassSaver<AIPInfo> saver;
    public void Initialize()
    {
        saver = new ClassSaver<AIPInfo>(fileName);
    }

    public void OnPurchasePlus2HeartsPremium()
    {
        saver.Data.PLUS_2_HEARTS_PREMIUM = true;
        saver.SaveToFile();
    }
    
    public void OnPurchasePlus2HeartsRegular()
    {
        saver.Data.PLUS_2_HEARTS_REGULAR = true;
        saver.SaveToFile();
    }

    public void OnShareForPlusHearts()
    {
        saver.Data.PLUS_HEARTS_FROM_SHARING += 1;
        saver.SaveToFile();
    }

    public AIPInfo GetInfo() { return saver.Data; }
}
