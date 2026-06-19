using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }
    public Audio[] listOfAudio;
    public ObjectPooling audioPool;

    [Header("Audio Effects")]
    public bool globalMuffle;
    public bool globalEcho;
    [HideInInspector] public AudioSource music;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (music != null)
        {
            music.volume = PlayerPrefs.GetFloat("Music", 0.5f);
        }
    }

    public AudioSource Play(string nameOfAudio)
    {
        Audio chosenAudio = null;
        foreach (Audio audio in listOfAudio)
        {
            if (audio.name == nameOfAudio)
            {
                chosenAudio = audio;
                break;
            }
        }
        if (chosenAudio != null)
        {
            AudioSource source = audioPool.GetObject().GetComponent<AudioSource>();
            if (globalMuffle && !chosenAudio.immuneToMuffle)
            {
                source.GetComponent<AudioLowPassFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioLowPassFilter>().enabled = false;
            }
            if (globalEcho && !chosenAudio.immuneToEcho)
            {
                source.GetComponent<AudioEchoFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioEchoFilter>().enabled = false;
            }
            source.transform.SetParent(transform);
            source.transform.position = Camera.main.transform.position;
            source.clip = chosenAudio.clip[Random.Range(0, chosenAudio.clip.Length)];
            source.loop = false;
            if (chosenAudio.randomPitched)
            {
                source.pitch = Random.Range(0.9f, 1.1f);
            }
            if (chosenAudio.isMusic)
            {
                source.volume = PlayerPrefs.GetFloat("Music", 0.5f);
            } else
            {
                source.volume = PlayerPrefs.GetFloat("SFX", 1);
            }
            source.Play();
            StartCoroutine(ReturnSound(source, source.clip));
            return source;
        }
        else
        {
            print(nameOfAudio + " doesn't exist in the library, might wanna add it first.");
            return null;
        }
    }

    public GameObject Play(string nameOfAudio, bool loop = false)
    {
        Audio chosenAudio = null;
        foreach (Audio audio in listOfAudio)
        {
            if (audio.name == nameOfAudio)
            {
                chosenAudio = audio;
                break;
            }
        }
        if (chosenAudio != null)
        {
            AudioSource source = audioPool.GetObject().GetComponent<AudioSource>();
            if (globalMuffle && !chosenAudio.immuneToMuffle)
            {
                source.GetComponent<AudioLowPassFilter>().enabled = true;
            } else
            {
                source.GetComponent<AudioLowPassFilter>().enabled = false;
            }
            if (globalEcho && !chosenAudio.immuneToEcho)
            {
                source.GetComponent<AudioEchoFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioEchoFilter>().enabled = false;
            }
            source.transform.SetParent(transform);
            source.transform.position = Camera.main.transform.position;
            source.clip = chosenAudio.clip[Random.Range(0, chosenAudio.clip.Length)];
            source.loop = loop;
            if (chosenAudio.randomPitched)
            {
                source.pitch = Random.Range(0.9f, 1.1f);
            }
            if (chosenAudio.isMusic)
            {
                source.volume = PlayerPrefs.GetFloat("Music", 0.5f);
            }
            else
            {
                source.volume = PlayerPrefs.GetFloat("SFX", 1);
            }
            source.Play();
            if (!loop)
            {
                StartCoroutine(ReturnSound(source, source.clip));
            }
            return source.gameObject;
        }
        else
        {
            print(nameOfAudio + " doesn't exist in the library, might wanna add it first.");
            return null;
        }
    }

    public GameObject Play(string nameOfAudio, Vector3 position, bool loop = false)
    {
        Audio chosenAudio = null;
        foreach (Audio audio in listOfAudio)
        {
            if (audio.name == nameOfAudio)
            {
                chosenAudio = audio;
                break;
            }
        }
        if (chosenAudio != null)
        {
            AudioSource source = audioPool.GetObject().GetComponent<AudioSource>();
            if (globalMuffle && !chosenAudio.immuneToMuffle)
            {
                source.GetComponent<AudioLowPassFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioLowPassFilter>().enabled = false;
            }
            if (globalEcho && !chosenAudio.immuneToEcho)
            {
                source.GetComponent<AudioEchoFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioEchoFilter>().enabled = false;
            }
            source.transform.SetParent(transform);
            source.transform.position = position;
            source.clip = chosenAudio.clip[Random.Range(0, chosenAudio.clip.Length)];
            source.loop = loop;
            if (chosenAudio.randomPitched)
            {
                source.pitch = Random.Range(0.9f, 1.1f);
            }
            if (chosenAudio.isMusic)
            {
                source.volume = PlayerPrefs.GetFloat("Music", 0.5f);
            }
            else
            {
                source.volume = PlayerPrefs.GetFloat("SFX", 1);
            }
            source.Play();
            if (!loop)
            {
                StartCoroutine(ReturnSound(source, source.clip));
            }
            return source.gameObject;
        }
        else
        {
            print(nameOfAudio + " doesn't exist in the library, might wanna add it first.");
            return null;
        }
    }

    public AudioSource PlayMusic(string nameOfAudio)
    {
        Audio chosenAudio = null;
        foreach (Audio audio in listOfAudio)
        {
            if (audio.name == nameOfAudio)
            {
                chosenAudio = audio;
                break;
            }
        }
        if (chosenAudio != null)
        {
            AudioSource source;
            if (music == null)
            {
                source = audioPool.GetObject().GetComponent<AudioSource>();
                music = source;
            } else
            {
                source = music;
                if (source.clip == chosenAudio.clip[0])
                    return source;
                source.Stop();
            }

            if (globalMuffle && !chosenAudio.immuneToMuffle)
            {
                source.GetComponent<AudioLowPassFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioLowPassFilter>().enabled = false;
            }
            if (globalEcho && !chosenAudio.immuneToEcho)
            {
                source.GetComponent<AudioEchoFilter>().enabled = true;
            }
            else
            {
                source.GetComponent<AudioEchoFilter>().enabled = false;
            }
            source.transform.SetParent(transform);
            source.clip = chosenAudio.clip[Random.Range(0, chosenAudio.clip.Length)];
            source.loop = true;
            if (chosenAudio.randomPitched)
            {
                source.pitch = Random.Range(0.9f, 1.1f);
            }
            if (chosenAudio.isMusic)
            {
                source.volume = PlayerPrefs.GetFloat("Music", 0.5f);
            }
            else
            {
                source.volume = PlayerPrefs.GetFloat("SFX", 1);
            }
            source.Play();
            return source;
        }
        else
        {
            print(nameOfAudio + " doesn't exist in the library, might wanna add it first.");
            return null;
        }
    }

    public void ReturnSound(GameObject sfx)
    {
        audioPool.ReturnObject(sfx);
    }

    IEnumerator ReturnSound(AudioSource source, AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        audioPool.ReturnObject(source.gameObject);
    }
}

[System.Serializable]
public class Audio
{
    public string name;
    public bool randomPitched; // 0.9 to 1.1
    public bool immuneToMuffle;
    public bool immuneToEcho;
    public bool isMusic;
    public AudioClip[] clip;
}
