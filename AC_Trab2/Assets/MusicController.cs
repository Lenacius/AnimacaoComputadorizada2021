using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    enum FFType { Blackman, BlackmanHarris, Hamming, Hanning, Rectangular, Triangle };

    AudioSource audioSource;

    float[] spectrum = new float[512];
    float[] freqBand = new float[8]; 
    float[] bandBuffer = new float[8];
    float[] bufferDecrease = new float[8];

    float[] freqBandHighest = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];

    public float bd = 0.005f;
    public float bdm = 1.2f;

    public bool trigger;
    float triggerMeter = 0.8f;

    public GameObject holder;
    GameObject[] test = new GameObject[8];

    public float TimeBTWBeat = 0;
    public float period = 0;
    void Start()
    {
        //TestInit();
    }

    void Update()
    {
        GetSpectrum(FFType.Blackman);
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        //Test();

        Trigger();
        //for (int x = 0; x < 5; x++)
        //    if (spectrum[x] * 10 > 5 && !trigger)
        //    {
        //        trigger = true;
        //    }
        //    else
        //        trigger = false;
    }

    void Trigger()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            triggerMeter = 0.8f;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            triggerMeter = 0.9f;
        }

        if (audioBandBuffer[0] >= triggerMeter || audioBandBuffer[1] >= triggerMeter)
            trigger = true;
        else
            trigger = false;

        if (audioBandBuffer[0] >= 0.8f && trigger)
        {
            float aux = TimeBTWBeat;
            TimeBTWBeat = Time.time;
            period = 1 / ((TimeBTWBeat - aux) * 100);
        }
            

    }


    void GetSpectrum(FFType fft)
    {
        if(fft == FFType.Blackman)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        else if (fft == FFType.BlackmanHarris)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        else if (fft == FFType.Hamming)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        else if (fft == FFType.Hanning)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);
        else if (fft == FFType.Rectangular)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        else if(fft == FFType.Triangle)
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Triangle);
    }

    void MakeFrequencyBands()
    {
        int count = 0;
        for(int x = 0; x < freqBand.Length; x++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, x) * 2;

            if(x == 7)
            {
                sampleCount += 2;
            }
            for(int y = 0; y < sampleCount; y++)
            {
                average += spectrum[count] * (count + 1);
                count++;
            }

            average /= count;

            freqBand[x] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int x = 0; x < freqBand.Length; x++)
        {
            if (freqBand[x] > bandBuffer[x])
            {
                bandBuffer[x] = freqBand[x];
                bufferDecrease[x] = bd;
            }
            if (freqBand[x] < bandBuffer[x])
            {
                bandBuffer[x] -= bufferDecrease[x];
                bufferDecrease[x] *= bdm;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int x = 0; x < freqBand.Length; x++)
        {
            if (freqBand[x] > freqBandHighest[x])
            {
                freqBandHighest[x] = freqBand[x];
            }
            audioBand[x] = (freqBand[x] / freqBandHighest[x]);
            audioBandBuffer[x] = (bandBuffer[x] / freqBandHighest[x]);
        }
    }

    void TestInit()
    {
        for (int x = 0; x < 8; x++)
        {
            test[x] = Instantiate(holder, new Vector3(x * 1.5f, 0, 0), transform.rotation);
        }
    }

    void Test()
    {
        for (int x = 0; x < 8; x++)
        {
            test[x].transform.localScale = new Vector3(1, audioBandBuffer[x] * 10, 1);
        }
    }

}
