using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;

namespace Lobby
{
    [Serializable]
    public class LobbyPlayConfig
    {
        public string SelectGameId;
        public int Gold;

        public int GetItemCount(string itemId)
        {
            int value = default;

            if(itemId == GlobalConfig.Configs.ItemTypeSingle.GlodItem)
            {
                value = Gold;
            }

            return value;
        }
    }
}