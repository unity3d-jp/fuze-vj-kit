using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/GameObject Property")]
public class VJGameObjectTrigger : VJBaseTrigger {

	public GameObjectPropertyType propertyToModify;

	public override void OnVJTrigger() {		
		VJGameObjectPropertyHelper.UpdateGameObject(gameObject, propertyToModify, GetValue() );
	}
}
