using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;

namespace Lobby
{
    public partial class LobbyPlay : Play<LobbyPlay>
    {
        public override IEnumerator OnCreating(Part play,Part parent)
        {
            AddPart<LobbyHomePart>();
            AddPart<MenuPart>();
            AddPart<SettingPart>();
            
            yield return base.OnCreating(this,this);

            Messenger.Add(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
            OnCreatingComplate();
        }

        private void OnCreatingComplate()
        {
            GetPart<LobbyHomePart>().Show();
            GetPart<MenuPart>().Show();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            Messenger.Remove(LobbyPlayMsgId.OnClickQuit,OnClickQuit);
        }

        private void OnClickQuit()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #endif
        }
    }
}

