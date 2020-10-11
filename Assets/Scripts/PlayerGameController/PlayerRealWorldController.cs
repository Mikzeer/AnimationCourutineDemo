using PositionerDemo;

public class PlayerRealWorldController
{
    public int playerID { get; set; }
    protected PLAYERWCONTROLLERTYPE controllerType;
    protected PLAYERWCONTROLLERSCHEME controllerScheme;

    public PlayerRealWorldController(int playerID)
    {
        this.playerID = playerID;
    }

    public virtual Tile GetTile()
    {
        return null;
    }

    public virtual bool Select()
    {
        return true;
    }

    public virtual void SpecialSelection()
    {
    }
}