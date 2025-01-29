using UnityEngine;


public interface IInteractible
{
    void PlayerInteract(Player player);
    void StartAnim();
    void StartVfxAndSfx();
}

public class InteractableObjects : MonoBehaviour, IInteractible
{
    public virtual void PlayerInteract(Player player)
    {
        print($"{player.name} interact with {name}");
        StartAnim();
        StartVfxAndSfx();
    }
    public virtual void StartAnim() { }
    public virtual void StartVfxAndSfx() { }
}
