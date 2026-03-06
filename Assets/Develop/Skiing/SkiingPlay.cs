using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using System;

namespace Skiing
{
    public class SkiingPlay : Play
    {
        private SkiingLineTerrainComp _terrainComp;

        [Inject]
        private ILoadingUI _loadingUI;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        protected override void OnPartInitialed()
        {
            _terrainComp = FindFirstObjectByType<SkiingLineTerrainComp>();

            _loadingUI.Hide();
            
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;

            base.OnPartInitialed();
            
            addListener();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
            
        }

        protected override void OnAllPartInitialed()
        {
            _terrainComp.Init();
        }

        private void addListener()
        {
            _orderedMessenger.Add(Lobby.LobbyPlayMsgId.OnClickQuit,OnClickQuit,10);
        }

        private void removeListener()
        {
            _orderedMessenger.Remove(Lobby.LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        private void OnClickQuit()
        {
            _loadingUI.Show();
            
            _orderedMessenger.Abort(Lobby.LobbyPlayMsgId.OnClickQuit);
            
            PartDIContainer.DestroyPlay<SkiingPlay>();
            
            _orderedMessenger.Broadcast(Lobby.LobbyPlayMsgId.OpenLobby);
        }


    }
}

