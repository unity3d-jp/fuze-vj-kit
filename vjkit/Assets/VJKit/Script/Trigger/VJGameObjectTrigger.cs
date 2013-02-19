using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/GameObject Property")]
public class VJGameObjectTrigger : VJBaseTrigger {

	public GameObjectPropertyType propertyToModify;

	public override void OnVJTrigger(GameObject go, float value) {		
		VJGameObjectPropertyHelper.UpdateGameObject(go, propertyToModify, value );
	}
}
