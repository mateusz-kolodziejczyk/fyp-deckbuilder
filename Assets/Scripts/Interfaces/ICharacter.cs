namespace Interfaces
{
    public interface ICharacter
    {
        int HitPoints
        {
            get;
            set;
        }

        (int x, int y) Position
        {
            get;
            set;
        }
    }
}