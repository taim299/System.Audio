using System.Runtime.InteropServices;

namespace System.Audio
{
    internal static class Bass
    {
        public delegate void SYNCPROC(int handle, int channel, int data, IntPtr user);

        public static bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win)
        {
            return BASS_Init(device, freq, flags, win, IntPtr.Zero);
        }

        public static int BASS_StreamCreateFile(string file, long offset, long length, BASSFlag flags)
        {
            flags |= BASSFlag.BASS_UNICODE;
            return BASS_StreamCreateFileUnicode(false, file, offset, length, flags);
        }

        public static long BASS_ChannelGetPosition(int handle)
        {
            return BASS_ChannelGetPosition(handle, BASSMode.BASS_POS_BYTE);
        }

        public static bool BASS_ChannelSetPosition(int handle, double seconds)
        {
            return BASS_ChannelSetPosition(handle, BASS_ChannelSeconds2Bytes(handle, seconds), BASSMode.BASS_POS_BYTE);
        }

        public static bool BASS_ChannelSetPosition(int handle, long pos)
        {
            return BASS_ChannelSetPosition(handle, pos, BASSMode.BASS_POS_BYTE);
        }

        public static bool BASS_ChannelSetPosition(int handle, int order, int row)
        {
            return BASS_ChannelSetPosition(handle, MakeLong(order, row), BASSMode.BASS_POS_MUSIC_ORDER);

            int MakeLong(int lowWord, int highWord)
            {
                return (highWord << 16) | (lowWord & 0xFFFF);
            }
        }

        [DllImport("bass")]
        public static extern int BASS_ChannelSetSync(int handle, BASSSync type, long param, SYNCPROC proc, IntPtr user);

        [DllImport("bass", EntryPoint = "BASS_PluginLoad")]
        private static extern int BASS_PluginLoadUnicode([In][MarshalAs(UnmanagedType.LPWStr)] string file, BASSFlag flags);
        
        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_Free();

        [DllImport("bass")]
        public static extern float BASS_GetCPU();

        [DllImport("bass")]
        public static extern int BASS_GetDevice();

        [DllImport("bass")]
        public static extern int BASS_GetVersion();

        [DllImport("bass")]
        public static extern float BASS_GetVolume();

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_Pause();

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelStop(int handle);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_SetVolume(float volume);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_StreamFree(int handle);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelIsSliding(int handle, BASSAttribute attrib);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelPause(int handle);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelPlay(int handle, [MarshalAs(UnmanagedType.Bool)] bool restart);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelSlideAttribute(int handle, BASSAttribute attrib, float value, int time);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelSetAttribute(int handle, BASSAttribute attrib, float value);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelGetAttribute(int handle, BASSAttribute attrib, ref float value);

        [DllImport("bass")]
        public static extern long BASS_ChannelSeconds2Bytes(int handle, double pos);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BASS_ChannelSetPosition(int handle, long pos, BASSMode mode);
        
        [DllImport("bass")]
        public static extern long BASS_ChannelGetPosition(int handle, BASSMode mode);
        
        [DllImport("bass")]
        public static extern double BASS_ChannelBytes2Seconds(int handle, long pos);

        [DllImport("bass", EntryPoint = "BASS_StreamCreateFile")]
        private static extern int BASS_StreamCreateFileUnicode([MarshalAs(UnmanagedType.Bool)] bool mem, [In][MarshalAs(UnmanagedType.LPWStr)] string file, long offset, long length, BASSFlag flags);

        [DllImport("bass")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BASS_Init(int device, int freq, BASSInit flags, IntPtr win, IntPtr clsid);
    }

