namespace CouchPlayTest.Utilities.UI;

public class UiInteractable
{
    //Todo: Comment explanations.
    //File subject to a lot of change however.
    
    double _interactionDelay = 0.0f;
    bool _interactedRecently = false;
    bool _buttonDownLastFrame = false;
    bool _blockNextFrame = false;

    public readonly record struct UiResult(
        bool Triggered,
        float Value
    );
    
    public enum UiAxis
    {
        X,
        Y
    }
    
    public UiResult UpdateInputButton(Player player, double deltaTime)
    {
        if (_blockNextFrame)
        {
            _blockNextFrame = false;
            return new UiResult(false, 0);
        }
        
        bool triggered = player.GetSpecialInput();

        if (_buttonDownLastFrame) _interactionDelay += deltaTime; else _interactionDelay = 0; 
        
        _buttonDownLastFrame = triggered;
        return new UiResult(triggered, (float)_interactionDelay);
    }
    
    public UiResult UpdateInputToggle(Player player, double deltaTime)
    {
        if (_blockNextFrame)
        {
            _blockNextFrame = false;
            return new UiResult(false, 0);
        }

        
        bool value = false;
        
        bool triggered = player.GetSpecialInput();
        bool justPressed = triggered && !_buttonDownLastFrame;
        _buttonDownLastFrame = triggered;
        
        switch (player.Voted) {
            case -1 when justPressed && !_interactedRecently:
                _interactionDelay = 0;
                _interactedRecently = true;
                value = true;
                break;
            case >= 0 when justPressed && !_interactedRecently:
                _interactionDelay = 0;
                _interactedRecently = true;
                value = false;
                break;
        }
        
        switch (_interactedRecently) {
            case true when _interactionDelay > 0.2f:
                _interactionDelay = 0;
                _interactedRecently = false;
                break;
            case true:
                _interactionDelay += deltaTime;
                break;
        }
        
        return new UiResult(justPressed, value ? 1 : 0);
    }
    
    public UiResult UpdateInputSelection(Player player, double deltaTime, UiAxis axis, (int min, int max) range, int index, bool enableLooping, bool invertDir = false)
    {
        if (_blockNextFrame)
        {
            _blockNextFrame = false;
            return new UiResult(false, 0);
        }
        
        bool triggered = false;

        float axisValue = axis switch
        {
            UiAxis.X => player.GetInput().X,
            UiAxis.Y => player.GetInput().Y,
            _ => 0
        };

        int raw = (int)Math.Clamp((invertDir ? -axisValue : axisValue) * 4f, -1f, 1f);
        
        switch (_interactedRecently) {
            case false when raw != 0:
                index = enableLooping ? Loop(range, index + raw) : Math.Clamp(index + raw, range.min, range.max);
                _interactedRecently = true;
                _interactionDelay += deltaTime;
                triggered = true;
                return new UiResult(triggered, index);
            case true:
                _interactionDelay += deltaTime;
                triggered = false;
                break;
        }

        switch (_interactionDelay) {
            case > 0.1f when raw == 0:
                _interactedRecently = false;
                _interactionDelay = 0;
                triggered = false;
                break;
            case > 0.2f: //when x != 0:
                index = enableLooping ? Loop(range, index + raw) : Math.Clamp(index + raw, range.min, range.max);
                _interactedRecently = true;
                _interactionDelay = 0;
                triggered = true;
                break;
        }
        
        return new UiResult(triggered, index);
    }
    
    public void Reset()
    {
        _interactionDelay = 0.0f;
        _interactedRecently = false;
        _buttonDownLastFrame = true;
        _blockNextFrame = true;
    }

    public static int Loop((int min, int max) loop, int index)
    {
        if (index < loop.min) index = loop.max;
        if (index > loop.max) index = loop.min;
        return index;
    }
}