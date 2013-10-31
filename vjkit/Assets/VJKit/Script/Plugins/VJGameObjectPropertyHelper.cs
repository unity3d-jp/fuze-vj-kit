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

public enum GameObjectPropertyType {
	Position_X,
	Position_Y,
	Position_Z,
	LocalPosition_X,
	LocalPosition_Y,
	LocalPosition_Z,
	Rotation_X_Euler,
	Rotation_Y_Euler,
	Rotation_Z_Euler,
	LocalRotation_X_Euler,
	LocalRotation_Y_Euler,
	LocalRotation_Z_Euler,
	Scale_X,
	Scale_Y,
	Scale_Z,
	Uniform_Scale,
	Velocity_X,
	Velocity_Y,
	Velocity_Z,
	AngularVelocity_X_Euler,
	AngularVelocity_Y_Euler,
	AngularVelocity_Z_Euler
}

public class VJGameObjectPropertyHelper : MonoBehaviour {

	public static void UpdateGameObject(GameObject obj, GameObjectPropertyType pType, float val ) {
		if( pType == GameObjectPropertyType.Position_X 	||
		pType == GameObjectPropertyType.Position_Y 		||
		pType == GameObjectPropertyType.Position_Z 		) 
		{
			_UpdatePosition(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.LocalPosition_X ||
		pType == GameObjectPropertyType.LocalPosition_Y 		 ||
		pType == GameObjectPropertyType.LocalPosition_Z 		 ) 
		{
			_UpdateLocalPosition(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.Rotation_X_Euler 	||
		pType == GameObjectPropertyType.Rotation_Y_Euler 			||
		pType == GameObjectPropertyType.Rotation_Z_Euler 			) 
		{
			_UpdateRotation(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.LocalRotation_X_Euler 	||
		pType == GameObjectPropertyType.LocalRotation_Y_Euler 			||
		pType == GameObjectPropertyType.LocalRotation_Z_Euler 			) 
		{
			_UpdateLocalRotation(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.Scale_X 	||
		pType == GameObjectPropertyType.Scale_Y 			||
		pType == GameObjectPropertyType.Scale_Z 			||
		pType == GameObjectPropertyType.Uniform_Scale 		) 
		{
			_UpdateScale(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.Velocity_X ||
		pType == GameObjectPropertyType.Velocity_Y 			||
		pType == GameObjectPropertyType.Velocity_Z 			) 
		{
			_UpdateVelocity(obj, pType, val);
		}
		else if( pType == GameObjectPropertyType.AngularVelocity_X_Euler ||
		pType == GameObjectPropertyType.AngularVelocity_Y_Euler 		 ||
		pType == GameObjectPropertyType.AngularVelocity_Z_Euler 		 ) 
		{
			_UpdateAngularVelocity(obj, pType, val);
		}
	}

	private static void _UpdatePosition(GameObject obj, GameObjectPropertyType pType, float val) {
		Vector3 pos = obj.transform.position;
		
		switch(pType) {
		case GameObjectPropertyType.Position_X:
			pos.x = val;
		break;
		case GameObjectPropertyType.Position_Y:
			pos.y = val;
		break;
		case GameObjectPropertyType.Position_Z:
			pos.z = val;
		break;
		}
		obj.transform.position = pos;
	}

	private static void _UpdateLocalPosition(GameObject obj, GameObjectPropertyType pType, float val) {
		Vector3 pos = obj.transform.localPosition;
		
		switch(pType) {
		case GameObjectPropertyType.LocalPosition_X:
			pos.x = val;
		break;
		case GameObjectPropertyType.LocalPosition_Y:
			pos.y = val;
		break;
		case GameObjectPropertyType.LocalPosition_Z:
			pos.z = val;
		break;
		}
		obj.transform.localPosition = pos;
	}

	private static void _UpdateRotation(GameObject obj, GameObjectPropertyType pType, float val) {
			
		Vector3 rot = obj.transform.rotation.eulerAngles;
		
		switch(pType) {
		case GameObjectPropertyType.Rotation_X_Euler:
			rot.x = val;
		break;
		case GameObjectPropertyType.Rotation_Y_Euler:
			rot.y = val;
		break;
		case GameObjectPropertyType.Rotation_Z_Euler:
			rot.z = val;
		break;
		}
		obj.transform.rotation = Quaternion.Euler(rot);
	}

	private static void _UpdateLocalRotation(GameObject obj, GameObjectPropertyType pType, float val) {
			
		Vector3 rot = obj.transform.localRotation.eulerAngles;
		
		switch(pType) {
		case GameObjectPropertyType.LocalRotation_X_Euler:
			rot.x = val;
		break;
		case GameObjectPropertyType.LocalRotation_Y_Euler:
			rot.y = val;
		break;
		case GameObjectPropertyType.LocalRotation_Z_Euler:
			rot.z = val;
		break;
		}
		obj.transform.localRotation = Quaternion.Euler(rot);
	}


	private static void _UpdateScale(GameObject obj, GameObjectPropertyType pType, float val) {

		Vector3 scl = obj.transform.localScale;
		
		switch(pType) {
		case GameObjectPropertyType.Scale_X:
			scl.x = val;
		break;
		case GameObjectPropertyType.Scale_Y:
			scl.y = val;
		break;
		case GameObjectPropertyType.Scale_Z:
			scl.z = val;
		break;
		case GameObjectPropertyType.Uniform_Scale:
			scl.x = val;
			scl.y = val;
			scl.z = val;
		break;
		}
		obj.transform.localScale = scl;
	}

	private static void _UpdateVelocity(GameObject obj, GameObjectPropertyType pType, float val) {
		Vector3 pos = obj.transform.localPosition;
		
		switch(pType) {
		case GameObjectPropertyType.Velocity_X:
			pos.x += val;
		break;
		case GameObjectPropertyType.Velocity_Y:
			pos.y += val;
		break;
		case GameObjectPropertyType.Velocity_Z:
			pos.z += val;
		break;
		}
		obj.transform.localPosition = pos;
	}

	private static void _UpdateAngularVelocity(GameObject obj, GameObjectPropertyType pType, float val) {
			
		Vector3 rot = obj.transform.localRotation.eulerAngles;
		
		switch(pType) {
		case GameObjectPropertyType.AngularVelocity_X_Euler:
			rot.x += val;
		break;
		case GameObjectPropertyType.AngularVelocity_Y_Euler:
			rot.y += val;
		break;
		case GameObjectPropertyType.AngularVelocity_Z_Euler:
			rot.z += val;
		break;
		}
		obj.transform.localRotation = Quaternion.Euler(rot);
	}
}
