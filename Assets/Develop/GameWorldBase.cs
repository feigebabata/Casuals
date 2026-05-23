using System;
using System.Collections;
using System.Collections.Generic;
using FGUFW;
using UnityEngine;

public abstract class GameWorldBase : MonoBehaviour
{
    //游戏速度
    public float Speed=1;

    //游戏暂停
    public bool IsPause;

    public float Score;

    //逻辑更新 外部调用
    public void LogicUpdate()
    {
        if(IsPause)return;

        OnLogicUpdate();
    }

    protected abstract void OnLogicUpdate();

    public virtual void OnGameInit()
    {
        Score = default;
    }
    public abstract void OnGameStart();
    protected abstract void OnGameOver();

    //复活
    protected virtual void OnGameRevive()
    {
        Score = default;
        IsPause = false;
    }
    
    //重开
    protected virtual void OnGameRestart()
    {
        Score = default;
        IsPause = false;
    }

    //游戏失败
    protected void onGameFail(int score)
    {
        IsPause = true;

        Action<bool> failCallback = (succ)=>
        {
            if(succ)
            {
                OnGameRevive();
                IsPause = false;
            }
            else
            {
                Action<bool> overCallback = (succ)=>
                {
                    if(succ)
                    {
                        OnGameRestart();
                    }
                    else
                    {
                        OnGameOver();
                        GlobalMessenger.M.Broadcast(Lobby.LobbyPlayMsgId.OnClickQuit);
                    }
                };

                GlobalMessenger.M.Broadcast(Lobby.LobbyPlayMsgId.OpenGameOverPopup,overCallback);
            }
        };

        GlobalMessenger.M.Broadcast(Lobby.LobbyPlayMsgId.OpenFailPopup,score,failCallback);
    }
}
