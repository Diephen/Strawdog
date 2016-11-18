using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

	public static bool[] _acquiredStates = new bool[7] {false, false, false, false, false, false, false};
	// 0 : Killed at ditch
	// 1 : Prisoner Escapes
	// 2 : Execution for Crimes
	// 3 : Stopped Escape
	// 4 : Happy Ending?
	// 5 : Plant Bomb
	// 6 : Final Ending
}
