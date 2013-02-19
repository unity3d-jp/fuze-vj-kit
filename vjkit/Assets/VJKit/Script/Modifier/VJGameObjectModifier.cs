using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/GameObject Property")]
public class VJGameObjectModifier : VJBaseModifier {

	public GameObjectPropertyType propertyToModify;

	public override void VJPerformAction(GameObject go, float value) {
		VJGameObjectPropertyHelper.UpdateGameObject(go, propertyToModify, value );
	}
}
