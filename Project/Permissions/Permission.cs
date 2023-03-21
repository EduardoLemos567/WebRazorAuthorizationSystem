namespace Project.Permissions;

public readonly struct Permission
{
    public readonly char data;
    public Permission(Places place, Actions action) => data = ToChar(place, action);
    public Permission(char c) => data = c;
    private static char ToChar(Places place, Actions action) => (char)(((int)place << 3) | (int)action);
    private static (Places, Actions) FromChar(char c) => ((Places)(c >> 3), (Actions)(c & 0b111));
    public override string ToString()
    {
        var (place, action) = FromChar(data);
        return $"{place}.{action}";
    }
}