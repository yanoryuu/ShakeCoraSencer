using UnityEngine;
public class GameManager : MonoBehaviour
{
    //Model
    public IngameModel model{ get; private set; }
    
    //Presenter
    public IngamePresenter presenter{ get; private set; }
    
    //View
    [SerializeField] private IngameView view;
    
    //その他
    [SerializeField] private IMUInputManager inputManager;
    
    private void Awake()
    {
        model = new IngameModel();
        presenter = new IngamePresenter(model, view, inputManager);
    }
}
