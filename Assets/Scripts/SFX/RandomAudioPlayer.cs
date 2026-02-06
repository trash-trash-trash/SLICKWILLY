using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Audio Clips")]
    [Tooltip("Array of audio clips to randomly choose from")]
    public AudioClip[] audioClips;
    
    [Header("Settings")]
    [Tooltip("Prevent the same clip from playing twice in a row")]
    public bool avoidRepeat = true;
    
    [Tooltip("Random pitch variation (0 = none, 0.2 = ±20% variation)")]
    [Range(0f, 0.5f)]
    public float pitchVariation = 0.1f;
    
    [Tooltip("Random volume variation (0 = none, 0.3 = ±30% variation)")]
    [Range(0f, 0.5f)]
    public float volumeVariation = 0.1f;
    
    [Tooltip("Base volume")]
    [Range(0f, 1f)]
    public float baseVolume = 1f;
    
    [SerializeField]
    private AudioSource audioSource;
    private int lastClipIndex = -1;
    
    void Awake()
    {
        // Get or add AudioSource component
        // audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configure AudioSource to not play on awake
        audioSource.playOnAwake = false;
    }
    
    /// <summary>
    /// Play a random audio clip from the array
    /// </summary>
    public void PlayRandomClip()
    {
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to RandomAudioPlayer!");
            return;
        }
        
        int clipIndex = GetRandomClipIndex();
        AudioClip selectedClip = audioClips[clipIndex];
        
        if (selectedClip == null)
        {
            Debug.LogWarning($"Audio clip at index {clipIndex} is null!");
            return;
        }
        
        // Apply random pitch variation
        audioSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        
        // Apply random volume variation
        float volume = baseVolume + Random.Range(-volumeVariation, volumeVariation);
        audioSource.volume = Mathf.Clamp01(volume);
        
        // Play the clip
        audioSource.PlayOneShot(selectedClip, audioSource.volume);
        
        // Remember this clip to avoid repeat
        lastClipIndex = clipIndex;
    }
    
    private int GetRandomClipIndex()
    {
        if (audioClips.Length == 1)
            return 0;
        
        if (avoidRepeat && audioClips.Length > 1)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, audioClips.Length);
            }
            while (newIndex == lastClipIndex);
            
            return newIndex;
        }
        
        return Random.Range(0, audioClips.Length);
    }
}