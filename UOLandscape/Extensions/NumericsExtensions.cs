using Microsoft.Xna.Framework;

namespace UOLandscape.Extensions
{
    public static class NumericsExtensions
    {
        public static Vector3 ToXnaVector3(this System.Numerics.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static Vector2 ToXnaVector2(this System.Numerics.Vector2 value)
        {
            return new Vector2(value.X, value.Y);
        }
    }
}