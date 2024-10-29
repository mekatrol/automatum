namespace Mekatrol.Automatum.Entities;

public abstract record Point
{
    public string Unit { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public Guid Id { get; set; } = Guid.Empty;

    protected abstract string? InternalValue { get; set; }
}

public record BytePoint : Point
{
    protected override string? InternalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public record WordPoint : Point
{
    protected override string? InternalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public record IntPoint : Point
{
    protected override string? InternalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public record FloatPoint : Point
{
    protected override string? InternalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

public record StringPoint : Point
{
    protected override string? InternalValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
