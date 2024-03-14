public interface IReversibleAction
{
    void Perform();
    void Undo();
}