namespace MafiaManager;

public class Player
{
    public string name { get; set; }
    public string role { get; set; }
    public bool alive { get; set; }

    public Player(string _name, string _role)
    {
        name = _name;
        role = _role;
        alive = true;
    }
}