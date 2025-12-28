using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;

namespace Lobby
{
    [UIPanelLoader("Assets/Develop/Lobby/SettingPart/SettingPartPanel.prefab",(int)UIPanelSortOrder.Upper2)]
    public partial class SettingPart : Part
    {
        private LobbyPlay _play;
        private SettingPartPanelComps _panelComps;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as LobbyPlay;
            yield return base.OnCreating(play,parent);
        }

        public override IEnumerator OnPreload()
        {
            yield return base.OnPreload();
            _panelComps = _uiPanel.Comp<SettingPartPanelComps>();
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

        internal void Show()
        {
            ShowPanel();
        }

        void OnClickCloseBtn()
        {
            HidePanel();
        }

        void OnClickQuitBtn()
        {
            HidePanel();
            _play.Messenger.Broadcast(LobbyPlayMsgId.OnClickQuit);
        }

    }
}

