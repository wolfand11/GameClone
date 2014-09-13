using UnityEngine;
using System.Collections;

public class ZombieAnimator : MonoBehaviour {

	public Sprite[] frames;
	public float frames_per_second;
	private SpriteRenderer sprite_renderer;
	private float delta_time_;
	private int	  frame_index = 0;
	// Use this for initialization
	void Start () {
		frames_per_second = Mathf.Abs(frames_per_second);
		delta_time_ = 0.0f;
		sprite_renderer = renderer as SpriteRenderer;
	}
	
	// Update is called once per frame
	void Update () {
		delta_time_ += Time.deltaTime;
		if ( delta_time_ > frames_per_second ) 
		{
			delta_time_ -= frames_per_second;
			frame_index++;
			frame_index %= frames.Length;
			sprite_renderer.sprite = frames[frame_index];
		}
	}
}
