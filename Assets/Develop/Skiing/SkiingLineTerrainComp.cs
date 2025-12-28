using System;
using FGUFW;
using UnityEngine;
using UnityEngine.U2D;

namespace Skiing
{
    [ExecuteAlways]
    public class SkiingLineTerrainComp : MonoBehaviour
    {
        public LineRenderer line;

        public AnimationCurve Curve;//地形曲线
        public float PointCount=100;//点数
        public float StartX = -10;//初始偏移 从左侧屏幕外开始
        public float Length = 20;//地形宽
        public float OffsetX;//位移X
        public float OffsetY;//位移Y
        public float Velocity = 10;
        public float TurnWeight = 1000;//曲线宽
        public float TurnHeight = 1000;//曲线高
        public float Gravity = -10;//重力
        public float JumpVelocityWeight = 1;

        public Transform Target,CameraRoot;
        private Vector3[] Points,OffsetPoints;
        private float normalOffset;

        void OnValidate()
        {
            Init();
            ResetAllPoint();
        }

        // void Start()
        // {
        //     Init();
        //     ResetAllPoint();
        // }

        public void Step()
        {
            OffsetX += Velocity * Time.fixedDeltaTime;
            OffsetY = getPoint(0).y;
            
            ResetAllPoint();

            var point = VectorHelper.Get2DPathPointByX(OffsetPoints,0);
            var normal = VectorHelper.Get2DPathNormalByX(OffsetPoints,0);

            Target.position = point;
            Target.up = normal;
            CameraRoot.position = point + new Vector3(0,0,-10);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            
            for (int i = 1; i < Points.Length; i++)
            {
                var start = Points[i-1]+VectorHelper.Get2DNormal(Points,i-1)*normalOffset;
                var end = Points[i]+VectorHelper.Get2DNormal(Points,i)*normalOffset;

                Gizmos.DrawLine(start,end);
            }
        }


        public void Init()
        {
            Points = new Vector3[PointCount.ti()];
            OffsetPoints = new Vector3[Points.Length];

            line.positionCount = Points.Length;
            normalOffset = line.widthMultiplier/2;
        }

        public void ResetAllPoint()
        {
            var startPoint = getPoint(0);
            
            for (int i = (PointCount-1).ti(); i >= 0; i--)
            {
                Vector3 point = getPoint(i)-startPoint+new Vector3(StartX,0,0);

                Points[i] = point;
            }
            VectorHelper.Offset2DPath(Points,OffsetPoints,normalOffset);

            line.SetPositions(Points);
        }

        Vector3 getPoint(int index)
        {
            var deltaX = Length/PointCount;
            var offsetX = OffsetX + deltaX*index;
            var point = VectorHelper.GetCurvePoint(Curve,new Vector2(TurnWeight,TurnHeight),offsetX);
            return point;
        }


    }
}
