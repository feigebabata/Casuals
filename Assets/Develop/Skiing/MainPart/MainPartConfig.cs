using System;
using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace Skiing
{
    public partial class MainPart : IPartConfig
    {
        [Serializable]
        public class Config
        {
            
        }

        public Config SelfConfig = new Config();        
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
