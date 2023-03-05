using UnityEngine;
namespace JDR.ExtensionMethods
{
    public static class Extensions
    {
        public static Vector3 ToVector3_XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }

        private static readonly Vector3[] DIRECTIONS = new Vector3[]
        { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

        public static CompassDirection ToClosestCompassDirection(this Vector3 v)
        {
            int bestIndex = -1;
            float closestValue = -1f;
            for (int i = 0; i < DIRECTIONS.Length; i++)
            {
                float dot = Vector3.Dot(DIRECTIONS[i], v);
                if (dot > closestValue)
                {
                    closestValue = dot;
                    bestIndex = i;
                }
            }

            return (CompassDirection)bestIndex;
        }

        public static Vector3 ToVector3(this CompassDirection cd)
        {
            return DIRECTIONS[(int)cd];
        }

        public static CompassDirection GetOpposite(this CompassDirection cd)
        {
            return (CompassDirection)(((int)cd + 2) % 4);
        }
    }
}
