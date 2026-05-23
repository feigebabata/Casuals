using System;
using FGUFW;
using FGUFW.Gameplay;
using UnityEngine;

namespace Skiing
{
    public class FailPart : Part
    {
        [Inject(InjectField.UI)]
        private FailPartPanelComps _panelComps;

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

            _orderedMessenger.Add<int,Action<bool>>(Lobby.LobbyPlayMsgId.OpenFailPopup,OnOpenFailPopup);
        }

        private void removeListener()
        {
            _panelComps.TryRemoveAllBtnListener();

            _orderedMessenger.Remove<int,Action<bool>>(Lobby.LobbyPlayMsgId.OpenFailPopup,OnOpenFailPopup);
        }

        private void OnOpenFailPopup(int score, Action<bool> callback)
        {
            _panelComps.Show();

            _callbacl = callback;

            _panelComps.Score.text = score.ts();
        }

        void OnClickReviveBtn()
        {
            _callbacl(true);

            _panelComps.Hide();
        }

        void OnClickCloseBtn()
        {
            _callbacl(false);

            _panelComps.Hide();
        }

    }
}
