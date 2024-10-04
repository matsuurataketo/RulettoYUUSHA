using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        [SerializeField] private string name;
        [SerializeField] private AudioClip clip;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;
        [SerializeField] private bool loop = false;

        public string Name => name;
        public AudioClip Clip => clip;
        public float Volume => volume;
        public bool Loop => loop;
    }

    [SerializeField] private List<Sound> sounds;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int poolSize = 10;

    private List<GameObject> audioSourcePool;
    private Dictionary<string, AudioSource> activeAudioSources = new Dictionary<string, AudioSource>();  // 再生中のサウンドを管理

    void Start()
    {
        audioSourcePool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab, this.transform);
            obj.SetActive(false);
            audioSourcePool.Add(obj);
        }
    }

    private GameObject GetPooledAudioSource()
    {
        foreach (GameObject obj in audioSourcePool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = Instantiate(audioSourcePrefab, this.transform);
        newObj.SetActive(false);
        audioSourcePool.Add(newObj);
        return newObj;
    }

    // サウンド再生
    public void PlaySound(string soundName, Transform parent = null)
    {
        Sound sound = sounds.Find(s => s.Name == soundName);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        GameObject audioObj = GetPooledAudioSource();
        audioObj.transform.SetParent(parent);
        audioObj.transform.localPosition = Vector3.zero;
        audioObj.SetActive(true);

        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.loop = sound.Loop;
        audioSource.Play();

        // 再生中のAudioSourceを保存（ループするサウンドのみ）
        if (sound.Loop)
        {
            activeAudioSources[soundName] = audioSource;
        }

        if (!sound.Loop)
        {
            StartCoroutine(DisableAfterPlay(audioSource));
        }
    }

    // サウンド停止
    public void StopSound(string soundName)
    {
        if (activeAudioSources.ContainsKey(soundName))
        {
            AudioSource audioSource = activeAudioSources[soundName];
            audioSource.Stop();
            audioSource.gameObject.SetActive(false); // プールに戻す
            audioSource.transform.SetParent(this.transform);

            activeAudioSources.Remove(soundName); // リストから削除
        }
        else
        {
            Debug.LogWarning("No active sound with name: " + soundName);
        }
    }

    private IEnumerator DisableAfterPlay(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.gameObject.SetActive(false);
        audioSource.transform.SetParent(this.transform);
    }
}