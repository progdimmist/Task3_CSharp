namespace GeometryLibrary;

public class Point : Figure
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override double GetSquare()
    {
        return 0;
    }

    public override Point GetCenterPoint()
    {
        return this;
    }

    public override Rectangle GetBoxRectangle()
    {
        return new Rectangle(X, Y, X, Y, X, Y, X, Y);
    }

    public override string ToString()
    {
        return $"Точка ({X};{Y})";
    }
}