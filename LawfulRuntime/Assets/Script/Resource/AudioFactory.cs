using StbImageSharp;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEditorInternal;
using UnityEngine;
using static TextureFactory;

public class AudioFactory
{
    public class AudioReference
    {
        public AudioClip unityClip;
        public uint referenceCount;
        public string audioName;
        public bool isReady;
    }

    // We store all loaded sounds here...
    static ConcurrentDictionary<string, AudioReference> AudioList = new();

    public static AudioReference LoadWavFromFile(string filepath)
    {
        // If the file does not exist, throw an exception.
        if (!File.Exists(filepath))
            throw new Exception($"File does not exist '{filepath}'!");

        // If we've already loaded this asset
        if (AudioList.ContainsKey(filepath))
        {
            AudioList[filepath].referenceCount++;
            return AudioList[filepath];
        }

        AudioReference newAudio = new()
        {
            unityClip = null,
            referenceCount = 1,
            audioName = filepath
        };

        AudioList[filepath] = newAudio;

        // Load the data on a task, when it is complete we can create
        Task<PcmFile> decodeTask = new(() =>
        {
            // Load the file
            PcmFile format;

            LoadSamplesFromWav(filepath, out format);
            
            return format;
        });

        decodeTask.ContinueWith((PreviousTask) =>
        {
            newAudio.unityClip = AudioClip.Create(filepath, PreviousTask.Result.samples.Length, PreviousTask.Result.channels, PreviousTask.Result.sampleRate, false);
            newAudio.unityClip.SetData(PreviousTask.Result.samples, 0);
            newAudio.isReady = true;

        }, TaskScheduler.FromCurrentSynchronizationContext());

        decodeTask.Start();

        return newAudio;
    }

    public static void LoadSamplesFromWav(string filepath, out PcmFile formatInfo)
    {
        formatInfo = new PcmFile();

        using (BinaryReader br = new(File.OpenRead(filepath)))  
        {
            // Check "RIFF" tag
            if (br.ReadUInt32() != 0x46464952)
                throw new Exception("Invalid WAV (Not RIFF!)");

            // Skip Size
            br.ReadUInt32();

            // Check "WAVE" tag
            if (br.ReadUInt32() != 0x45564157)
                throw new Exception("Invalid WAV (No WAVE block!)");

            // Check "fmt " tag
            if (br.ReadUInt32() != 0x20746D66)
                throw new Exception("Invalid WAV (Wave Format Bad)");

            // Read the format block
            int formatBlockSize = (int) br.ReadUInt32();

            // Check format
            if (br.ReadUInt16() != 1)
                throw new Exception("Invalid WAV (Must be PCM data!");

            formatInfo.channels       = br.ReadUInt16();
            formatInfo.sampleRate     = (int)br.ReadUInt32();
            br.ReadUInt32();
            br.ReadUInt16();
            formatInfo.bitsPerSample  = br.ReadUInt16();
            formatInfo.bytesPerSample = (formatInfo.bitsPerSample / 8);

            // Skip remaining data with some fuckery
            br.BaseStream.Seek(formatBlockSize - 16, SeekOrigin.Current);

            // Is there a fact block here?..
            uint thisBlockSmells = br.ReadUInt32();
            if (thisBlockSmells == 0x74636166)
                br.BaseStream.Seek(8, SeekOrigin.Current);

            if (br.ReadUInt32() != 0x61746164)
                throw new Exception("Invalid WAV (Expected data block next!)");

            long numberOfSamples = br.ReadUInt32();

            byte[]  samples = br.ReadBytes((int)numberOfSamples);
            float[] final = new float[numberOfSamples / formatInfo.bytesPerSample];

            // We must now convert our sample data to floats...

            for (int i = 0; i < numberOfSamples; i += formatInfo.bytesPerSample)
                final[i / formatInfo.bytesPerSample] = (BitConverter.ToInt16(samples, i) / (float)short.MaxValue);

            // Read all our sample data...
            formatInfo.samples = final;
        }
    }

    public static void FreeAudio(AudioReference audioReference)
    {
        audioReference.referenceCount--;

        if (audioReference.referenceCount == 0)
        {
            audioReference.unityClip = null;

            AudioList.TryRemove(audioReference.audioName, out _);
            audioReference.audioName = null;
        }
    }
}

public struct PcmFile
{
    public int channels;
    public int sampleRate;
    public int bitsPerSample;
    public int bytesPerSample;
    public float[] samples;
}
