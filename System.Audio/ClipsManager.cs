using System.Collections.Generic;

namespace System.Audio
{
    /// <summary>
    /// Manages the loaded clips
    /// </summary>
    public static class ClipsManager
    {
        internal static List<Clip> LoadedClips = new List<Clip>();

        /// <summary>
        /// The amount of clips loaded
        /// </summary>
        public static int Amount => LoadedClips.Count;

        /// <summary>
        /// The global volume of the app
        /// </summary>
        public static float GlobalVolume
        {
            get => Bass.BASS_GetVolume();
            set
            {
                Bass.BASS_SetVolume(value);
            }
        }

        /// <summary>
        /// Unloads all clips
        /// </summary>
        public static void UnloadAll()
        {
            foreach (var clip in LoadedClips)
            {
                clip.unload();
            }

            LoadedClips.Clear();
        }

        /// <summary>
        /// Plays all clips
        /// </summary>
        public static void PlayAll()
        {
            foreach (var clip in LoadedClips)
            {
                clip.play();
            }
        }

        /// <summary>
        /// Stops all clips
        /// </summary>
        public static void StopAll()
        {
            foreach (var clip in LoadedClips)
            {
                clip.stop();
            }
        }

        /// <summary>
        /// Resumes all clips
        /// </summary>
        public static void ResumeAll()
        {
            foreach (var clip in LoadedClips)
            {
                clip.resume();
            }
        }
    }
}