using R3;

public class ResultPresenter : IPresenter
{
    private ResultView resultView;
    private ResultModel resultModel;

    private StateManager stateManager;

    private CompositeDisposable disposable;
    
    public ResultPresenter(ResultView resultView,ResultModel resultModel,StateManager stateManager)
    {
        this.resultView = resultView;
        this.resultModel = resultModel;
        this.stateManager = stateManager;

        disposable = new CompositeDisposable();
        
        stateManager.RegisterOnEnter(GameState.result,Enter);
        
        Bind();
    }
    
    public void Enter()
    {
        resultView.Initialize();
        SoundManager.Instance.PlayBGM("Result");
        ShowScore();
    }

    private void Bind()
    {
        resultView.onHome.Subscribe(_ =>
        {
            stateManager.ChangeState(GameState.home);
            SoundManager.Instance.PlaySE("Button");
        }).AddTo(disposable);

        resultView.onRetry.Subscribe(_ =>
        {
            stateManager.ChangeState(GameState.ingame);
            SoundManager.Instance.PlaySE("Button");
        }).AddTo(disposable);
    }

    private void ShowScore()
    {
        resultView.SetScore(resultModel.score);
    }
}
