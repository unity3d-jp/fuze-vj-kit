using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/GameObject Property")]
public class VJGameObjectModifier : VJBaseModifier {

	public GameObjectPropertyType propertyToModify;

	void Update () {
		VJGameObjectPropertyHelper.UpdateGameObject(gameObject, propertyToModify, GetValue() );
	}
}
