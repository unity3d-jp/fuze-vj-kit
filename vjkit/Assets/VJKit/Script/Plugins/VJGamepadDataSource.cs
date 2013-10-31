/* 
 * fuZe vjkit
 * 
 * Copyright (C) 2013 Unity Technologies Japan, G.K.
 * 
 * Permission is hereby granted, free of charge, to any 
 * person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, subject 
 * to the following conditions: The above copyright notice and 
 * this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Special Thanks:
 * The original software design and architecture of fuZe vjkit 
 * is inspired by Visualizer Studio, created by Altered Reality 
 * Entertainment LLC.
 */
using UnityEngine;
using System.Collections;

public enum VJGamepadButton {
	Button_A,
	Button_B,
	Button_X,
	Button_Y,
	Button_Back,
	Button_Start,
	Trigger_L,
	Trigger_R,
	Shoulder_L,
	Shoulder_R,
	Stick_L_Vertical,
	Stick_L_Horizontal,
	Stick_L_Click,
	Stick_R_Vertical,
	Stick_R_Horizontal,
	Stick_R_Click,
	Digipad_Up,
	Digipad_Down,
	Digipad_Left,
	Digipad_Right,
	Button_Xbox,
}

public enum VJGamePadButtonType {
	Type_Digital,
	Type_Analog,
}

public static class VJGamePadButtonUtility {

	static VJGamePadButtonType[] s_types;
	static KeyCode[] s_keys;
	static string[] s_semantics;
	static string[] s_buttonNames;

	public static VJGamePadButtonType GetButtonTypeOf(VJGamepadButton b) {

		if(s_types == null) {
			s_types = new VJGamePadButtonType[] {
				VJGamePadButtonType.Type_Digital, //Button_A,
				VJGamePadButtonType.Type_Digital, //Button_B,
				VJGamePadButtonType.Type_Digital, //Button_X,
				VJGamePadButtonType.Type_Digital, //Button_Y,
				VJGamePadButtonType.Type_Digital, //Button_Back,
				VJGamePadButtonType.Type_Digital, //Button_Start,
				VJGamePadButtonType.Type_Analog,//Trigger_L,
				VJGamePadButtonType.Type_Analog,//Trigger_R,
				VJGamePadButtonType.Type_Digital, //Shoulder_L,
				VJGamePadButtonType.Type_Digital, //Shoulder_R,
				VJGamePadButtonType.Type_Analog,//Stick_L_Vertical,
				VJGamePadButtonType.Type_Analog,//Stick_L_Horizontal,
				VJGamePadButtonType.Type_Digital, //Stick_L_Click,
				VJGamePadButtonType.Type_Analog,//Stick_R_Vertical,
				VJGamePadButtonType.Type_Analog,//Stick_R_Horizontal,
				VJGamePadButtonType.Type_Digital, //Stick_R_Click,
				VJGamePadButtonType.Type_Digital, //Digipad_Up,
				VJGamePadButtonType.Type_Digital, //Digipad_Down,
				VJGamePadButtonType.Type_Digital, //Digipad_Left,
				VJGamePadButtonType.Type_Digital, //Digipad_Right,				
				VJGamePadButtonType.Type_Digital  //Button_Xbox,				
			};
		}		
		return s_types[(int)b];
	}

	public static KeyCode GetKeyCodeOf(VJGamepadButton b) {

		if(s_keys == null) {
			s_keys = new KeyCode[] {
				KeyCode.JoystickButton16, //Button_A,
				KeyCode.JoystickButton17, //Button_B,
				KeyCode.JoystickButton18, //Button_X,
				KeyCode.JoystickButton19, //Button_Y,
				KeyCode.JoystickButton10, //Button_Back,
				KeyCode.JoystickButton9, //Button_Start,
				KeyCode.JoystickButton0,//Trigger_L,
				KeyCode.JoystickButton0,//Trigger_R,
				KeyCode.JoystickButton13, //Shoulder_L,
				KeyCode.JoystickButton14, //Shoulder_R,
				KeyCode.JoystickButton0,//Stick_L_Vertical,
				KeyCode.JoystickButton0,//Stick_L_Horizontal,
				KeyCode.JoystickButton11, //Stick_L_Click,
				KeyCode.JoystickButton0,//Stick_R_Vertical,
				KeyCode.JoystickButton0,//Stick_R_Horizontal,
				KeyCode.JoystickButton12, //Stick_R_Click,
				KeyCode.JoystickButton5, //Digipad_Up,
				KeyCode.JoystickButton6, //Digipad_Down,
				KeyCode.JoystickButton7, //Digipad_Left,
				KeyCode.JoystickButton8, //Digipad_Right,				
				KeyCode.JoystickButton15 //Button_Xbox,				
			};
		}		
		return s_keys[(int)b];
	}


