namespace Cards
{
    public enum ZoneApplication
    {
        Everything,
        Nothing,
        OnlyBase,
        OnlyTower
    }
    
    public interface IHaveZoneApplication
    {
        ZoneApplication ZoneApplication { get; set; }
    }
}