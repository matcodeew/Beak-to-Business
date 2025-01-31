using UnityEngine;

public class ObjectTest : InteractableObjects
{
    public override void PlayerInteract()
    {
        base.PlayerInteract();

        print("COUCOU");
    }
}
