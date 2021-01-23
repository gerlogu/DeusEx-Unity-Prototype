
public class CameraShutDownState : State
{
    #region Variables
    private readonly EnemyCameraController _enemyController;
    #endregion

    #region Methods
    public CameraShutDownState(EnemyCameraController enemyController, MyStateMachine stateMachine) : base(stateMachine)
    {
        _enemyController = enemyController;
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (_enemyController.cameraArea.activated)
            stateMachine.SetState(new CameraLookOutState(_enemyController, stateMachine));
    }
    #endregion
}
