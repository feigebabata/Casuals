using System.Collections;
using System.Collections.Generic;
using FGUFW.Gameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;

namespace Skiing
{
    public class SkiingPlay : Play
    {
        private SkiingLineTerrainComp _terrainComp;

        [Inject]
        private ILoadingUI _loadingUI;


        protected override void OnPartInitialed()
        {
            _terrainComp = findGO<SkiingLineTerrainComp>("Terrain");

            _loadingUI.Hide();
            
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;

            base.OnPartInitialed();
        }

        protected override void OnPartDestroy()
        {
            
        }

        protected override void OnAllPartInitialed()
        {
            _terrainComp.Init();
        }


    }
}

