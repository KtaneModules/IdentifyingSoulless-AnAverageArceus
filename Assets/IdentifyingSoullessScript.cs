﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class IdentifyingSoullessScript : MonoBehaviour
{
	public KMAudio Audio;
    public KMBombInfo Bomb;
    public KMBombModule Module;
	public AudioSource SoullessMusic;
	
	public KMSelectable[] TypableText;
	public KMSelectable[] ShiftButtons;
	public KMSelectable[] UselessButtons;
	public KMSelectable Backspace;
	public KMSelectable Enter;
	public KMSelectable SpaceBar;
	public KMSelectable Border;
	
	public SpriteRenderer MainSprite;
    public Sprite GetSmoked;
    public Material[] ImageLighting;
	
	public MeshRenderer[] LightBulbs;
	public Material[] TheLights;
	
	public TextMesh[] Text;
	public TextMesh TextBox;
	public GameObject TheBox;
	
	bool Shifted = false;
	
	public AudioClip[] NotBuffer;
    public AudioClip[] SoullessSections;
    private AudioClip PlayTheSection;
	
	string[][] ChangedText = new string[2][]{
		new string[47] {"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\", "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'", "z", "x", "c", "v", "b", "n", "m", ",", ".", "/"},
		new string[47] {"~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|", "A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\"", "Z", "X", "C", "V", "B", "N", "M", "<", ">", "?"}
	};
	
	bool Playable = false;
	bool Enterable = false;
	bool Toggleable = true;
	int Stages = 0;
    int SolveSound = 0;
	
	//Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool ModuleSolved;

	void Awake()
	{
		moduleId = moduleIdCounter++;
        SolveSound = UnityEngine.Random.Range(0, 50);
		for (int b = 0; b < TypableText.Count(); b++)
        {
            int KeyPress = b;
            TypableText[KeyPress].OnInteract += delegate
            {
                TypableKey(KeyPress);
				return false;
            };
        }
		
		for (int a = 0; a < ShiftButtons.Count(); a++)
        {
            int Shifting = a;
            ShiftButtons[Shifting].OnInteract += delegate
            {
                PressShift(Shifting);
				return false;
            };
        }
		
		for (int c = 0; c < UselessButtons.Count(); c++)
        {
            int Useless = c;
            UselessButtons[Useless].OnInteract += delegate
            {
                UselessButtons[Useless].AddInteractionPunch(.2f);
				Audio.PlaySoundAtTransform(NotBuffer[1].name, transform);
				return false;
            };
        }
		
		Backspace.OnInteract += delegate () { PressBackspace(); return false; };
		Enter.OnInteract += delegate () { PressEnter(); return false; };
		SpaceBar.OnInteract += delegate () { PressSpaceBar(); return false; };
		Border.OnInteract += delegate () { PressBorder(); return false; };
	}
	
	
	void Start()
	{
		this.GetComponent<KMSelectable>().UpdateChildren();
		Module.OnActivate += Introduction;
	}
	
	void Introduction()
	{
		StartCoroutine(Reintroduction());
	}
	
	IEnumerator Reintroduction()
	{
		Startup = true;
		Debug.LogFormat("[Identifying Soulless #{0}] Unless you are using this log to report a bug or to check after you defused/exploded, you must now FC Soulless 4.", moduleId);
        SoullessMusic.clip = NotBuffer[0];
		SoullessMusic.Play();
        while (SoullessMusic.isPlaying)
		{
			yield return new WaitForSecondsRealtime(0.001f);
		}
		Playable = true;
		Startup = false;
	}
	
	void TypableKey(int KeyPress)
	{
		TypableText[KeyPress].AddInteractionPunch(.2f);
        SoullessMusic.clip = NotBuffer[1];
        SoullessMusic.Play();
		if (Playable && Enterable)
		{
			float width = 0;
			foreach (char symbol in TextBox.text)
			{
				CharacterInfo info;
				if (TextBox.font.GetCharacterInfo(symbol, out info, TextBox.fontSize, TextBox.fontStyle))
				{
					width += info.advance;
				}
			}
			width =  width * TextBox.characterSize * 0.1f;
			
			if (width < 0.28f)
			{
				TextBox.text += Text[KeyPress].text;
				if (width > 0.28)
				{
					string Copper = TextBox.text;
					Copper = Copper.Remove(Copper.Length - 1);
					TextBox.text = Copper;
				}
			}
		}
	}
	
	void PressBackspace()
	{
		Backspace.AddInteractionPunch(.2f);
        SoullessMusic.clip = NotBuffer[1];
        SoullessMusic.Play();
        if (Playable)
		{
			if (TextBox.text.Length != 0)
			{
				string Copper = TextBox.text;
				Copper = Copper.Remove(Copper.Length - 1);
				TextBox.text = Copper;
			}
		}
	}
	
	void PressSpaceBar()
	{
		SpaceBar.AddInteractionPunch(.2f);
        SoullessMusic.clip = NotBuffer[1];
        SoullessMusic.Play();
        if (Playable && Enterable)
		{
			float width = 0;
			foreach (char symbol in TextBox.text)
			{
				CharacterInfo info;
				if (TextBox.font.GetCharacterInfo(symbol, out info, TextBox.fontSize, TextBox.fontStyle))
				{
					width += info.advance;
				}
			}
			width =  width * TextBox.characterSize * 0.1f;
			
			if (width < 0.28f)
			{
				TextBox.text += " ";
				if (width > 0.28)
				{
					string Copper = TextBox.text;
					Copper = Copper.Remove(Copper.Length - 1);
					TextBox.text = Copper;
				}
			}
		}
	}
	
	void PressBorder()
	{
		Border.AddInteractionPunch(.2f);
		if (Playable && Toggleable)
		{
			StartCoroutine(PlayTheQueue());
		}
	}
	
	void PressEnter()
	{
		Enter.AddInteractionPunch(.2f);
        SoullessMusic.clip = NotBuffer[1];
        SoullessMusic.Play();
        if (Playable && Enterable)
		{
			StartCoroutine(TheCorrect());
		}
	}
	
	void PressShift(int Shifting)
	{
		ShiftButtons[Shifting].AddInteractionPunch(.2f);
        SoullessMusic.clip = NotBuffer[1];
        SoullessMusic.Play();
        if (Shifted == true)
		{
			Shifted = false;
			StartingNumber = 0;
		}
		
		else
		{
			Shifted = true;
			StartingNumber = 1;
		}
		
		if (Shifted == true)
		{
			for (int b = 0; b < Text.Count(); b++)
			{
				Text[b].text = ChangedText[1][b];
			}
		}
		
		else
		{
			for (int a = 0; a < Text.Count(); a++)
			{
				Text[a].text = ChangedText[0][a];
			}
		}
	}
	
	IEnumerator PlayTheQueue()
	{
		Toggleable = false;
		ActiveBorder = true;
		Playable = false;
        int index = UnityEngine.Random.Range(0, SoullessSections.Length);
        PlayTheSection = SoullessSections[index];
        SoullessMusic.clip = PlayTheSection;
        SoullessMusic.Play();
        Debug.LogFormat("[Identifying Soulless #{0}] The section played: {1}", moduleId, PlayTheSection.name);
		while (SoullessMusic.isPlaying)
        {
            yield return new WaitForSecondsRealtime(0.001f);
        }
		Playable = true;
		ActiveBorder = false;
		Enterable = true;
	}
	
	IEnumerator TheCorrect()
	{
		string Analysis = TextBox.text;
		TextBox.text = "";
		if (Analysis == PlayTheSection.name)
		{
			Stages++;
			Playable = false;
			Enterable = false;
			if (Stages == 3)
			{
				Animating1 = true;
				Debug.LogFormat("[Identifying Soulless #{0}] You submitted {1}, and successfully identified three sections in a row.", moduleId, Analysis);
                if (Bomb.GetTime() < 60)
                {
                    SoullessMusic.clip = NotBuffer[10];
                    SoullessMusic.Play();
                    LightBulbs[2].material = TheLights[1];
                }
                else if (SolveSound == 0)
                {
                    SoullessMusic.clip = NotBuffer[5];
                    SoullessMusic.Play();
                    yield return new WaitForSecondsRealtime(0.05f);
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(1.05f);
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.525f);
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.27125f);
                    LightBulbs[0].material = TheLights[0];
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(1.525f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(1.05f);
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.525f);
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.063125f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.27125f);
                    LightBulbs[0].material = TheLights[0];
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.435f);
                    LightBulbs[0].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.025f);
                    LightBulbs[1].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.025f);
                    LightBulbs[2].material = TheLights[1];
                }
                else
                {
                    SoullessMusic.clip = NotBuffer[3];
                    SoullessMusic.Play();
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[0].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.432f);
                    LightBulbs[0].material = TheLights[0];
                    LightBulbs[2].material = TheLights[0];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[0];
                    LightBulbs[2].material = TheLights[1];
                    yield return new WaitForSecondsRealtime(0.216f);
                    LightBulbs[0].material = TheLights[1];
                    LightBulbs[1].material = TheLights[1];
                    LightBulbs[2].material = TheLights[1];
                }
                    Debug.LogFormat("[Identifying Soulless #{0}] The module has been solved. Great job.", moduleId);
                    Module.HandlePass();
                    Animating1 = false;
			}
			
			else
			{
				Debug.LogFormat("[Identifying Soulless #{0}] You submitted {1}. That matches what was played. Stage passed.", moduleId, Analysis);
				Animating1 = true;
                if (Analysis == "Message To Blue")
                {
                    SoullessMusic.clip = NotBuffer[9];
                    SoullessMusic.Play();
                    LightBulbs[Stages-1].material = TheLights[1];
                    TextBox.text = "Message To Blue:";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += " H";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "I";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "T";
                    yield return new WaitForSecondsRealtime(0.54f);
                    TextBox.text += " M";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "O";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "A";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "R";
                    yield return new WaitForSecondsRealtime(0.54f);
                    TextBox.text += " N";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "O";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "T";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "E";
                    yield return new WaitForSecondsRealtime(0.27f);
                    TextBox.text += "S";
                    yield return new WaitForSecondsRealtime(0.35f);
                    TextBox.text += "!";
                    yield return new WaitForSecondsRealtime(1.55f);
                    TextBox.text = "";
                }
                else if (Analysis == "Break These Chains")
                {
                    SoullessMusic.PlayOneShot(NotBuffer[11]);
                    LightBulbs[Stages - 1].material = TheLights[1];
                    TextBox.text = "Break these chains";
                    yield return new WaitForSeconds(1.125f);
                    TextBox.text = "that take your freedom";
                    yield return new WaitForSeconds(1.875f);
                    TextBox.text = "Liberation";
                    yield return new WaitForSeconds(1.5f);
                    TextBox.text += " from your nation";
                    yield return new WaitForSeconds(1.5f);
                    TextBox.text = "Break these chains";
                    yield return new WaitForSeconds(1.125f);
                    TextBox.text = "and fly to heaven";
                    yield return new WaitForSeconds(1.875f);
                    TextBox.text = "Liberation";
                    yield return new WaitForSeconds(1.5f);
                    TextBox.text += " from your nation";
                    yield return new WaitForSeconds(1.5f);
                    TextBox.text = "";
                }
                else {
                    if (PlayTheSection.name == "Machinations")
                    {
                        SoullessMusic.clip = NotBuffer[6];
                    }
                    else if (PlayTheSection.name == "Caged")
                    {
                        SoullessMusic.clip = NotBuffer[7];
                    }
                    else if (PlayTheSection.name == "Outro")
                    {
                        SoullessMusic.clip = NotBuffer[8];
                    }
                    else
                        SoullessMusic.clip = NotBuffer[2];
                    SoullessMusic.Play();
                    while (SoullessMusic.isPlaying)
                    {
                        LightBulbs[Stages - 1].material = TheLights[1];
                        yield return new WaitForSecondsRealtime(0.075f);
                        LightBulbs[Stages - 1].material = TheLights[0];
                        yield return new WaitForSecondsRealtime(0.075f);
                    }
                    LightBulbs[Stages - 1].material = TheLights[1];
                }
				Playable = true;
				Toggleable = true;
				Animating1 = false;
			}
		}
		
		else
		{
			Debug.LogFormat("[Identifying Soulless #{0}] You submitted {1}. That's sad.", moduleId, Analysis);
			Animating1 = true;
			SoullessMusic.clip = NotBuffer[4];
			SoullessMusic.Play();
			Enterable = false;
			
			LightBulbs[0].material = TheLights[2];
			LightBulbs[1].material = TheLights[0];
			yield return new WaitForSecondsRealtime(0.4f);
			LightBulbs[1].material = TheLights[2];
			yield return new WaitForSecondsRealtime(0.4f);
			LightBulbs[2].material = TheLights[2];
			yield return new WaitForSecondsRealtime(1.0f);
			LightBulbs[0].material = TheLights[0];
			yield return new WaitForSecondsRealtime(0.25f);
			LightBulbs[1].material = TheLights[0];
			yield return new WaitForSecondsRealtime(0.25f);
            LightBulbs[2].material = TheLights[0];
            yield return new WaitForSecondsRealtime(0.8f);
            LightBulbs[0].material = TheLights[2];
            LightBulbs[1].material = TheLights[2];
            LightBulbs[2].material = TheLights[2];
            yield return new WaitForSecondsRealtime(0.3f);
            LightBulbs[0].material = TheLights[0];
            LightBulbs[1].material = TheLights[0];
            LightBulbs[2].material = TheLights[0];
            yield return new WaitForSecondsRealtime(0.3f);
            LightBulbs[0].material = TheLights[2];
            LightBulbs[1].material = TheLights[2];
            LightBulbs[2].material = TheLights[2];
            yield return new WaitForSecondsRealtime(0.3f);
            LightBulbs[0].material = TheLights[0];
            LightBulbs[1].material = TheLights[0];
            LightBulbs[2].material = TheLights[0];
            yield return new WaitForSecondsRealtime(0.3f);
            LightBulbs[0].material = TheLights[2];
            LightBulbs[1].material = TheLights[2];
            LightBulbs[2].material = TheLights[2];
            yield return new WaitForSecondsRealtime(0.3f);
            Debug.LogFormat("[Identifying Soulless #{0}] You did not imagine the nerves. I have given you a strike and a reset for your failure.", moduleId);
			yield return new WaitForSecondsRealtime(1f);
			LightBulbs[0].material = TheLights[0];
			LightBulbs[1].material = TheLights[0];
			LightBulbs[2].material = TheLights[0];
			Playable = true;
			Toggleable = true;
			Animating1 = false;
			Stages = 0;
			Module.HandleStrike();
		}
	}
	
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"To start the section, use !{0} play | To type in the text box, use !{0} type <text> | To submit, use !{0} submit | To clear the text, use !{0} [clear/fastclear] | Skip animations with !{0} skip (Strike animations cannot be skipped)";
    #pragma warning restore 414
	
	int StartingNumber = 0;
	bool Startup = false;
	bool ActiveBorder = false;
	bool Animating1 = false;
	string Current = "";
	
	IEnumerator ProcessTwitchCommand(string command)
	{
		string[] parameters = command.Split(' ');
		if (Regex.IsMatch(parameters[0], @"^\s*type\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Startup == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still playing the section. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The keys are not yet pressable. Command was ignored";
				yield break;
			}
			
			for (int x = 0; x < parameters.Length - 1; x++)
			{
				foreach (char c in parameters[x+1])
				{
					if (!c.ToString().EqualsAny(ChangedText[0]) && !c.ToString().EqualsAny(ChangedText[1]))
					{
						yield return "sendtochaterror The command being submitted contains a character that is not + typable in the given keyboard";
						yield break;
					}
				}
			}
			
			for (int y = 0; y < parameters.Length - 1; y++)
			{
				yield return "trycancel The command to type the text given was halted due to a cancel request";
				foreach (char c in parameters[y+1])
				{
					yield return "trycancel The command to type the text given was halted due to a cancel request";
					Current = TextBox.text;
					if (!c.ToString().EqualsAny(ChangedText[StartingNumber]))
					{
						ShiftButtons[0].OnInteract();
						yield return new WaitForSecondsRealtime(0.05f);
					}
					
					for (int z = 0; z < ChangedText[StartingNumber].Count(); z++)
					{
						if (c.ToString() == ChangedText[StartingNumber][z])
						{
							TypableText[z].OnInteract();
							yield return new WaitForSecondsRealtime(0.05f);
							break;
						}
					}
					
					if (Current == TextBox.text)
					{
						yield return "sendtochaterror The command was stopped due to the text box not able to recieve more characters";
						yield break;
					}
				}

				if (y != parameters.Length - 2)
				{
					SpaceBar.OnInteract();
					yield return new WaitForSecondsRealtime(0.05f);
				}
				
				if (Current == TextBox.text)
				{
					yield return "sendtochaterror The command was stopped due to the text box not able to recieve more characters";
					yield break;
				}
			}
		}
		
		else if (Regex.IsMatch(command, @"^\s*clear\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Startup == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still playing the section. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			
			while (TextBox.text.Length != 0)
			{
				yield return "trycancel The command to clear text in the text box was halted due to a cancel request";
				Backspace.OnInteract();
				yield return new WaitForSecondsRealtime(0.05f);
			}
		}
		
		else if (Regex.IsMatch(command, @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Startup == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still playing the section. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			yield return "solve";
			yield return "strike";
				Enter.OnInteract();
		}
		
		else if (Regex.IsMatch(command, @"^\s*play\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Startup == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored.";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still playing the section. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == true)
			{
				yield return "sendtochaterror You are not able to press the border again. Command was ignored";
				yield break;
			}
			
			Border.OnInteract();
		}

        else if (Regex.IsMatch(command, @"^\s*skip\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;

            if (Startup == true)
            {
                Backspace.OnInteract();
            }
            else if (ActiveBorder == true)
            {
                Backspace.OnInteract();
            }
            else if (Animating1 == true)
            {
                Backspace.OnInteract();
            }
            else
            {
                yield return "sendtochaterror Nothing's skippable currently. Command was ignored.";
                yield break;
            }
        }

        else if (Regex.IsMatch(command, @"^\s*fastclear\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
		{
			yield return null;
			
			if (Startup == true)
			{
				yield return "sendtochaterror The introduction music is still playing. Command was ignored.";
				yield break;
			}
			
			if (ActiveBorder == true)
			{
				yield return "sendtochaterror The module is still playing the section. Command was ignored";
				yield break;
			}
			
			if (Animating1 == true)
			{
				yield return "sendtochaterror The module is performing an animation. Command was ignored";
				yield break;
			}
			
			if (Enterable == false)
			{
				yield return "sendtochaterror The key is not yet pressable. Command was ignored";
				yield break;
			}
			
			while (TextBox.text.Length != 0)
			{
				Backspace.OnInteract();
			}
		}
	}
}
