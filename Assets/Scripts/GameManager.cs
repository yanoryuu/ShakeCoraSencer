using Unity.VisualScripting;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    //Manager
    private StateManager stateManager;
    [SerializeField] private PanelManager panelManager;
    
    //Model
    private IngameModel ingameModel;
    private ResultModel resultModel;
    
    //Presenter
    private IngamePresenter ingamePresenter;
    private ResultPresenter resultPresenter;
    private HomePresenter homePresenter;
    
    //View
    [SerializeField] private IngameView ingameView;
    [SerializeField] private ResultView resultView;
    [SerializeField] private HomeView homeView;
    
    //障害物
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    
    //アイテム
    [SerializeField] private ObstacleSpawner normalCoraSpawner;
    [SerializeField] private ObstacleSpawner goldCoraSpawner;
    
    //その他
    [SerializeField] private IMUInputManager inputManager;
    
    private void Start()
    {
        stateManager = new StateManager(panelManager);
        
        ingameModel = new IngameModel();
        resultModel = new ResultModel();

        homePresenter = new HomePresenter(homeView, stateManager);
        ingamePresenter = new IngamePresenter(ingameModel, ingameView, inputManager, resultModel, stateManager ,obstacleSpawner, normalCoraSpawner, goldCoraSpawner);
        resultPresenter = new ResultPresenter(resultView, resultModel, stateManager);
    }
}