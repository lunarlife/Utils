using Utils.Dots;

namespace Utils
{
    public static class MathUtils
    {
        public static bool Clamp(float value, float min, float max) => value.Equals(ClampOut(value, min, max));
        public static bool Clamp(Dot2 value, Dot2 min, Dot2 max) => ClampOut(value, min, max) == value;
        public static bool Clamp(Rect value, Rect min, Rect max) => ClampOut(value, min, max) == value;
        public static bool Clamp(int value, int min, int max) => value == ClampOut(value, min, max);
        
        public static float ClampOut(float value, float min, float max) => MathF.Max(min, MathF.Min(max, value));
        public static int ClampOut(int value, int min, int max) => (int)MathF.Max(min, MathF.Min(max, value));
        
        public static Dot2 ClampOut(Dot2 value, Dot2 minInclusive, Dot2 maxInclusive) => new(ClampOut(value.X, minInclusive.X, maxInclusive.X), ClampOut(value.Y, minInclusive.Y, maxInclusive.Y));

        public static Rect ClampOut(Rect value, Rect min, Rect max) =>
            new(ClampOut(value.Position, min.Position, max.Position),
                ClampOut(value.Width, min.Width, max.Width), ClampOut(value.Height, min.Height, max.Height));

        public static float Lerp(float start, float end, float time) => LerpUnclamped(start, end, ClampOut(time, 0f, 1f));
        public static float LerpUnclamped(float start, float end, float time) => start * (1f - time) + end * time;
     
        
        private static Dot2 GetPositionOfLerpCurve(IList<Dot2> curvePositions, float time)
        {
            var count = curvePositions.Count;
            while (count != 1)
            {
                for (var i = 0; i < count - 1; i++)
                    curvePositions[i] = Dot2.Lerp(curvePositions[i], curvePositions[i + 1], time);
                count--;
            }
            return curvePositions[0];
        }
        
        public static Dot2 LerpCurve(float time, params Dot2[] curvePositions)
        {
            if (curvePositions is null || curvePositions.Length < 2)
                throw new MathUtilsException("curve positions is null or length < 2");
            time = ClampOut(time, 0f, 1f);
            return GetPositionOfLerpCurve(curvePositions, time);
        }

        public static async Task<Dot2> LerpCurveAsync(float time, params Dot2[] curvePositions) => await Task.Run(() => LerpCurve(time, curvePositions));

        
        
        public static float VectorsProduct(Dot2 first, Dot2 second) => first.X * second.Y - second.X * first.Y;

        public static bool IsLinesIntersect(Dot2 first, Dot2 second, Dot2 third, Dot2 fourth)
        {
            var v1 = VectorsProduct(fourth - third, first - third);
            var v2 = VectorsProduct(fourth - third, second - third);
            var v3 = VectorsProduct(second - first, third - first);
            var v4 = VectorsProduct(second - first, fourth - first);
            return v1 * v2 < 0 && v3 * v4 < 0;
        }
      
    }

    public class MathUtilsException : Exception
    {
        public MathUtilsException(string msg) : base(msg)
        {
            
        }
    }
}