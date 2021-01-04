
public class MyStateMachine
{
    #region Variables
    protected State currentState;
    #endregion

    #region Methods
    public MyStateMachine()
    {

    }

    public void SetInitialState(State initialState)
    {
        currentState = initialState;
        currentState.EnterState();
    }

    public void SetState(State nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    public void Update(float deltaTime)
    {
        currentState.Update(deltaTime);
    }

    public void FixedUpdate(float deltaTime)
    {
        currentState.FixedUpdate(deltaTime);
    }
    #endregion
}