    [Flags]
    internal enum BASSSync
    {
        BASS_SYNC_POS = 0,
        BASS_SYNC_MUSICINST = 1,
        BASS_SYNC_END = 2,
        BASS_SYNC_MUSICFX = 3,
        BASS_SYNC_META = 4,
        BASS_SYNC_SLIDE = 5,
        BASS_SYNC_STALL = 6,
        BASS_SYNC_DOWNLOAD = 7,
        BASS_SYNC_FREE = 8,
        BASS_SYNC_MUSICPOS = 0xA,
        BASS_SYNC_SETPOS = 0xB,
        BASS_SYNC_OGG_CHANGE = 0xC,
        BASS_SYNC_DEV_FAIL = 0xE,
        BASS_SYNC_DEV_FORMAT = 0xF,
        BASS_SYNC_THREAD = 0x20000000,
        BASS_SYNC_MIXTIME = 0x40000000,
        BASS_SYNC_ONETIME = int.MinValue,
        BASS_SYNC_MIXER_ENVELOPE = 0x10200,
        BASS_SYNC_MIXER_ENVELOPE_NODE = 0x10201,
        BASS_SYNC_MIXER_QUEUE = 0x10202,
        BASS_SYNC_WMA_CHANGE = 0x10100,
        BASS_SYNC_WMA_META = 0x10101,
        BASS_SYNC_CD_ERROR = 0x3E8,
        BASS_SYNC_CD_SPEED = 0x3EA,
        BASS_WINAMP_SYNC_BITRATE = 0x64,
        BASS_SYNC_MIDI_MARKER = 0x10000,
        BASS_SYNC_MIDI_CUE = 0x10001,
        BASS_SYNC_MIDI_LYRIC = 0x10002,
        BASS_SYNC_MIDI_TEXT = 0x10003,
        BASS_SYNC_MIDI_EVENT = 0x10004,
        BASS_SYNC_MIDI_TICK = 0x10005,
        BASS_SYNC_MIDI_TIMESIG = 0x10006,
        BASS_SYNC_MIDI_KEYSIG = 0x10007,
        BASS_SYNC_HLS_SEGMENT = 0x10300,
        BASS_SYNC_HLS_SDT = 0x10301
    }

    internal enum BASSAttribute
    {
        BASS_ATTRIB_FREQ = 1,
        BASS_ATTRIB_VOL = 2,
        BASS_ATTRIB_PAN = 3,
        BASS_ATTRIB_EAXMIX = 4,
        BASS_ATTRIB_NOBUFFER = 5,
        BASS_ATTRIB_VBR = 6,
        BASS_ATTRIB_CPU = 7,
        BASS_ATTRIB_SRC = 8,
        BASS_ATTRIB_NET_RESUME = 9,
        BASS_ATTRIB_SCANINFO = 10,
        BASS_ATTRIB_NORAMP = 11,
        BASS_ATTRIB_BITRATE = 12,
        BASS_ATTRIB_BUFFER = 13,
        BASS_ATTRIB_GRANULE = 14,
        BASS_ATTRIB_USER = 15,
        BASS_ATTRIB_TAIL = 16,
        BASS_ATTRIB_PUSH_LIMIT = 17,
        BASS_ATTRIB_DOWNLOADPROC = 18,
        BASS_ATTRIB_VOLDSP = 19,
        BASS_ATTRIB_VOLDSP_PRIORITY = 20,
        BASS_ATTRIB_MUSIC_AMPLIFY = 256,
        BASS_ATTRIB_MUSIC_PANSEP = 257,
        BASS_ATTRIB_MUSIC_PSCALER = 258,
        BASS_ATTRIB_MUSIC_BPM = 259,
        BASS_ATTRIB_MUSIC_SPEED = 260,
        BASS_ATTRIB_MUSIC_VOL_GLOBAL = 261,
        BASS_ATTRIB_MUSIC_ACTIVE = 262,
        BASS_ATTRIB_MUSIC_VOL_CHAN = 512,
        BASS_ATTRIB_MUSIC_VOL_INST = 768,
        BASS_ATTRIB_TEMPO = 65536,
        BASS_ATTRIB_TEMPO_PITCH = 65537,
        BASS_ATTRIB_TEMPO_FREQ = 65538,
        BASS_ATTRIB_TEMPO_OPTION_USE_AA_FILTER = 65552,
        BASS_ATTRIB_TEMPO_OPTION_AA_FILTER_LENGTH = 65553,
        BASS_ATTRIB_TEMPO_OPTION_USE_QUICKALGO = 65554,
        BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS = 65555,
        BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS = 65556,
        BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS = 65557,
        BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK = 65558,
        BASS_ATTRIB_REVERSE_DIR = 69632,
        BASS_ATTRIB_MIDI_PPQN = 73728,
        BASS_ATTRIB_MIDI_CPU = 73729,
        BASS_ATTRIB_MIDI_CHANS = 73730,
        BASS_ATTRIB_MIDI_VOICES = 73731,
        BASS_ATTRIB_MIDI_VOICES_ACTIVE = 73732,
        BASS_ATTRIB_MIDI_STATE = 73733,
        BASS_ATTRIB_MIDI_SRC = 73734,
        BASS_ATTRIB_MIDI_KILL = 73735,
        BASS_ATTRIB_MIDI_SPEED = 73736,
        BASS_ATTRIB_MIDI_REVERB = 73737,
        BASS_ATTRIB_MIDI_VOL = 73738,
        BASS_ATTRIB_MIDI_TRACK_VOL = 73984,
        BASS_ATTRIB_OPUS_ORIGFREQ = 77824,
        BASS_ATTRIB_DSD_GAIN = 81920,
        BASS_ATTRIB_DSD_RATE = 81921,
        BASS_ATTRIB_MIXER_LATENCY = 86016,
        BASS_ATTRIB_MIXER_THREADS = 86017,
        BASS_ATTRIB_MIXER_VOL = 86018,
        BASS_ATTRIB_SPLIT_ASYNCBUFFER = 86032,
        BASS_ATTRIB_SPLIT_ASYNCPERIOD = 86033,
        BASS_ATTRIB_WEBM_TRACK = 90112,
        BASS_SLIDE_LOG = 16777216
    }

