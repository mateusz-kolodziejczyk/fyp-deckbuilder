using Helper;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeManagement : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private TMP_InputField mainVolumeInput, musicVolumeInput, sfxVolumeInput;
        [SerializeField] private Slider mainVolumeSlider, musicVolumeSlider, sfxVolumeSlider;

        [SerializeField] [Range(-80, 20)] private float minVolume, maxVolume;
        [SerializeField] [Range(0, 100)] private float muteThreshold;
    

        private void Start()
        {
            if (maxVolume < minVolume)
            {
                maxVolume = minVolume;
            }
            SetupSliders();
        }

        private void SetupSliders()
        {
            // Main
            mainVolumeSlider.maxValue = maxVolume;
            mainVolumeSlider.minValue = minVolume;
            mainVolumeSlider.value = maxVolume;
        
            // Music
            musicVolumeSlider.maxValue = maxVolume;
            musicVolumeSlider.minValue = minVolume;
            musicVolumeSlider.value = maxVolume;
        
            // SFX
            sfxVolumeSlider.maxValue = maxVolume;
            sfxVolumeSlider.minValue = minVolume;
            sfxVolumeSlider.value = maxVolume;
        }

        public void SetMainVolume(float value)
        {
            SetVolume(value, "mainVol");
            var newText = VolumeFloatToString(value);
            mainVolumeInput.text = newText;
        }
        public void SetMusicVolume(float value)
        {
            SetVolume(value, "musicVol");
            var newText = VolumeFloatToString(value);
            musicVolumeInput.text = newText;
        }
        public void SetSFXVolume(float value)
        {
            SetVolume(value, "sfxVol");
            var newText = VolumeFloatToString(value);
            sfxVolumeInput.text = newText;
        }
        private void SetVolume(float value, string parameter)
        {
            mixer.SetFloat(parameter, value);
            // If below mute threshold, set volume to -80
            if (VolumeIsBelowMuteThreshold(value))
            {
                mixer.SetFloat(parameter, -80);
            }
        }
    
        public void SetMainVolume(string text)
        {
            var (result, newVol) = SetVolume(text, "mainVol");
            if (result)
            {
                mainVolumeSlider.value = newVol;
            }
        }
        public void SetMusicVolume(string text)
        {
            var (result, newVol) = SetVolume(text, "musicVol");
            if (result)
            {
                musicVolumeSlider.value = newVol;
            }
        }
        public void SetSFXVolume(string text)
        {
            var (result, newVol) = SetVolume(text, "sfxVol");
            if (result)
            {
                sfxVolumeSlider.value = newVol;
            }
        }

        private (bool, float) SetVolume(string text, string parameter)
        {
            var newVolume = VolumeStringToFloat(text);
            if (newVolume >= minVolume && newVolume <= maxVolume)
            {
                mixer.SetFloat(parameter, newVolume);
                // If below mute threshold, set volume to -80
                if (VolumeIsBelowMuteThreshold(newVolume))
                {
                    mixer.SetFloat(parameter, -80);
                }

                return (true, newVolume);
            }

            return (false, newVolume);
        }
        private bool VolumeIsBelowMuteThreshold(float volume)
        {
            var normalisedVolume = RangeConversionHelpers.RangeConversion(volume, maxVolume, minVolume, 100, 0);
            return normalisedVolume <= muteThreshold;
        }

        private float VolumeStringToFloat(string text)
        {
            if (!int.TryParse(text, out var oldVal))
            {
                // Return an impossible value if string does not convert
                return float.MinValue;
            }
            // convert text that is 0-100 to whatever the volume is based on
            // using code from https://stackoverflow.com/a/929107
            return RangeConversionHelpers.RangeConversion(oldVal, 100, 0, maxVolume, minVolume);
        }

        private string VolumeFloatToString(float value)
        {
            var newVal = (int)RangeConversionHelpers.RangeConversion(value, maxVolume, minVolume, 100, 0);
            return newVal.ToString();
        }
    }
}
