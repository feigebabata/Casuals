using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FGUFW;
using System;

namespace ExcelConfig
{
    [Serializable]
    public class GameConfigEC
    {


        public Table<string,SubGame> SubGames;
        [Serializable]
        public class SubGame
        {
            public string Id;
            public string Name;
            public string Description;
            public string Icon;
            public string Type;

        }


        public Table<string,Item> Items;
        [Serializable]
        public class Item
        {
            public string Id;
            public string Name;
            public string Description;
            public string Icon;

        }


        public ItemType ItemTypeSingle;
        [Serializable]
        public class ItemType
        {
            public string GlodItem;

        }



    }
}
