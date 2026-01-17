using UnityEngine;
using UnityEngine.Events;

public enum ViewmodelStates
{
        Default,
        CanAttack,
        InProgress, // While Attacking
}

public class ViewmodelHandler : MonoBehaviour
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
        // Пока что в этом нет смысла
        // switch (state)
        // {
        //     case ViewmodelStates.CanAttack:
        //         CurrentState = state;
        //         break;

        //     default:
        //         CurrentState = ViewmodelStates.Default;
        //         break;
        // }

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
