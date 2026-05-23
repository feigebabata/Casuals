using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using System;
using UnityEngine.UI;
using Skiing;
using TMPro;
using System.Linq;

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
            foreach (var k in _subGameConfigs.Where((kv=>!kv.Value.Enabled)).Select(kv=>kv.Key).ToList())
            {
                _subGameConfigs.Remove(k);
            }

            addListener();

            generateGameList();

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
                comp.GetChild("select").SetActive(_lobbyPlayConfig.SelectGameId == comp.Data.Get<string>());
            }));
            _panelComps.PlayBtn.interactable = !_lobbyPlayConfig.SelectGameId.IsNull();
            _panelComps.PlayBtn.GetChild("disable").SetActive(_lobbyPlayConfig.SelectGameId.IsNull());
        }

        private void generateGameList()
        {
            _panelComps.ItemListRoot.Foreach<PointerClicker,ExcelConfig.GameConfigEC.SubGame>(_subGameConfigs.Values,(comp,data)=>
            {
                comp.GetChild<TMP_Text>("name").text = data.Name;

                comp.Data.Set(data.Id);
                comp.OnClick += OnClickGameItem;

                comp.SetActive(data.Enabled);
            });
        }
    }
}

