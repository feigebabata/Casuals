using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using Lobby;
using System;

namespace Skiing
{
    public partial class MainPart : Part
    {

        [Inject(InjectField.UI)]
        private MainPartPanelComps _panelComps;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        [Inject]
        private ILoadingUI _loadingUI;

        private SkiingLineTerrainComp _terrainComp;

        protected override void OnPartInitialed()
        {
            addListener();
            
            _terrainComp = findGO<SkiingLineTerrainComp>("Terrain");
            Show();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
        }


        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);

            _orderedMessenger.Add(LobbyPlayMsgId.OnClickQuit,OnClickQuit,10);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            _orderedMessenger.Remove(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        public void Show()
        {
            _panelComps.SetActive(true);
            _terrainComp.Step();
        }

        public void Hide()
        {
            _panelComps.SetActive(false);
        }

        private void OnClickQuit()
        {
            _loadingUI.Show();
            
            _orderedMessenger.Abort(LobbyPlayMsgId.OnClickQuit);
            // _play.Destroy();
            // LobbyPlay.P.GetPart<LobbyHomePart>().Show();
        }

        void OnClickPlayBtn()
        {
            Hide();

            gameLoop().Start(this);
        }

        IEnumerator gameLoop()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                _terrainComp.Step();
            }
        }
    }
}

