using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;

namespace Lobby
{
    public partial class LobbyPlay : Play<LobbyPlay>, IPartConfig
    {
        [Serializable]
        public class Config
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

        public Config SelfConfig = new();

        public object PartConfig 
        { 
            get
            {
                return SelfConfig;
            }
            set
            {
                SelfConfig = value as Config;
            }
        }

        public Type GetPartConfigType()
        {
            return typeof(Config);
        }
    }
}