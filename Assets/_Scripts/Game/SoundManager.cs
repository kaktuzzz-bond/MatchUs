using UnityEngine;

namespace Game
{
    public class SoundManager : Singleton<SoundManager>
    {
        //public AudioClip[] backgroundClips;

        [Range(0, 1)]
        public float musicVolume = 0.5f;

        public float lowPinch;

        public float highPinch;

        public AudioSource PlayClipAt(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (clip != null)
            {
                GameObject go = new("Clip " + clip.name);

                go.transform.position = position;

                AudioSource source = go.AddComponent<AudioSource>();

                source.clip = clip;

                source.volume = volume;

                source.pitch = Random.Range(lowPinch, highPinch);

                source.Play();

                Destroy(go, clip.length);

                return source;
            }

            return null;
        }

        //
        // public AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f)
        // {
        //     if (clips == null) return null;
        //
        //     if (clips.Length == 0) return null;
        //
        //     int randomIndex = Random.Range(0, clips.Length);
        //
        //     if (clips[randomIndex] == null) return null;
        //
        //     AudioSource source = PlayClipAt(clips[randomIndex], position, volume);
        //
        //     return source;
        // }
        //
        // public IEnumerator PlayRandomMusic()
        // {
        //     AudioSource source = PlayRandom(backgroundClips, Vector3.zero, musicVolume);
        //
        //     yield return new WaitForSeconds(source.clip.length);
        //
        //     StartCoroutine(PlayRandomMusic());
        // }
    }
}