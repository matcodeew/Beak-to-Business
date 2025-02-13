using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip _pimentAudio;
    [SerializeField] private AudioClip _healAudio;
    [SerializeField] private AudioClip _sniperAudio;
    [SerializeField] private AudioClip _deathAudio;
    [SerializeField] private AudioClip _pillsAudio;
    [SerializeField] private AudioClip _rollerAudio;
    [SerializeField] private AudioClip _equipedWeaponAudio;

    public void PlayPimentAudio()
    {
        if(IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlaySniperAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlayHealAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlayDeathAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlayPillsAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlayRollerAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }

    public void PlayEquipedWeaponAudio()
    {
        if (IsOwner)
        {
            PlaySoundServerRpc();
        }
    }


    [ServerRpc]
    private void PlaySoundServerRpc()
    {
        PlaySoundClientRpc();
    }

    [ClientRpc]
    private void PlaySoundClientRpc()
    {
        audioSource.PlayOneShot(_pimentAudio);
    }
}
