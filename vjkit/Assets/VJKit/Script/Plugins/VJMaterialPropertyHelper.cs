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
}
