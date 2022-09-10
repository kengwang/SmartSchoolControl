namespace SchoolComputerControl.Client.Interfaces;

public interface ISetting<TSetting>
{
    public TSetting? Setting { get; }
}