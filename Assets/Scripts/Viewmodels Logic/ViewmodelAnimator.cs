using UnityEngine;
using UnityEngine.Events;

public enum ViewmodelStates
{
        Default, // Undefined State
        CanAttack,
        InProgress, // While Attacking
}

public class ViewmodelAnimator : MonoBehaviour
{
    [SerializeField] ViewmodelStates CurrentState;
    public Animator AnimationController;

    public UnityAction<ViewmodelStates> OnStateChange;

    void Start()
    {
        ChangeState(ViewmodelStates.Default);
    }

    public void ChangeState(ViewmodelStates state)
    {
        CurrentState = state;
        OnStateChange?.Invoke(CurrentState);
    }

    public void PrimaryAction()
    {
        if (CurrentState != ViewmodelStates.InProgress)
        {
            AnimationController.SetTrigger("Attack");
            ChangeState(ViewmodelStates.InProgress);
        }
    }
}
