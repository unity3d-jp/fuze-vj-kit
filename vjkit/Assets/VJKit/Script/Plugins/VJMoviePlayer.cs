using UnityEngine;
using System.Collections;

/// <summary>
/// VJ movie player.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class VJMoviePlayer : MonoBehaviour {
	
	
	public MovieTexture movie;
	
	
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = (Texture)movie;
		movie.loop = true;
		
		
		movie.Play();
	}
}
