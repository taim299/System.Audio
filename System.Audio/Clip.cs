using System.IO;

namespace System.Audio
{
    public sealed class Clip
    {
        private Action assignedAction;
        private EndAction endAction;
        private string path;
        private int id;
        
        /// <summary>
        /// Gets and sets the playback time (set can be a bit slow)
        /// </summary>
        public double time
        {
            get => Bass.BASS_ChannelBytes2Seconds(id, Bass.BASS_ChannelGetPosition(id));
            set => Bass.BASS_ChannelSetPosition(id, value);
        }

        //Gets and sets the volume of the playback
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

        public bool loaded => id != -1;
        
        public Clip(string path, EndAction endAction = EndAction.Nothing, Action callback = null)
        {
            if (File.Exists(path))
            {
                this.path = path;

                this.endAction = endAction;

                id = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);

                Bass.BASS_ChannelSetSync(id, BASSSync.BASS_SYNC_END, 0, Callback, IntPtr.Zero);

                assignedAction = callback;
            }
            else
            {
                throw new FileNotFoundException($"The file does not exist in the given path '{path}'");
            }
        }
        
        /// <summary>
        /// Plays or restarts the playback from the start
        /// </summary>
        public void play()
        {
            Bass.BASS_ChannelPlay(id, true);
        }

        /// <summary>
        /// Stops the playback
        /// </summary>
        public void stop()
        {
            Bass.BASS_ChannelStop(id);
        }
        
        /// <summary>
        /// Resumes the playback from the current time
        /// </summary>
        public void resume()
        {
            Bass.BASS_ChannelPlay(id, false);
        }

        /// <summary>
        /// Unloads it from memory if loaded
        /// </summary>
        public void unload()
        {
            if(loaded)
            {
                Bass.BASS_StreamFree(id);
                id = -1;
            }
        }

        /// <summary>
        /// Reloads it to memory if not loaded
        /// </summary>
        public void reload()
        {
            if (!loaded)
            {
                id = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
                Bass.BASS_ChannelSetSync(id, BASSSync.BASS_SYNC_END, 0, Callback, IntPtr.Zero);
            }
        }
        
        /// <summary>
        /// Changes the action to call when the playback ends, use null to remove it
        /// </summary>
        /// <param name="action">The action to call</param>
        public void ChangeListener(Action action)
        {
            assignedAction = action;
        }

        /// <summary>
        /// Changes the action to do when the playback ends
        /// </summary>
        /// <param name="endAction">The action to do</param>
        public void ChangeEndAction(EndAction endAction)
        {
            this.endAction = endAction;
        }

        static Clip()
        {
            Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
        }

        private void Callback(int handle, int channel, int idk, IntPtr user)
        {
            assignedAction?.Invoke();

            switch (endAction)
            {
                case EndAction.Dispose:
                    unload();
                    break;

                case EndAction.Loop:
                    play();
                    break;
            }
        }
        
        /// <summary>
        /// Actions to perform when the playback is over
        /// </summary>
        public enum EndAction
        {
            Nothing,
            Dispose,
            Loop
        }
    }
}