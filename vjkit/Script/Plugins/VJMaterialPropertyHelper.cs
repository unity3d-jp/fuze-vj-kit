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

public enum MaterialPropertyType {
	Color_R,
	Color_G,
	Color_B,
	Color_RGB,
	Color_A,
	Color_R_Relative,
	Color_G_Relative,
	Color_B_Relative,
	Color_RGB_Relative,
	Color_A_Relative,
	TextureOffset_X,
	TextureOffset_Y,
	TextureScale_X,
	TextureScale_Y,
	Float,
}

public class VJMaterialPropertyHelper : MonoBehaviour {

	public static void UpdateMaterial(Material obj, MaterialPropertyType pType, float val, string paramName ) {
		if( pType == MaterialPropertyType.Color_R 	||
		pType == MaterialPropertyType.Color_G 		||
		pType == MaterialPropertyType.Color_B 		||
		pType == MaterialPropertyType.Color_RGB 	||
		pType == MaterialPropertyType.Color_A 		) 
		{
			_UpdateColor(obj, pType, val, paramName);
		}
		else if( pType == MaterialPropertyType.Color_R_Relative 	||
		pType == MaterialPropertyType.Color_G_Relative 		||
		pType == MaterialPropertyType.Color_B_Relative 		||
		pType == MaterialPropertyType.Color_RGB_Relative 	||
		pType == MaterialPropertyType.Color_A_Relative 		) 
		{
			_UpdateColorRelative(obj, pType, val, paramName);
		}
		else {
			_UpdateMaterialProperty(obj, pType, val, paramName);
		}
	}

	private static void _UpdateColor(Material obj, MaterialPropertyType pType, float val, string paramName) {
		
		Color c;
		
		if(paramName == null || paramName.Length == 0) {
			c = obj.color;
		} else {
			c = obj.GetColor(paramName);
		}
		
		switch(pType) {
		case MaterialPropertyType.Color_R:
			c.r = val;
		break;
		case MaterialPropertyType.Color_G:
			c.g = val;
		break;
		case MaterialPropertyType.Color_B:
			c.b = val;
		break;
		case MaterialPropertyType.Color_RGB:
			c.r = val;
			c.g = val;
			c.b = val;
		break;
		case MaterialPropertyType.Color_A:
			c.a = val;
		break;
		}

		if(paramName == null || paramName.Length == 0) {
			obj.color = c;
		} else {
			obj.SetColor(paramName, c);
		}
	}

	private static void _UpdateColorRelative(Material obj, MaterialPropertyType pType, float val, string paramName) {

		Color c;
		
		if(paramName == null || paramName.Length == 0) {
			c = obj.color;
		} else {
			c = obj.GetColor(paramName);
		}
		
		switch(pType) {
		case MaterialPropertyType.Color_R_Relative:
			c.r = Mathf.Clamp01(c.r + val);
		break;
		case MaterialPropertyType.Color_G_Relative:
			c.g = Mathf.Clamp01(c.g + val);
		break;
		case MaterialPropertyType.Color_B_Relative:
			c.b = Mathf.Clamp01(c.b + val);
		break;
		case MaterialPropertyType.Color_RGB_Relative:
			c.r = Mathf.Clamp01(c.r + val);
			c.g = Mathf.Clamp01(c.g + val);
			c.b = Mathf.Clamp01(c.b + val);
		break;
		case MaterialPropertyType.Color_A_Relative:
			c.a = Mathf.Clamp01(c.a + val);
		break;
		}

		if(paramName == null || paramName.Length == 0) {
			obj.color = c;
		} else {
			obj.SetColor(paramName, c);
		}
	}


	private static void _UpdateMaterialProperty(Material obj, MaterialPropertyType pType, float val, string paramName) {
		
		Vector2 offs;
		
		switch(pType) {
		case MaterialPropertyType.TextureOffset_X:
			offs = obj.mainTextureOffset;
			offs.x = val;
			obj.mainTextureOffset = offs;
		break;
		case MaterialPropertyType.TextureOffset_Y:
			offs = obj.mainTextureOffset;
			offs.y = val;
			obj.mainTextureOffset = offs;
		break;
		case MaterialPropertyType.TextureScale_X:
			offs = obj.mainTextureScale;
			offs.x = val;
			obj.mainTextureScale = offs;
		break;
		case MaterialPropertyType.TextureScale_Y:
			offs = obj.mainTextureScale;
			offs.y = val;
			obj.mainTextureScale = offs;
		break;
		case MaterialPropertyType.Float:
			if(paramName != null && paramName.Length > 0) {
				obj.SetFloat(paramName, val);
			}
		break;
		}
	}

	public static float GetMaterialValue(Material obj, MaterialPropertyType pType, string paramName ) {
		if( pType == MaterialPropertyType.Color_R 	||
		pType == MaterialPropertyType.Color_G 		||
		pType == MaterialPropertyType.Color_B 		||
		pType == MaterialPropertyType.Color_RGB 	||
		pType == MaterialPropertyType.Color_A 		||
		pType == MaterialPropertyType.Color_G_Relative 		||
		pType == MaterialPropertyType.Color_B_Relative 		||
		pType == MaterialPropertyType.Color_RGB_Relative 	||
		pType == MaterialPropertyType.Color_A_Relative 		) 
		{
			return _GetColor(obj, pType, paramName);
		}
		else {
			return _GetMaterialProperty(obj, pType, paramName);
		}
	}

	private static float _GetColor(Material obj, MaterialPropertyType pType, string paramName) {
		
		Color c;
		
		if(paramName == null || paramName.Length == 0) {
			c = obj.color;
		} else {
			c = obj.GetColor(paramName);
		}
		
		switch(pType) {
		case MaterialPropertyType.Color_R:
			return c.r;
		case MaterialPropertyType.Color_G:
			return c.g;
		case MaterialPropertyType.Color_B:
			return c.b;
		case MaterialPropertyType.Color_RGB:
			return c.r;
		case MaterialPropertyType.Color_A:
			return c.a;
		case MaterialPropertyType.Color_R_Relative:
			return c.r;
		case MaterialPropertyType.Color_G_Relative:
			return c.g;
		case MaterialPropertyType.Color_B_Relative:
			return c.b;
		case MaterialPropertyType.Color_RGB_Relative:
			return c.r;
		case MaterialPropertyType.Color_A_Relative:
			return c.a;
		}
		
		return 0.0f;
	}

	private static float _GetMaterialProperty(Material obj, MaterialPropertyType pType, string paramName) {
		
		Vector2 offs;
		
		switch(pType) {
		case MaterialPropertyType.TextureOffset_X:
			offs = obj.mainTextureOffset;
			return offs.x;
		case MaterialPropertyType.TextureOffset_Y:
			offs = obj.mainTextureOffset;
			return offs.y;
		case MaterialPropertyType.TextureScale_X:
			offs = obj.mainTextureScale;
			return offs.x;
		case MaterialPropertyType.TextureScale_Y:
			offs = obj.mainTextureScale;
			return offs.y;
		case MaterialPropertyType.Float:
			if(paramName != null && paramName.Length > 0) {
				return obj.GetFloat(paramName);
			}
			break;
		}
		
		return 0.0f;
	}
	

}