	public static string GetSemanticOf(VJGamepadButton b) {

		if(s_semantics == null) {
			s_semantics = new string[] {
				"Fire1", //Button_A,
				"Fire2", //Button_B,
				"Fire3", //Button_X,
				"Jump", //Button_Y,
				"", //Button_Back,
				"", //Button_Start,
				"Left Trigger",//Trigger_L,
				"Right Trigger",//Trigger_R,
				"", //Shoulder_L,
				"", //Shoulder_R,
				"Vertical",//Stick_L_Vertical,
				"Horizontal",//Stick_L_Horizontal,
				"", //Stick_L_Click,
				"Vertical2",//Stick_R_Vertical,
				"Horizontal2",//Stick_R_Horizontal,
				"", //Stick_R_Click,
				"", //Digipad_Up,
				"", //Digipad_Down,
				"", //Digipad_Left,
				"", //Digipad_Right,				
				""  //Button_Xbox,				
			};
		}		
		return s_semantics[(int)b];
	}

	public static string GetButtonNameOf(VJGamepadButton b) {

		if(s_buttonNames == null) {
			s_buttonNames = new string[] {
				"A Button", //Button_A,
				"B Button", //Button_B,
				"X Button", //Button_X,
				"Y Button", //Button_Y,
				"Back Button", //Button_Back,
				"Start Button", //Button_Start,
				"L Trigger",//Trigger_L,
				"R Trigger",//Trigger_R,
				"L Shoulder", //Shoulder_L,
				"R Shoulder", //Shoulder_R,
				"L Vertical",//Stick_L_Vertical,
				"L Horizontal",//Stick_L_Horizontal,
				"L Stick Click", //Stick_L_Click,
				"R Vertical",//Stick_R_Vertical,
				"R Horizontal",//Stick_R_Horizontal,
				"R Stick Click", //Stick_R_Click,
				"Digipad Up", //Digipad_Up,
				"Digipad Down", //Digipad_Down,
				"Digipad Left", //Digipad_Left,
				"Digipad Right",  //Digipad_Right,				
				"Xbox Button"  //Button_Xbox,				
			};
		}		
		return s_buttonNames[(int)b];
	}

}

[AddComponentMenu("VJKit/System/Gamepad source")]
[RequireComponent (typeof (VJGamepadManager))]
public class VJGamepadDataSource : VJAbstractDataSource {

//	private VJGamepadManager m_manager;

	public VJGamepadButton button;
	public float value = 1.0f;

	// Use this for initialization
	public void Awake() {
//		m_manager = GetComponent<VJGamepadManager>();
	}

	// Update is called once per frame
	public void Update () {
	
		float raw_current = 0.0f;
		
		switch( VJGamePadButtonUtility.GetButtonTypeOf(button) ) {
			case VJGamePadButtonType.Type_Analog:
				float analogInput = Input.GetAxis(VJGamePadButtonUtility.GetSemanticOf(button));
				// trigger goes to -1.0f when released after press. 
				if( button == VJGamepadButton.Trigger_L || button == VJGamepadButton.Trigger_R ) {
					analogInput = Mathf.Max(0.0f, analogInput);
				}
				raw_current = value * boost * analogInput;
			break;
			case VJGamePadButtonType.Type_Digital:
				if( Input.GetKey(VJGamePadButtonUtility.GetKeyCodeOf(button)) ) {
//				if( Input.GetButton(VJGamePadButtonUtility.GetSemanticOf(button)) ) {
					raw_current = value * boost;
				}
			break;
		}
				
//		if( Input.GetKey(VJGamePadButtonUtility.GetKeyCodeOf(button)) ) {
//			raw_current = value * boost;
//		}
//		if( Input.GetKey(VJGamePadButtonUtility.GetKeyCodeOf(button)) ) {
//			raw_current = value * boost;
//		}

		previous = current;
		current = raw_current;
		diff = current - previous;
	}
}
