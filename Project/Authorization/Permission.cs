namespace Project.Authorization;

public readonly struct Permission
{
    public readonly char data;
    public static readonly Permission NOT_REQUIRED = new();
    public Permission(Places place, Actions action) => data = (char)(((int)place << 3) | (int)action);
    public Permission(char c) => data = c;
    public Places Place => (Places)(data >> 3);
    public Actions Action => (Actions)(data & 0b111);
    public override string ToString() => $"{Place}.{Action}";
}