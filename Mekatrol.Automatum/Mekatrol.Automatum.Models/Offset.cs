﻿namespace Mekatrol.Automatum.Models;

public class Offset
{
    public double X { get; set; }
    public double Y { get; set; }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

}
