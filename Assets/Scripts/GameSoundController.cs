using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameSoundController : MonoBehaviour {

	public static bool isSoundEnabled = true;
	public Sprite soundon, soundoff;

	float animationTime = 0f;
	int a = 1;

	public AudioSource backgroundMusic;
	public float maxVolumeCoef;
	public AudioSource soundEffects;
	public AudioClip buttonClick;
	public AudioClip popEffect1;
	public AudioClip popEffect2;
	public AudioClip badClick;
	public AudioClip winSound;
	public AudioClip shortWinSound;
	public AudioClip loseSound;


	public void Start()
	{
		print (isSoundEnabled);
		if (!isSoundEnabled) {
			SoundSpriteSwap ();
			soundEffects.mute = !soundEffects.mute;
			backgroundMusic.mute = !backgroundMusic.mute;
		}
	}

	public void SoundButtonClick()
	{
		isSoundEnabled = !isSoundEnabled;
		SoundSpriteSwap ();
		soundEffects.mute = !soundEffects.mute;
		backgroundMusic.mute = !backgroundMusic.mute;
	}

	void SoundSpriteSwap()
	{
		if (this.gameObject.GetComponent<Image> ().sprite == soundon)
			this.gameObject.GetComponent<Image> ().sprite = soundoff;
		else this.gameObject.GetComponent<Image> ().sprite = soundon;
	}

	public void ButtonClick()
	{
		soundEffects.clip = buttonClick;
		soundEffects.PlayOneShot (soundEffects.clip);
	}

	public void PopEffect1()
	{
		soundEffects.clip = popEffect1;
		soundEffects.PlayOneShot (popEffect1);
	}

	public void PopEffect2()
	{
		soundEffects.clip = popEffect2;
		soundEffects.PlayOneShot (popEffect2);
	}

	public void BadClick()
	{
		soundEffects.clip = badClick;
		soundEffects.PlayOneShot (badClick);
	}

	public void WinEffect(float time)
	{
		soundEffects.clip = winSound;
		Invoke ("InvokeWin", time);
	}

	public void ShortWinEffect()
	{
		soundEffects.clip = shortWinSound;
		soundEffects.PlayOneShot (shortWinSound);
	}

	void InvokeWin()
	{
		soundEffects.PlayOneShot (winSound);
	}

	public void LoseEffect(float time)
	{
		soundEffects.clip = loseSound;
		Invoke ("InvokeLose", time);
	}

	void InvokeLose()
	{
		soundEffects.PlayOneShot (loseSound);
	}

	public void PlayMusic()
	{
		a = 1;
		animationTime = 2f;

		backgroundMusic.Play ();
	}

	public void StopMusic()
	{
		print ("stopping");

		a = -1;
		animationTime = 2f;

	}

	// Update is called once per frame
	void Update () {
		if (animationTime > 0) {
			Fade ();
			animationTime -= Time.deltaTime;
		} else
			animationTime = 0;
	}

	void Fade()
	{
		backgroundMusic.volume += a * maxVolumeCoef * Time.deltaTime;
	}
}