    [Flags]
    internal enum BASSMode
    {
        BASS_POS_BYTE = 0,
        BASS_POS_MUSIC_ORDER = 1,
        BASS_POS_MIDI_TICK = 2,
        BASS_POS_OGG = 3,
        BASS_POS_CD_TRACK = 4,
        BASS_POS_MIXER_DELAY = 5,
        BASS_POS_END = 0x10,
        BASS_POS_LOOP = 0x11,
        BASS_POS_FLUSH = 0x1000000,
        BASS_POS_RESET = 0x2000000,
        BASS_POS_RELATIVE = 0x4000000,
        BASS_POS_INEXACT = 0x8000000,
        BASS_MUSIC_POSRESET = 0x8000,
        BASS_MUSIC_POSRESETEX = 0x400000,
        BASS_MIXER_CHAN_NORAMPIN = 0x800000,
        BASS_POS_MIXER_RESET = 0x10000,
        BASS_POS_DECODE = 0x10000000,
        BASS_POS_DECODETO = 0x20000000,
        BASS_POS_SCAN = 0x40000000,
        BASS_MIDI_DECAYSEEK = 0x4000
    }

    [Flags]
    internal enum BASSFlag
    {
        BASS_DEFAULT = 0,
        BASS_SAMPLE_8BITS = 1,
        BASS_SAMPLE_MONO = 2,
        BASS_SAMCHAN_NEW = 1,
        BASS_SAMCHAN_STREAM = 2,
        BASS_SAMPLE_LOOP = 4,
        BASS_SAMPLE_3D = 8,
        BASS_SAMPLE_SOFTWARE = 0x10,
        BASS_SAMPLE_MUTEMAX = 0x20,
        BASS_SAMPLE_VAM = 0x40,
        BASS_SAMPLE_FX = 0x80,
        BASS_SAMPLE_FLOAT = 0x100,
        BASS_RECORD_PAUSE = 0x8000,
        BASS_RECORD_ECHOCANCEL = 0x2000,
        BASS_RECORD_AGC = 0x4000,
        BASS_STREAM_PRESCAN = 0x20000,
        BASS_STREAM_AUTOFREE = 0x40000,
        BASS_STREAM_RESTRATE = 0x80000,
        BASS_STREAM_BLOCK = 0x100000,
        BASS_STREAM_DECODE = 0x200000,
        BASS_STREAM_STATUS = 0x800000,
        BASS_SPEAKER_FRONT = 0x1000000,
        BASS_SPEAKER_REAR = 0x2000000,
        BASS_SPEAKER_CENLFE = 0x3000000,
        BASS_SPEAKER_REAR2 = 0x4000000,
        BASS_SPEAKER_SIDE = 0x4000000,
        BASS_SPEAKER_LEFT = 0x10000000,
        BASS_SPEAKER_RIGHT = 0x20000000,
        BASS_SPEAKER_FRONTLEFT = 0x11000000,
        BASS_SPEAKER_FRONTRIGHT = 0x21000000,
        BASS_SPEAKER_REARLEFT = 0x12000000,
        BASS_SPEAKER_REARRIGHT = 0x22000000,
        BASS_SPEAKER_CENTER = 0x13000000,
        BASS_SPEAKER_LFE = 0x23000000,
        BASS_SPEAKER_SIDELEFT = 0x14000000,
        BASS_SPEAKER_SIDERIGHT = 0x24000000,
        BASS_SPEAKER_REAR2LEFT = 0x14000000,
        BASS_SPEAKER_REAR2RIGHT = 0x24000000,
        BASS_SPEAKER_PAIR1 = 0x1000000,
        BASS_SPEAKER_PAIR2 = 0x2000000,
        BASS_SPEAKER_PAIR3 = 0x3000000,
        BASS_SPEAKER_PAIR4 = 0x4000000,
        BASS_SPEAKER_PAIR5 = 0x5000000,
        BASS_SPEAKER_PAIR6 = 0x6000000,
        BASS_SPEAKER_PAIR7 = 0x7000000,
        BASS_SPEAKER_PAIR8 = 0x8000000,
        BASS_SPEAKER_PAIR9 = 0x9000000,
        BASS_SPEAKER_PAIR10 = 0xA000000,
        BASS_SPEAKER_PAIR11 = 0xB000000,
        BASS_SPEAKER_PAIR12 = 0xC000000,
        BASS_SPEAKER_PAIR13 = 0xD000000,
        BASS_SPEAKER_PAIR14 = 0xE000000,
        BASS_SPEAKER_PAIR15 = 0xF000000,
        BASS_ASYNCFILE = 0x40000000,
        BASS_UNICODE = int.MinValue,
        BASS_SAMPLE_OVER_VOL = 0x10000,
        BASS_SAMPLE_OVER_POS = 0x20000,
        BASS_SAMPLE_OVER_DIST = 0x30000,
        BASS_WV_STEREO = 0x400000,
        BASS_AC3_DOWNMIX_2 = 0x200,
        BASS_AC3_DOWNMIX_4 = 0x400,
        BASS_DSD_RAW = 0x200,
        BASS_DSD_DOP = 0x400,
        BASS_DSD_DOP_AA = 0x800,
        BASS_AC3_DOWNMIX_DOLBY = 0x600,
        BASS_AC3_DYNAMIC_RANGE = 0x800,
        BASS_AAC_FRAME960 = 0x1000,
        BASS_AAC_STEREO = 0x400000,
        BASS_MIXER_END = 0x10000,
        BASS_MIXER_CHAN_PAUSE = 0x20000,
        BASS_MIXER_NONSTOP = 0x20000,
        BASS_MIXER_RESUME = 0x1000,
        BASS_MIXER_CHAN_ABSOLUTE = 0x1000,
        BASS_MIXER_POSEX = 0x2000,
        BASS_MIXER_NOSPEAKER = 0x4000,
        BASS_MIXER_CHAN_LIMIT = 0x4000,
        BASS_MIXER_LIMIT = 0x4000,
        BASS_MIXER_QUEUE = 0x8000,
        BASS_MIXER_CHAN_MATRIX = 0x10000,
        BASS_MIXER_MATRIX = 0x10000,
        BASS_MIXER_CHAN_DOWNMIX = 0x400000,
        BASS_MIXER_CHAN_NORAMPIN = 0x800000,
        BASS_MIXER_NORAMPIN = 0x800000,
        BASS_SPLIT_SLAVE = 0x1000,
        BASS_MIXER_CHAN_BUFFER = 0x2000,
        BASS_MIXER_BUFFER = 0x2000,
        BASS_SPLIT_POS = 0x2000,
        BASS_CD_SUBCHANNEL = 0x200,
        BASS_CD_SUBCHANNEL_NOHW = 0x400,
        BASS_CD_C2ERRORS = 0x800,
        BASS_MIDI_NODRUMPARAM = 0x400,
        BASS_MIDI_NOSYSRESET = 0x800,
        BASS_MIDI_DECAYEND = 0x1000,
        BASS_MIDI_NOFX = 0x2000,
        BASS_MIDI_DECAYSEEK = 0x4000,
        BASS_MIDI_NOCROP = 0x8000,
        BASS_MIDI_NOTEOFF1 = 0x10000,
        BASS_MIDI_ASYNC = 0x400000,
        BASS_MIDI_SINCINTER = 0x800000,
        BASS_MIDI_FONT_MEM = 0x10000,
        BASS_MIDI_FONT_MMAP = 0x20000,
        BASS_MIDI_FONT_XGDRUMS = 0x40000,
        BASS_MIDI_FONT_NOFX = 0x80000,
        BASS_MIDI_FONT_LINATTMOD = 0x100000,
        BASS_MIDI_FONT_LINDECVOL = 0x200000,
        BASS_MIDI_FONT_NORAMPIN = 0x400000,
        BASS_MIDI_FONT_NOLIMITS = 0x800000,
        BASS_MIDI_FONT_MINFX = 0x1000000,
        BASS_MIDI_PACK_NOHEAD = 1,
        BASS_MIDI_PACK_16BIT = 2,
        BASS_MIDI_PACK_48KHZ = 4,
        BASS_FX_FREESOURCE = 0x10000,
        BASS_FX_BPM_BKGRND = 1,
        BASS_FX_BPM_MULT2 = 2,
        BASS_FX_TEMPO_ALGO_LINEAR = 0x200,
        BASS_FX_TEMPO_ALGO_CUBIC = 0x400,
        BASS_FX_TEMPO_ALGO_SHANNON = 0x800,
        BASS_MUSIC_FLOAT = 0x100,
        BASS_MUSIC_MONO = 2,
        BASS_MUSIC_LOOP = 4,
        BASS_MUSIC_3D = 8,
        BASS_MUSIC_FX = 0x80,
        BASS_MUSIC_AUTOFREE = 0x40000,
        BASS_MUSIC_DECODE = 0x200000,
        BASS_MUSIC_PRESCAN = 0x20000,
        BASS_MUSIC_RAMP = 0x200,
        BASS_MUSIC_RAMPS = 0x400,
        BASS_MUSIC_SURROUND = 0x800,
        BASS_MUSIC_SURROUND2 = 0x1000,
        BASS_MUSIC_FT2PAN = 0x2000,
        BASS_MUSIC_FT2MOD = 0x2000,
        BASS_MUSIC_PT1MOD = 0x4000,
        BASS_MUSIC_NONINTER = 0x10000,
        BASS_MUSIC_SINCINTER = 0x800000,
        BASS_MUSIC_POSRESET = 0x8000,
        BASS_MUSIC_POSRESETEX = 0x400000,
        BASS_MUSIC_STOPBACK = 0x80000,
        BASS_MUSIC_NOSAMPLE = 0x100000
    }

    [Flags]
    internal enum BASSInit
    {
        BASS_DEVICE_DEFAULT = 0,
        BASS_DEVICE_MONO = 2,
        BASS_DEVICE_16BITS = 8,
        BASS_DEVICE_REINIT = 0x80,
        BASS_DEVICE_LATENCY = 0x100,
        BASS_DEVICE_CPSPEAKERS = 0x400,
        BASS_DEVICE_SPEAKERS = 0x800,
        BASS_DEVICE_NOSPEAKER = 0x1000,
        BASS_DEVIDE_DMIX = 0x2000,
        BASS_DEVICE_FREQ = 0x4000,
        BASS_DEVICE_STEREO = 0x8000,
        BASS_DEVICE_HOG = 0x10000,
        BASS_DEVICE_AUDIOTRACK = 0x20000,
        BASS_DEVICE_DSOUND = 0x40000,
        BASS_DEVICE_SOFTWARE = 0x80000
    }
}