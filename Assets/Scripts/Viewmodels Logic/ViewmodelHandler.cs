using UnityEngine;
using UnityEngine.Events;

public enum ViewmodelStates
    {
        Pickup,
        Idle,
        Attack,
    }

public class ViewmodelHandler : MonoBehaviour
{
    [SerializeField] ViewmodelStates CurrentState;
    public Animation Viewmodel;
    public AnimationClip Pickup, Attack;

    public UnityAction<ViewmodelStates> OnStateChange;

    void Start()
    {
        ChangeState(ViewmodelStates.Pickup);
    }

    public void ChangeState(ViewmodelStates state)
    {
        switch (state)
        {
            case ViewmodelStates.Pickup:
                CurrentState = state;
                if (Pickup)
                    Viewmodel.Play(Pickup.name);
                break;

            case ViewmodelStates.Attack:
                if (CurrentState != ViewmodelStates.Attack)
                {
                    CurrentState = state;
                    if (Attack)
                    {
                        Viewmodel.Stop();
                        Viewmodel.Play(Attack.name, PlayMode.StopAll);
                    }
                }
                break;

            case ViewmodelStates.Idle:
                CurrentState = state;
                break;

            default:
                CurrentState = ViewmodelStates.Idle;
                break;
        }

        OnStateChange?.Invoke(CurrentState);
    }
}
