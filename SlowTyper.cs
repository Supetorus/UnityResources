// Script written by Alan Buechner github.com/AlanBuechner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SlowTyper : MonoBehaviour
{

	#region Commands

	[System.Serializable]
	public class Command
	{ }

	// type command
	[System.Serializable]
	public class TypeCommand : Command
	{
		public TypeCommand(string t)
		{ text = t; }
		public string text;
	}

	// speed command
	[System.Serializable]
	public class SpeedCommand : Command
	{
		public SpeedCommand(float s)
		{ speed = s; }
		public float speed;
	}

	// sleep command
	[System.Serializable]
	public class SleepCommand : Command
	{
		public SleepCommand(float t)
		{ time = t; }
		public float time;
	}

	// clear command
	[System.Serializable]
	public class ClearCommand : Command
	{ }

	// back command
	[System.Serializable]
	public class BackCommand : Command
	{
		public BackCommand(int c)
		{ count = c; }
		public int count;
	}

	#endregion

	private List<Command> m_Commands;

	private TextMeshProUGUI m_TextMesh;

	[SerializeField]
	private float m_Speed = 1 / 0.05f;

	[SerializeField]
	private float m_InitalTimeOffset = 0.0f;

	[SerializeField, TextArea]
	private string m_Text;

	private int m_Position;

	[SerializeField]
	private bool m_FinishTypingOnClick = false;

	[SerializeField]
	private bool m_OnEnable = false;

	[Header("Events")]
	[SerializeField]
	private UnityEvent m_LoadEvent;
	[SerializeField]
	private UnityEvent m_StartEvent;
	[SerializeField]
	private UnityEvent m_TypeEvent;
	[SerializeField]
	private UnityEvent m_StopEvent;

	private Coroutine m_Typing;

	private string m_CurrentText;

	private bool m_FinishTyping = false;

	private bool m_DoneTyping = false;

	private void OnEnable()
	{
		if (m_OnEnable)
			m_Typing = StartCoroutine(StartTyping());
	}

	private void Start()
	{
		Parse();
		if (!m_OnEnable)
			m_Typing = StartCoroutine(StartTyping());
	}

	private void OnDisable()
	{
		if(m_OnEnable)
		{
			StopCoroutine(m_Typing);
		}
	}

	private void Update()
	{
		if(!m_DoneTyping && m_FinishTypingOnClick && Input.anyKeyDown)
		{
			m_FinishTyping = true;
			StopCoroutine(m_Typing);
			m_Typing = StartCoroutine(StartTyping());
		}
	}

	private void Parse()
	{
		m_Commands = new List<Command>();
		
		while(true)
		{
			string token = GetNextToken();
			if (token == "")
				break;

			if (CleanToken(token) == "<")
			{
				token = GetNextTokenClean();
				if (token.ToLower() == "speed")
				{
					token = GetNextTokenClean();
					if (token != "=")
						Debug.LogError("invalid token " + token + "expecting \"=\"");

					token = GetNextTokenClean();

					float newSpeed = float.Parse(token);
					m_Commands.Add(new SpeedCommand(newSpeed));
				}
				else if (token.ToLower() == "clear")
				{
					m_Commands.Add(new ClearCommand());
				}
				else if(token.ToLower() == "sleep")
				{
					token = GetNextTokenClean();
					if (token != "=")
						Debug.LogError("invalid token " + token + "expecting \"=\"");

					token = GetNextTokenClean();

					float sleepTime = float.Parse(token);
					m_Commands.Add(new SleepCommand(sleepTime));
				}
				else if (token.ToLower() == "back")
				{
					token = GetNextTokenClean();
					if (token != "=")
						Debug.LogError("invalid token " + token + "expecting \"=\"");

					token = GetNextTokenClean();

					int backCount = int.Parse(token);
					m_Commands.Add(new BackCommand(backCount));
				}
				GetNextToken();
			}
			else if(token == "\"")
			{
				token = GetNextToken();
				m_Commands.Add(new TypeCommand(token));
				GetNextToken();
			}
		}
	}

	private string CleanToken(string token)
	{
		int position = m_Position;

		// remove spaces and ne lines at the front of the string
		for (int i = position; i < m_Text.Length; i++)
		{
			if (m_Text[i] == ' ' || m_Text[i] == '\n')
				position++;
			else
				break;
		}

		// remove spaces and new lines at the end of the token
		int back;
		for (back = 0; back < token.Length; back++)
		{
			int index = token.Length - 1 - back;
			if (token[index] != ' ' && token[index] != '\n')
				break;
		}
		if (back != 0)
			token = token.Remove(token.Length - back);

		return token;
	}

	private string GetNextTokenClean()
	{

		if (m_Position >= m_Text.Length)
			return "";

		string token = GetNextToken();

		return CleanToken(token);
	}

	private string GetNextToken()
	{
		if (m_Position >= m_Text.Length)
			return "";

		string token = "";

		char lastChar = ' ';

		char[] tokens = new char[] { '<', '>', '=', '"' };

		for(int i = 0; i < tokens.Length; i++)
		{
			if (m_Text[m_Position] == tokens[i])
			{
				token += m_Text[m_Position];
				m_Position++;
				return token;
			}
		}

		for(int i = m_Position; i < m_Text.Length; i++)
		{
			char current = m_Text[i];

			if (lastChar != '\\')
			{
				bool found = false;
				for (int j = 0; j < tokens.Length; j++)
				{
					if (current == tokens[j])
					{
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			token += current;
			m_Position++;


			for (int j = 0; j < tokens.Length; j++)
			{
				if (current == tokens[j])
					return token;
			}

			lastChar = current;
		}

		return token;
	}

	private IEnumerator StartTyping()
	{
		m_DoneTyping = false;

		m_TextMesh = GetComponent<TextMeshProUGUI>();

		m_CurrentText = "";
		m_TextMesh.SetText(m_CurrentText);

		m_LoadEvent.Invoke();

		if(!m_FinishTyping)
			yield return new WaitForSeconds(m_InitalTimeOffset);

		m_StartEvent.Invoke();

		m_Typing = StartCoroutine(Type());

		yield return null;
	}

	private IEnumerator Type() 
	{
		for(int i = 0; i < m_Commands.Count; i++)
		{
			int counter = 0;

			while (true)
			{

				if (m_Commands[i] is TypeCommand tc)
				{
					if (counter < tc.text.Length)
					{
						m_TypeEvent.Invoke();
						PlayTypingSound();
						m_CurrentText += tc.text[counter];
						m_TextMesh.SetText(m_CurrentText);
					}
					else
						break;
				}
				else if(m_Commands[i] is SpeedCommand sc)
				{
					m_Speed = sc.speed;
					break;
				}
				else if(m_Commands[i] is ClearCommand cc)
				{
					m_CurrentText = "";
					m_TextMesh.SetText(m_CurrentText);
					break;
				}
				else if(m_Commands[i] is SleepCommand spc)
				{
					if(!m_FinishTyping)
						yield return new WaitForSeconds(spc.time);
					break;
				}
				else if(m_Commands[i] is BackCommand bc)
				{

					if (counter < bc.count)
					{
						m_TypeEvent.Invoke();
						PlayTypingSound();
						m_CurrentText = m_CurrentText.Remove(m_CurrentText.Length-1);
						m_TextMesh.SetText(m_CurrentText);
					}
					else
						break;
				}

				counter++;

				if (!m_FinishTyping)
					yield return new WaitForSeconds(1/m_Speed);

			}
		}

		m_DoneTyping = true;
		m_FinishTyping = false;
		m_StopEvent.Invoke();
		yield return null;
	}

	void PlayTypingSound()
	{
		AudioManager.Get()?.Play("Typing");
	}
}
