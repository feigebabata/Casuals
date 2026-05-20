using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using System;
using UnityEngine.UI;
using Skiing;

namespace Lobby
{
    public partial class LobbyHomePart : Part
    {
        [Inject(InjectField.Save)]
        LobbyPlayConfig _lobbyPlayConfig;

        [Inject(InjectField.UI)]
        LobbyHomePartPanelComps _panelComps;

        [Inject]
        Table<string,ExcelConfig.GameConfigEC.SubGame> _subGameConfigs;

        [Inject]
        IAssetLoader _assetLoader;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        protected override void OnPartInitialed()
        {
            addListener();

            generateGameList();

            Show();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
            GameObject.Destroy(_panelComps.gameObject);
        }

        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);

            _orderedMessenger.Add(LobbyPlayMsgId.OpenLobby,OnOpenLobby);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            _orderedMessenger.Remove(LobbyPlayMsgId.OpenLobby,OnOpenLobby);
        }

        private void OnOpenLobby()
        {
            Show();
        }

        void OnClickPlayBtn()
        {
            if(_lobbyPlayConfig.SelectGameId.IsNull())return;

            var data = _subGameConfigs[_lobbyPlayConfig.SelectGameId];
            _assetLoader.LoadSceneAsync(data.ScenePath);

            Hide();
        }

        public void Show()
        {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;


            GlobalLoading.I.Hide();
            
            _panelComps.Show();
            resetGameListSelect();            
        }

        public void Hide()
        {
            _panelComps.Hide();
            
        }

        private void OnClickGameItem(PointerClicker clicker)
        {
            _lobbyPlayConfig.SelectGameId = clicker.Data.Get<string>();
            resetGameListSelect();
        }

        private void resetGameListSelect()
        {
            _panelComps.ItemListRoot.Foreach(_subGameConfigs.Values,(Action<PointerClicker, ExcelConfig.GameConfigEC.SubGame>)((comp,data)=>
            {
                comp.GetChild(2).SetActive(_lobbyPlayConfig.SelectGameId == comp.Data.Get<string>());
            }));
        }

        private void generateGameList()
        {
            _panelComps.ItemListRoot.Foreach<PointerClicker,ExcelConfig.GameConfigEC.SubGame>(_subGameConfigs.Values,(comp,data)=>
            {
                comp.Data.Set(data.Id);
                comp.GetChild<Image>(0).sprite = AssetHelper.Load<Sprite>(data.Icon);
                comp.OnClick += OnClickGameItem;
            });
        }
    }
}

