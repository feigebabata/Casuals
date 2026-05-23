using FGUFW;
using FGUFW.Gameplay;
using UnityEngine;

namespace NoWhiteTile
{
    public class NoWhiteTilePlay : Play
    {
        [Inject]
        ILoadingUI _loadingUI;

        [Inject]
        IOrderedMessenger<string> _orderedMessenger;

        NoWhiteTileWorld _noWhiteTile;

        protected override void OnPartInitialed()
        {
            _loadingUI.Hide();
            
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;

            addListener();
            base.OnPartInitialed();
        }

        protected override void OnAllPartInitialed()
        {
            _noWhiteTile = FindFirstObjectByType<NoWhiteTileWorld>();
            _noWhiteTile.GetComponent<Canvas>().worldCamera = UICamera.I.Camera;

            _noWhiteTile.OnGameInit();
        }

        protected override void OnPartDestroy()
        {
            removeListener();
        }

        

        private void addListener()
        {
            _orderedMessenger.Add(Lobby.LobbyPlayMsgId.OnClickQuit,OnClickQuit,10);
        }

        private void removeListener()
        {
            _orderedMessenger.Remove(Lobby.LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        private void OnClickQuit()
        {
            _loadingUI.Show();
            
            _orderedMessenger.Abort(Lobby.LobbyPlayMsgId.OnClickQuit);
            
            PartDIContainer.DestroyPlay<NoWhiteTilePlay>();
            
            _orderedMessenger.Broadcast(Lobby.LobbyPlayMsgId.OpenLobby);
        }

        void Update()
        {
            if(!this.initialed)return;

            _noWhiteTile.LogicUpdate();
        }

    }
}
