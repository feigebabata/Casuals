using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace Lobby
{
    public partial class SettingPart : Part
    {
        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        [Inject(InjectField.UI)]
        private SettingPartPanelComps _panelComps;

        protected override void OnPartInitialed()
        {
            addListener();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
        }

        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);

            _orderedMessenger.Add(LobbyPlayMsgId.OpenSettingPart,Show);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            _orderedMessenger.Remove(LobbyPlayMsgId.OpenSettingPart,Show);
        }

        internal void Show()
        {
            _panelComps.SetActive(true);
        }

        void OnClickCloseBtn()
        {
            _panelComps.SetActive(false);
        }

        void OnClickQuitBtn()
        {
            OnClickCloseBtn();
            _orderedMessenger.Broadcast(LobbyPlayMsgId.OnClickQuit);
        }
    }
}

