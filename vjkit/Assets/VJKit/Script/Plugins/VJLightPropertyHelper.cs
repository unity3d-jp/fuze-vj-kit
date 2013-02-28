using UnityEngine;
using System.Collections;

public enum LightPropertyType {
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
	Range,
	SpotAngle,
	Intensity,
	ShadowStrength,
	ShadowBias,
	ShadowSoftness,
	shadowSoftnessFade
}

public class VJLightPropertyHelper : MonoBehaviour {

	public static void UpdateLight(Light obj, LightPropertyType pType, float val ) {
		if( pType == LightPropertyType.Color_R 	||
		pType == LightPropertyType.Color_G 		||
		pType == LightPropertyType.Color_B 		||
		pType == LightPropertyType.Color_RGB 	||
		pType == LightPropertyType.Color_A 		) 
		{
			_UpdateColor(obj, pType, val);
		}
		else if( pType == LightPropertyType.Color_R_Relative 	||
		pType == LightPropertyType.Color_G_Relative 		||
		pType == LightPropertyType.Color_B_Relative 		||
		pType == LightPropertyType.Color_RGB_Relative 	||
		pType == LightPropertyType.Color_A_Relative 		) 
		{
			_UpdateColorRelative(obj, pType, val);
		}
		else {
			_UpdateLightProperty(obj, pType, val);
		}
	}

	private static void _UpdateColor(Light obj, LightPropertyType pType, float val) {
		
		Color c = obj.color;
		
		switch(pType) {
		case LightPropertyType.Color_R:
			c.r = val;
		break;
		case LightPropertyType.Color_G:
			c.g = val;
		break;
		case LightPropertyType.Color_B:
			c.b = val;
		break;
		case LightPropertyType.Color_RGB:
			c.r = val;
			c.g = val;
			c.b = val;
		break;
		case LightPropertyType.Color_A:
			c.a = val;
		break;
		}
		obj.color = c;
	}

	private static void _UpdateColorRelative(Light obj, LightPropertyType pType, float val) {

		Color c = obj.color;
		
		switch(pType) {
		case LightPropertyType.Color_R_Relative:
			c.r = Mathf.Clamp01(c.r + val);
		break;
		case LightPropertyType.Color_G_Relative:
			c.g = Mathf.Clamp01(c.g + val);
		break;
		case LightPropertyType.Color_B_Relative:
			c.b = Mathf.Clamp01(c.b + val);
		break;
		case LightPropertyType.Color_RGB_Relative:
			c.r = Mathf.Clamp01(c.r + val);
			c.g = Mathf.Clamp01(c.g + val);
			c.b = Mathf.Clamp01(c.b + val);
		break;
		case LightPropertyType.Color_A_Relative:
			c.a = Mathf.Clamp01(c.a + val);
		break;
		}
		obj.color = c;
	}


	private static void _UpdateLightProperty(Light obj, LightPropertyType pType, float val) {
		
		switch(pType) {
		case LightPropertyType.Range:
			obj.range = val;
		break;
		case LightPropertyType.SpotAngle:
			obj.spotAngle = val;
		break;
		case LightPropertyType.Intensity:
			obj.intensity = val;
		break;
		case LightPropertyType.ShadowStrength:
			obj.shadowStrength = val;
		break;
		case LightPropertyType.ShadowBias:
			obj.shadowBias = val;
		break;
		case LightPropertyType.ShadowSoftness:
			obj.shadowSoftness = val;
		break;
		case LightPropertyType.shadowSoftnessFade:
			obj.shadowSoftnessFade = val;
		break;
		}
	}
}
