using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioSettings : MonoBehaviour {

	EventInstance SFXVolumeTestEvent;

	Bus Music;
	Bus SFX;
	Bus Master_eb;
	float MusicVolume = 0.5f;
	float SFXVolume = 0.5f;
	float MasterVolume = 1f;

	void Awake ()
	{
		Music = RuntimeManager.GetBus ("bus:/Master_eb/Music");
		SFX = RuntimeManager.GetBus ("bus:/Master_eb/SFX");
		Master_eb = RuntimeManager.GetBus ("bus:/Master_eb");
		SFXVolumeTestEvent = RuntimeManager.CreateInstance ("event:/Chair/chair_collisions_PH");
	}
		
	void Update () 
	{
		Music.setVolume (MusicVolume);
		SFX.setVolume (SFXVolume);
		Master_eb.setVolume (MasterVolume);
	}

	public void MasterVolumeLevel (float newMasterVolume)
	{
		MasterVolume = newMasterVolume;
	}

	public void MusicVolumeLevel (float newMusicVolume)
	{
		MusicVolume = newMusicVolume;
	}

	public void SFXVolumeLevel (float newSFXVolume)
	{
		SFXVolume = newSFXVolume;

		PLAYBACK_STATE PbState;
		SFXVolumeTestEvent.getPlaybackState (out PbState);
		if (PbState != PLAYBACK_STATE.PLAYING)
		{
			SFXVolumeTestEvent.start ();
		}
	}
}
