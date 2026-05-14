using UnityEngine;

public class FootStepPlayer : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioClip[] footstepSounds;
    private int index = 0; // Index for footstep sounds
    [SerializeField] private float footStepInterval = 0.5f; // Time interval between footsteps
    private float footstepTimer = 0f; // Timer to track time since last footstep


    public void PlayFootStep(float footStepInterval, float pitch, float volume)
    {
        if(!footstepAudioSource)
        {
            Debug.LogWarning("Foot step audio source not initialized");
            return;
        }
        this.footStepInterval = footStepInterval;
        footstepAudioSource.volume = volume;
        footstepAudioSource.pitch = pitch;

        footstepTimer += Time.deltaTime;
        if (footstepTimer >= this.footStepInterval)
        {
            if (footstepSounds.Length > 0)
            {
                index = Random.Range(0, footstepSounds.Length);
            }
            //footstepAudioSource.pitch = Random.Range(0.6f, 7.1f);
            footstepAudioSource.PlayOneShot(footstepSounds[index]);
            footstepTimer = 0f;
        }
    }


}
