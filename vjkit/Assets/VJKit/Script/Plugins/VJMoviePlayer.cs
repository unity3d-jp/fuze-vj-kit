using UnityEngine;
using System.Collections;

/// <summary>
/// VJ movie player.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class VJMoviePlayer : MonoBehaviour {
	
	
	public MovieTexture movie;
	
	[HideInInspector]
	public AudioSource source;
	
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = (Texture)movie;
		movie.loop = true;
		
		source = gameObject.AddComponent<AudioSource>(); 
		source.clip = movie.audioClip;
		
		movie.Play();
		source.Play();
	}
}
