public abstract class State
{
    #region Variables
    protected MyStateMachine stateMachine;
    #endregion

    #region Methods
    public State(MyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {

    }

    public virtual void Update(float deltaTime)
    {

    }

    public virtual void FixedUpdate(float deltaTime)
    {

    }

    public virtual void ExitState()
    {

    }
    #endregion
}
