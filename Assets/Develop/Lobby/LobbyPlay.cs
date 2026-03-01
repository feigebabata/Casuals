using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using System;

namespace Lobby
{
    public partial class LobbyPlay : Play
    {
        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        [Inject]
        ILoadingUI _loadingUI;

        protected override void OnPartInitialed()
        {

            addListener();

            base.OnPartInitialed();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
        }

        protected override void OnAllPartInitialed()
        {
            _loadingUI.Hide();
        }

        private void addListener()
        {
            _orderedMessenger.Add(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        private void removeListener()
        {
            _orderedMessenger.Remove(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        private void OnClickQuit()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }
    }
}

