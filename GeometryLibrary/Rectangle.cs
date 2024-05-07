namespace GeometryLibrary;

public class Rectangle : Figure
{
    public Point UpLeftPoint { get; set; }
    public Point UpRightPoint { get; set; }
    public Point DownLeftPoint { get; set; }
    public Point DownRightPoint { get; set; }

    public Rectangle(double point1x, double point1y, double point2x, double point2y, double point3x, double point3y, double point4x, double point4y)
    {
        UpLeftPoint = new Point(point1x, point1y);
        UpRightPoint = new Point(point2x, point2y);
        DownLeftPoint = new Point(point3x, point3y);
        DownRightPoint = new Point(point4x, point4y);
    }

    public override double GetSquare()
    {
        return Math.Abs(UpLeftPoint.X - UpRightPoint.X) * Math.Abs(UpLeftPoint.Y - DownLeftPoint.Y);
    }

    public override Point GetCenterPoint()
    {
        return new Line(DownLeftPoint.X, DownLeftPoint.Y, UpRightPoint.X, UpRightPoint.Y).GetCenterPoint();
    }

    public override Rectangle GetBoxRectangle()
    {
        return this;
    }

    public override string ToString()
    {
        return $"Прямоугольник. Левая верхняя точка - {UpLeftPoint}. " +
               $"Правая верхняя точка - {UpRightPoint}. " +
               $"Левая нижняя точка- {DownLeftPoint}. " +
               $"Правая нижняя точка - {DownRightPoint}";
    }
}