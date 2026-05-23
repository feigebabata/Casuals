
using System;
using System.Collections.Generic;
using FGUFW;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NoWhiteTile
{
    public class NoWhiteTileWorld : GameWorldBase
    {
        public RectTransform TileRoot;
        public Color ClickColor = Color.black;
        public Color BaseColor = Color.white;
        public Vector2Int TileCount=new Vector2Int(4,4);
        public float MoveSpeed = 500;
        public float MoveSpeedInc = 10;
        public TMP_Text ScoreText;

        private float _currentMoveSpeed;
        private RectTransform[] TileRects;
        private PointerClicker[] TileClickers;
        private Image[] TileImgs;
        private Vector3Int[] TileIndexs;

        private Vector2 _tileSize;
        private float _offsetY;
        private float _recycleEdge;


        public override void OnGameInit()
        {
            base.OnGameInit();
            //格子中心点在左下角 格子起点是屏幕左下角

            _currentMoveSpeed = MoveSpeed;

            var count = TileCount.x*TileCount.y+TileCount.x;

            TileRects = new RectTransform[count];
            TileClickers = new PointerClicker[count];
            TileImgs = new Image[count];
            TileIndexs = new Vector3Int[count];

            _tileSize = new Vector2(TileRoot.rect.width/TileCount.x,TileRoot.rect.height/TileCount.y);

            _recycleEdge = -_tileSize.y;

            TileRoot.For<PointerClicker>(count,(i,comp)=>
            {
                Vector3Int tileIndex = new Vector3Int(i%TileCount.x,i/TileCount.x);

                TileRects[i] = comp.transform.AsRT();
                TileClickers[i] = comp;
                TileImgs[i] = comp.GetComponent<Image>();

                TileRects[i].sizeDelta = _tileSize;
                TileImgs[i].color = BaseColor;
                TileIndexs[i] = tileIndex;

                comp.Data.Set(i);
                comp.name = tileIndex.ts();
                comp.OnClick = OnClickTile;
                
            });
            
            resetTilesPos();
            resetScoreText();
        }

        private void OnClickTile(PointerClicker comp)
        {
            var i = comp.Data.Get<int>();
            var tileIndex = TileIndexs[i];

            if(tileIndex.z==1)
            {
                tileIndex.z = 0;
                TileIndexs[i] = tileIndex;
                TileImgs[i].color = BaseColor;

                Score++;
            }
            else
            {
                onGameFail(Score.ti());
            }
            resetScoreText();
        }

        private void resetTilesPos()
        {
            for (int i = 0; i < TileIndexs.Length; i++)
            {
                var tileIndex = TileIndexs[i];
                TileRects[i].anchoredPosition = new Vector2(tileIndex.x*_tileSize.x,tileIndex.y*_tileSize.y-_offsetY);
            }
        }

        private void resetScoreText()
        {
            ScoreText.text = Score.ts();
        }

        public override void OnGameStart()
        {
        }

        protected override void OnGameOver()
        {
            clearTiles();
        }

        protected override void OnGameRestart()
        {
            base.OnGameRestart();

            clearTiles();
            _currentMoveSpeed = MoveSpeed;
            resetScoreText();

        }

        protected override void OnGameRevive()
        {
            clearTiles();

            base.OnGameRevive();
            resetScoreText();
        }

        private void clearTiles()
        {
            for (int i = 0; i < TileIndexs.Length; i++)
            {
                var tileIndex = TileIndexs[i];
                tileIndex.z = 0;
                TileIndexs[i] = tileIndex;
                TileImgs[i].color = BaseColor;
            }
        }

        protected override void OnLogicUpdate()
        {
            _currentMoveSpeed += MoveSpeedInc * Time.deltaTime;
            _offsetY += _currentMoveSpeed * Time.deltaTime;
            resetTilesPos();
            
            int clickIndex = -1;

            for (int i = 0; i < TileIndexs.Length; i++)
            {
                var tileIndex = TileIndexs[i];
                var rectT = TileRects[i];

                if(rectT.anchoredPosition.y<_recycleEdge)
                {
                    if(clickIndex==-1)
                    {
                        clickIndex = UnityEngine.Random.Range(0,TileCount.x);
                    }
                    
                    if(tileIndex.z==1)
                    {
                        onGameFail(Score.ti());
                        break;
                    }


                    tileIndex.y += TileCount.y+1;
                    tileIndex.z = tileIndex.x==clickIndex?1:0;
                    TileIndexs[i] = tileIndex;

                    TileImgs[i].color = tileIndex.x==clickIndex?ClickColor:BaseColor;
                    rectT.name = tileIndex.ts();
                }
            }

        }
    }
}