using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Movie Player")]
[RequireComponent(typeof(Renderer))]
public class VJMoviePlayer : MonoBehaviour {
	
	public MovieTexture movie;
	
	void Start () {
		renderer.material.mainTexture = (Texture)movie;
		movie.loop = true;
		
		movie.Play();
	}
}
