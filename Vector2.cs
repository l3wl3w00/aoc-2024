using System.Numerics;

namespace Aoc._2024;

public readonly record struct Vector2<T>(T X, T Y) 
    where T : struct, INumber<T>
{
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
}