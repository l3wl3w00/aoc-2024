using System.Numerics;

namespace Aoc._2024.Common;

public readonly record struct Vector2<T>(T X, T Y) 
    where T : struct, INumber<T>
{
    public static readonly Vector2<T> PositiveX = new(1, 0);
    public static readonly Vector2<T> NegativeX = new(-1, 0);
    public static readonly Vector2<T> PositiveY = new(0, 1);
    public static readonly Vector2<T> NegativeY = new(0, -1);
    public static readonly IEnumerable<Vector2<T>> UpDownLeftRightDirs = [PositiveX, NegativeX, PositiveY, NegativeY];
    public Vector2(int x, int y) : this(T.CreateChecked(x), T.CreateChecked(y))
    {
    }

    public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2<T> operator -(Vector2<T> a, Vector2<T> b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2<T> operator *(T scalar, Vector2<T> a) => new(a.X * scalar, a.Y * scalar);
    public static Vector2<T> operator *(Vector2<T> a, T scalar) => scalar * a;
    public static Vector2<T> operator *(int scalar, Vector2<T> a) 
        => new(T.CreateChecked(scalar) * a.X, T.CreateChecked(scalar) * a.Y);
    public Vector2<T> RotateRight() => new(-Y, X);

    public double DistanceTo(Vector2<T> freqPos2) => (this + T.CreateChecked(-1) * freqPos2).Length;

    private double Length => Math.Sqrt(double.CreateChecked(X * X + Y * Y));
    public IEnumerable<Vector2<T>> NonDiagonalNeighbours
    {
        get
        {
            var self = this;
            return UpDownLeftRightDirs.Select(d => self + d);
        }
    }
}