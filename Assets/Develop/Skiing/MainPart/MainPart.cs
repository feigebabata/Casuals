using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using Lobby;
using System;

namespace Skiing
{
    [UIPanelLoader("Assets/Develop/Skiing/MainPart/MainPartPanel.prefab",(int)UIPanelSortOrder.Base)]
    public partial class MainPart : Part
    {
        private SkiingPlay _play;
        private MainPartPanelComps _panelComps;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            _play = play as SkiingPlay;
            yield return base.OnCreating(play,parent);
        }

        public override IEnumerator OnPreload()
        {
            yield return base.OnPreload();
            _panelComps = _uiPanel.Comp<MainPartPanelComps>();
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

            LobbyPlay.P.Messenger.Add(LobbyPlayMsgId.OnClickQuit,OnClickQuit,10);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            LobbyPlay.P.Messenger.Remove(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        public void Show()
        {
            ShowPanel();
        }

        public void Hide()
        {
            HidePanel();
        }

        void OnClickPlayBtn()
        {
            
        }

        private void OnClickQuit()
        {
            GlobalLoading.I.Show();
            
            LobbyPlay.P.Messenger.Abort(LobbyPlayMsgId.OnClickQuit);
            _play.Destroy();
            LobbyPlay.P.GetPart<LobbyHomePart>().Show();
        }

    }
}

