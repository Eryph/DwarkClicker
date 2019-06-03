using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialbox", menuName = "Data/FTUE/Dialbox")]
public class DialboxData : ScriptableObject {
	// Key to extract from DatabaseManager
	[SerializeField] private string _key = "DIALBOX_KEY";
	// Key to extract from FTUEManager
	[SerializeField] private int _step = 0;
	// Text to print into Dialbox
	[SerializeField] [TextArea(10, 10)] private string _text = "Dialbox Text.";

	public string Key { get { return _key; } } 
	public int Step { get { return _step; } }
	public string Text { get { return _text; } }
}
