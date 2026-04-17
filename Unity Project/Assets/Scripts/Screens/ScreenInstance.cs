using UnityEngine;

public class ScreenInstance : MonoBehaviour
{
    private static readonly int VisibleHash = Animator.StringToHash("Visible");
    
    public ScreensManager.ScreenType Type;
    [SerializeField] private Animator _animator;

    public void Appear()
    {
        _animator?.SetBool(VisibleHash, true);
    }

    public void Disappear()
    {
        _animator?.SetBool(VisibleHash, false);
    }

    public void OnFinishDisappearing()
    {
        Destroy(this.gameObject);
    }
}
