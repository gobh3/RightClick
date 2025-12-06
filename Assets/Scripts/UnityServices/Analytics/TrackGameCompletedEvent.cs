using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UIElements;

public class TrackGameCompletedEvent : MonoBehaviour
{
    
    public void Awake()
    {
        initialize();
    }
    public class GameCompleted : Unity.Services.Analytics.Event
    {
        public GameCompleted() : base("GameCompleted")
        {
        }
        public string unityPlayerID { set { SetParameter("unityPlayerID", value); } }
        public int score { set { SetParameter("score", value); } }
        public string gameProcess { set { SetParameter("gameProcess", value); } }

    }

    GameCompleted e = new GameCompleted()
    {
        gameProcess = ""
    };

    bool isInitilizeNewGameEventPassed = false;
    string gameProcess;

     void initialize()
    {
        gameProcess = "";
        e.Reset();
    }
    public void SetScore(int score)
    {
        e.score = score;
    }

    public void SetPassLimitOfPointsCh()
    {
        gameProcess += ">PassLimitOfPointsCh";
    }

    public void SetPass3TimesCh()
    {
        gameProcess += ">Pass3TimesCh";
    }

    public void SetPass3TimesChPartially(string s)
    {
        gameProcess += (">Pass3TimesChPartially:" + s);
    }

    public void SetGameOver(int score)
    {
        gameProcess += (">GameOver:"+score.ToString());
    }


    public void setGameContinues()
    {
        //gameProcess += ">GameContinues";
    }

    public void setPurchse3Hearts()
    {
        gameProcess += ">Purchse3Hearts";
    }

    public void setPurchse8Hearts()
    {
        gameProcess += ">Purchse8Hearts";
    }

    public void RecordGameCompletedEvent()
    {
        if (isInitilizeNewGameEventPassed && gameProcess != "")
        {
            e.unityPlayerID = AuthenticationService.Instance.PlayerId;
            e.gameProcess = gameProcess;
            AnalyticsService.Instance.RecordEvent(e);
            Debug.Log("Record GameCompleted. \n GameProcess: " + gameProcess);
            initialize();
        }
        else if(!isInitilizeNewGameEventPassed)
            isInitilizeNewGameEventPassed=true;
    }

}
