using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
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
            
            _terrainComp = FindFirstObjectByType<SkiingLineTerrainComp>();
            Show();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
            _panelComps.Release();
        }


        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

        }

        public void Show()
        {
            _panelComps.Show();
            _terrainComp.Step();
        }

        public void Hide()
        {
            _panelComps.Hide();
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

