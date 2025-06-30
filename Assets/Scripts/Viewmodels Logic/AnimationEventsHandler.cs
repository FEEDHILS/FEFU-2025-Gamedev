using UnityEngine;

public class AnimationEventsHandler : MonoBehaviour
{
    public ViewmodelHandler Viewmodel;
    public AttackHitbox hitbox;
    public void ChangeState(ViewmodelStates state)
    {
        Viewmodel.ChangeState(state);
    }

    public void CreateHitbox()
    {
        hitbox.CreateHitbox();
    }
}
