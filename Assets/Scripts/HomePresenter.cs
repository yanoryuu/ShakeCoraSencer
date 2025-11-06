using R3;

public class HomePresenter : IPresenter
{
    private HomeView homeView;
    private StateManager stateManager;

    private CompositeDisposable disposables;
    public HomePresenter(HomeView homeView, StateManager stateManager)
    {
        this.homeView = homeView;
        this.stateManager = stateManager;
        disposables = new CompositeDisposable();
        stateManager.RegisterOnEnter(GameState.home,Enter);
        Bind();
    }

    private void Bind()
    {
        homeView.onStart.Subscribe(_ =>
        {
            stateManager.ChangeState(GameState.ingame);
        }).AddTo(disposables);
    }

    public void Enter()
    {
        homeView.Initialize();
    }
}
