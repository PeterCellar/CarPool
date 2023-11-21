using CarPool.BL;


namespace CarPool.App.Messages;
public class VisibleMessage
{
    public bool Visible { get; }
    public string Name { get; }
    public VisibleMessage(bool visible, string name)
    {
        Visible = visible;
        Name = name;
    }
}
