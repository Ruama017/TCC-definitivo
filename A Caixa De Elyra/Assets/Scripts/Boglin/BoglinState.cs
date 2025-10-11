public abstract class BoglinState
{
    protected BoglinController boglin;

    public BoglinState(BoglinController boglin)
    {
        this.boglin = boglin;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public abstract void LogicUpdate();
}