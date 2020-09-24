using NumVector2 = System.Numerics.Vector2;
using NumVector3 = System.Numerics.Vector3;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaVector3 = Microsoft.Xna.Framework.Vector3;

namespace UOLandscape.Extensions
{
    public static class VectorExtensions
    {
        public static NumVector3 ToNumericVector3(this XnaVector3 value)
        {
            return new NumVector3(value.X, value.Y, value.Z);
        }

        public static NumVector2 ToNumericVector2(this XnaVector2 value)
        {
            return new NumVector2(value.X, value.Y);
        }
    }
}
