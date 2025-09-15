using UnityEngine;

public class IngamePresenter
{   
    private IngameModel model;
    private IngameView view;
    private IMUInputManager inputManager;
    
    public IngamePresenter(IngameModel model, IngameView view, IMUInputManager inputManager)
    {
        this.model = model;
        this.view = view;
        this.inputManager = inputManager;
    }

    private void Bind()
    {
        
    }
}
