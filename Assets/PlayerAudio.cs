using UnityEngine;
using Unity.Netcode;
using NUnit.Framework;

public class PlayerAudio : NetworkBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pimentAudio;
    //public AudioClip healAudio;
    [SerializeField] private AudioClip sniperAudio;
    //public AudioClip clip4;

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

    [ServerRpc]
    private void PlaySoundServerRpc()
    {
        PlaySoundClientRpc();
    }

    [ClientRpc]
    private void PlaySoundClientRpc()
    {
        audioSource.PlayOneShot(pimentAudio);
    }
}
