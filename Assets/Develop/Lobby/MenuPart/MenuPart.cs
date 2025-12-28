using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;
using UnityEngine.UI;
using TMPro;

namespace Lobby
{
    [UIPanelLoader("Assets/Develop/Lobby/MenuPart/MenuPartPanel.prefab",(int)UIPanelSortOrder.Upper)]
    public partial class MenuPart : Part
    {
        private LobbyPlay _play;
        private MenuPartPanelComps _panelComps;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as LobbyPlay;
            yield return base.OnCreating(play,parent);
        }

        public override IEnumerator OnPreload()
        {
            yield return base.OnPreload();
            _panelComps = _uiPanel.Comp<MenuPartPanelComps>();
            addListener();
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

        public void Show()
        {
            ShowPanel();

            resetItemList();
        }

        void OnClickSettingBtn()
        {
            _play.GetPart<SettingPart>().Show();
        }

        private void resetItemList()
        {
            var datas = GlobalConfig.Configs.Items.Values;
            _panelComps.ItemList.Foreach<PointerClicker,ExcelConfig.GameConfigEC.Item>(datas,(comp,data)=>
            {
                comp.GetChild<Image>(0).sprite = load<Sprite>(data.Icon);
                comp.GetChild<Image>(0).SetSizeFlexibleHeight();

                comp.GetChild<TMP_Text>(1).text = _play.SelfConfig.GetItemCount(data.Id).ts();
            });
        }

    }
}

