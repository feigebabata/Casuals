using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using System;
using UnityEngine.UI;
using TMPro;

namespace Lobby
{
    
    public partial class MenuPart : Part
    {
        [Inject(InjectField.UI)]
        private MenuPartPanelComps _panelComps;

        [Inject]
        private Table<string,ExcelConfig.GameConfigEC.Item> _itemConfigs;

        [Inject(InjectField.Save)]
        LobbyPlayConfig _lobbyPlayConfig;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        [Inject]
        private IAssetLoader _assetLoader;

        protected override void OnPartInitialed()
        {
            addListener();
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

            resetItemList();
        }

        void OnClickSettingBtn()
        {
            _orderedMessenger.Broadcast(LobbyPlayMsgId.OpenSettingPart);
        }

        private void resetItemList()
        {
            _panelComps.ItemList.Foreach<PointerClicker,ExcelConfig.GameConfigEC.Item>(_itemConfigs.Values,(comp,data)=>
            {
                // comp.GetChild<Image>(0).sprite = _assetLoader.Load<Sprite>(data.Icon);
                // comp.GetChild<Image>(0).SetSizeFlexibleHeight();

                comp.GetChild<TMP_Text>("value").text = _lobbyPlayConfig.GetItemCount(data.Id).ts();
                comp.GetChild<TMP_Text>("name").text = data.Name;
            });
        }
    }
}

