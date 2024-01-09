using System.IO;

namespace System.Audio
{
    public class Clip
    {
        string path;
        int id;
        
        public double time
        {
            get => Bass.BASS_ChannelBytes2Seconds(id, Bass.BASS_ChannelGetPosition(id));
            set => Bass.BASS_ChannelSetPosition(id, value);
        }

        public float volume
        {
            get
            {
                float v = 1;
                Bass.BASS_ChannelGetAttribute(id, BASSAttribute.BASS_ATTRIB_VOL, ref v);
                return v;
            }

            set => Bass.BASS_ChannelSetAttribute(id, BASSAttribute.BASS_ATTRIB_VOL, value);
        }

        public bool disposed => id == -1;
        
        public Clip(string path, EndAction endAction = EndAction.Nothing)
        {
            if (File.Exists(path))
            {
                this.path = path;

                action = endAction;

                id = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);

                Bass.BASS_ChannelSetSync(id, BASSSync.BASS_SYNC_END, 0, Callback, IntPtr.Zero);
            }
            else
            {
                throw new Exception("File not found!");
            }
        }
        
        public void play()
        {
            Bass.BASS_ChannelPlay(id, true);
        }

        public void stop()
        {
            Bass.BASS_ChannelStop(id);
        }

        public void resume()
        {
            Bass.BASS_ChannelPlay(id, false);
        }

        public void dispose()
        {
            if(!disposed)
            {
                Bass.BASS_StreamFree(id);
                id = -1;
            }
        }

        public void reuse()
        {
            if (disposed)
            {
                id = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
                Bass.BASS_ChannelSetSync(id, BASSSync.BASS_SYNC_END, 0, Callback, IntPtr.Zero);
            }
        }

        public void addListener(Action action)
        {
            OnFinish += action;
        }

        static Clip()
        {
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
        }

        private void Callback(int handle, int channel, int idk, IntPtr user)
        {
            OnFinish?.Invoke();

            switch (action)
            {
                case EndAction.Dispose:
                    dispose();
                    break;

                case EndAction.Loop:
                    play();
                    break;
            }
        }

        EndAction action;
        
        private event Action OnFinish = delegate{ };
        public enum EndAction
        {
            Nothing,
            Dispose,
            Loop
        }
    }
}