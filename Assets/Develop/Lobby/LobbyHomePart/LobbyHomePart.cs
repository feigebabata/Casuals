using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;
using UnityEngine.UI;
using Skiing;

namespace Lobby
{
    [UIPanelLoader("Assets/Develop/Lobby/LobbyHomePart/LobbyHomePartPanel.prefab",(int)UIPanelSortOrder.Base)]
    public partial class LobbyHomePart : Part
    {
        private LobbyPlay _play;
        private LobbyHomePartPanelComps _panelComps;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as LobbyPlay;
            yield return base.OnCreating(play,parent);

        }

        public override IEnumerator OnPreload()
        {
            yield return base.OnPreload();
            _panelComps = _uiPanel.Comp<LobbyHomePartPanelComps>();
            addListener();

            generateGameList();
        }

        protected override void OnDispose()
        {
            removeListener();
            base.OnDispose();
        }

        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();
        }

        void OnClickPlayBtn()
        {
            if(_play.SelfConfig.SelectGameId.IsNull())return;

            var data = GlobalConfig.Configs.SubGames[_play.SelfConfig.SelectGameId];
            Part.Create(GetType().Assembly,data.Type).OnCreating(default,default).StartByGCS();
            Hide();
        }

        public void Show()
        {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;

            
            loadSceneAsync("Assets/Develop/Lobby/Lobby.unity");

            GlobalLoading.I.Hide();
            
            ShowPanel();
            resetGameListSelect();            
        }

        public void Hide()
        {
            HidePanel();
        }

        private void OnClickGameItem(PointerClicker clicker)
        {
            _play.SelfConfig.SelectGameId = clicker.Data.Get<string>();
            resetGameListSelect();
        }

        private void resetGameListSelect()
        {
            var datas = GlobalConfig.Configs.SubGames;
            _panelComps.ItemListRoot.Foreach(datas.Values,(Action<PointerClicker, ExcelConfig.GameConfigEC.SubGame>)((comp,data)=>
            {
                comp.GetChild(2).SetActive(_play.SelfConfig.SelectGameId == comp.Data.Get<string>());
            }));
        }

        private void generateGameList()
        {
            var datas = GlobalConfig.Configs.SubGames;
            _panelComps.ItemListRoot.Foreach<PointerClicker,ExcelConfig.GameConfigEC.SubGame>(datas.Values,(comp,data)=>
            {
                comp.Data.Set(data.Id);
                comp.GetChild<Image>(0).sprite = AssetHelper.Load<Sprite>(data.Icon);
                comp.OnClick += OnClickGameItem;
            });
        }
    }
}

