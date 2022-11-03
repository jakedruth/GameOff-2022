using UnityEngine;
namespace JDR.ExtensionMethods
{
    public static class Extensions
    {
        public static Vector3 Vector2ToVector3_XZ(this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }
    }
}
