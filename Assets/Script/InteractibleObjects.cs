using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public interface IInteractible
{
    void PlayerInteract();
    void StartAnim();
    void StartVfxAndSfx();
}

public class InteractableObjects : MonoBehaviour, IInteractible
{
    public virtual void PlayerInteract()
    {
        StartAnim();
        StartVfxAndSfx();
    }

    public virtual void StartAnim()
    {
        
    }

    public virtual void StartVfxAndSfx()
    {
        
    }

}
