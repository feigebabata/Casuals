using System;
using FGUFW;
using FGUFW.Gameplay;
using UnityEngine;

namespace Skiing
{
    public class GameOverPart : Part
    {
        [Inject(InjectField.UI)]
        private GameOverPartPanelComps _panelComps;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;
        private Action<bool> _callbacl;

        protected override void OnPartInitialed()
        {
            addListener();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
            _panelComps.Release();
        }

        private void addListener()
        {
            _panelComps.TryAddAllBtnListener(this);

            _orderedMessenger.Add<Action<bool>>(Lobby.LobbyPlayMsgId.OpenGameOverPopup,OnOpenGameOverPopup);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            _orderedMessenger.Remove<Action<bool>>(Lobby.LobbyPlayMsgId.OpenGameOverPopup,OnOpenGameOverPopup);
        }

        private void OnOpenGameOverPopup(Action<bool> callback)
        {
            _panelComps.Show();

            _callbacl = callback;
        }

        void OnClickRestartBtn()
        {
            _callbacl(true);

            _panelComps.Hide();
        }

        void OnClickBackBtn()
        {
            _callbacl(false);

            _panelComps.Hide();
        }

    }
}
