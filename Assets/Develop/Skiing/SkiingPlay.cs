using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;
using FGUFW;
using static FGUsing;
using System;

namespace Skiing
{
    public class SkiingPlay : Play<SkiingPlay>
    {
        public SkiingLineTerrainComp TerrainComp;

        public override IEnumerator OnCreating(Part play,Part parent)
        {
            GlobalLoading.I.Show();
            yield return loadSceneAsync("Assets/Develop/Skiing/Skiing.unity");
            TerrainComp = findGO<SkiingLineTerrainComp>("Terrain");
         
            AddPart<MainPart>();   
            yield return base.OnCreating(this,this);

            GlobalLoading.I.Hide();
            
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;

            OnCreateEnd();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            
        }

        private void OnCreateEnd()
        {
            GetPart<MainPart>().Show();

            TerrainComp.Init();
        }

    }
}

