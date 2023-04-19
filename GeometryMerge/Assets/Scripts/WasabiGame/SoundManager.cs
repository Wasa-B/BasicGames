using UnityEngine;
using UnityEngine.SceneManagement;
namespace WasabiGame
{
    [System.Serializable]
    public class SoundSetting
    {
        [SerializeField]
        float bgm;
        [SerializeField]
        float fx;

        public float BGM { get { return bgm; } set { bgm = value; } }
        public float FX { get { return fx; } set { fx = value; } }
    }

    [System.Serializable]
    public class ClipStatus
    {
        [SerializeField]
        AudioClip clip;
        [SerializeField]
        float volume = 1;
        [SerializeField]
        SoundManager.SoundType soundType;

        public void Play()
        {
            SoundManager.Play(clip, soundType: soundType, volume: volume);
        }
    }

    public class SoundManager
    {
        static SoundManager instance;
        static SoundManager Instance { get { if (instance == null) instance = new SoundManager(); return instance; } }
        public enum SoundType { BGM, FX, COUNT }
        public SoundSetting soundSetting;

        const int FX_COUNT = 10;

        AudioSource BGM_player;
        AudioSource[] FX_player = new AudioSource[FX_COUNT];
        int fx_index = 0;

        public SoundManager()
        {
            SceneManager.activeSceneChanged += activeSceneChanged;
            Init();
        }
        void Init()
        {
            var gobj = new GameObject();
            gobj.transform.SetParent(Camera.main.transform);
            for(int i = 0; i < FX_COUNT; i++)
                FX_player[i] = gobj.AddComponent<AudioSource>();
        }
        private void activeSceneChanged(Scene arg0, Scene arg1)
        {

        }
        void PlaySound(AudioClip clip, SoundType soundType = SoundType.FX, float volume = 1)
        {
            switch (soundType)
            {
                case SoundType.BGM:
                    break;
                case SoundType.FX:
                    FX_Play(clip, volume);
                    break;
                case SoundType.COUNT:
                    break;
            }
            
        }
        void FX_Play(AudioClip clip, float volume)
        {
            FX_player[fx_index].PlayOneShot(clip, volume);
            fx_index++;
            if(fx_index >= FX_COUNT)fx_index = 0;
        }

        static public void Play(AudioClip clip, SoundType soundType = SoundType.FX, float volume = 1)
        {
            Instance.PlaySound(clip, soundType: soundType, volume: volume);
        }
    }
}