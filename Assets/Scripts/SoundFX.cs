using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType {
    CLICK,
    YES,
    NO,
    BENAR,
    SALAH,
    TYPE,
    WIN,
    LOSE,
    START_GAME,
	SWOOSH
}

public enum BGMType {
    MAIN_MENU,
    GAMEPLAY_1,
    GAMEPLAY_2
}
[System.Serializable]
public class AudioClass {
	public SfxType Type;
	public AudioClip SFX;
}

public class SoundFX : MonoBehaviour {
    [SerializeField]
	List <AudioClass> _AudioList;
    [SerializeField]
	List <AudioSource> _AudioSub;

	private AudioSource _MainAudio;

    [SerializeField]
    AudioSource _BGM;

    [SerializeField]
    AudioClip _MainMenuBGM;
    [SerializeField]
    AudioClip _Gameplay1;
    [SerializeField]
    AudioClip _Gameplay2;

    // Use this for initialization

    void Awake (){
		_MainAudio = GetComponent<AudioSource>();
        EventManager.AddListener<SFXPlayEvent>(PlaySFX);
        EventManager.AddListener<BGMEvent>(BGMHandler);
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            _AudioSub.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }
    }

    private void BGMHandler(BGMEvent e)
    {
        _BGM.Stop();
        switch (e.Type)
        {
            case BGMType.MAIN_MENU:
                _BGM.clip = _MainMenuBGM;
                _BGM.Play();
                break;
            case BGMType.GAMEPLAY_1:
                _BGM.clip = _Gameplay1;
                _BGM.Play();
                break;
            case BGMType.GAMEPLAY_2:
                _BGM.clip = _Gameplay2;
                _BGM.Play();
                break;

        }
    }

    public void PlaySFX (SFXPlayEvent e)
    {
		bool isFind = false;
		bool notPlay = false;

		if (e.IsEnd){
			_MainAudio.Stop();
			for (int i = 0 ; i < _AudioList.Count && !isFind; i++){
				if (_AudioList[i].Type == e.Sfx){
					isFind = true;
					_MainAudio.clip = _AudioList[i].SFX;
					_MainAudio.Play();
				}
			}
		}
		else{
			for (int i = 0 ; i < _AudioList.Count && !isFind; i++){
				if (_AudioList[i].Type == e.Sfx)
                {
					isFind = true;
					for (int x = 0; x < _AudioSub.Count && !notPlay;x++){
						if (!_AudioSub[x].isPlaying){
							_AudioSub[x].Stop();
							_AudioSub[x].clip = _AudioList[i].SFX;
							_AudioSub[x].Play();
							notPlay = true;
						}
					}
				}
			}
		}
	}
}
