using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoScrollLayer : MonoBehaviour {

	public float min_boundary;
	public float max_boundary;
	public float fixed_pos;
	public float speed;
	public bool  enable_auto_scroll = false;
	public GameObject template_node;

	public enum Direction
	{
		kVertical,
		kHorizontal
	};

	public Direction direction;

	private Vector3 original_local_pos  = Vector3.zero;
	private float 	instance_size = 0.0f;

	private float 	total_offset = 0.0f;
	private float 	shift;
	private float 	offset = 0.0f;
	private Vector3 offset_vector = Vector3.zero;
	private List<GameObject> _sprites = new List<GameObject>();
	// Use this for initialization
	void Start () {
		original_local_pos = transform.localPosition;

		if(!template_node) return;

		SpriteRenderer sprite = template_node.GetComponent<SpriteRenderer>();
		if(!sprite) return;

		switch(direction)
		{
		case Direction.kHorizontal:
		{
			instance_size = sprite.bounds.size.x;
			break;
		}
		case Direction.kVertical:
		{
			instance_size = sprite.bounds.size.y;
			break;
		}
		}
		Debug.Log("instance_size:"+instance_size);
		int need_count = Mathf.CeilToInt(Mathf.Abs(max_boundary - min_boundary)/instance_size);
		need_count += 2;
		int half_count = need_count / 2;
		float start_pos = 0.0f;
		start_pos = -1.0f * instance_size * half_count;
		if(need_count%2==0)
		{
			start_pos += instance_size/2.0f;
		}

		for(int i=0; i<need_count; i++)
		{
			GameObject obj = Instantiate (template_node) as GameObject;
			Vector3 pos = Vector3.zero;
			switch(direction)
			{
			case Direction.kHorizontal:
			{
				pos = new Vector3(start_pos+i*instance_size,
				                  fixed_pos,
				                  obj.transform.position.z);
				break;
			}
			case Direction.kVertical:
			{
				pos = new Vector3(fixed_pos,
		                          start_pos+i*instance_size,
		                          obj.transform.position.z);
				break;
			}
			}
			obj.transform.parent = gameObject.transform;
			obj.transform.localPosition = pos;
			Debug.Log("pos:"+pos.x+" "+pos.y+" "+pos.z);
			_sprites.Add(obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Movement
		shift = speed*Time.deltaTime;
		total_offset += shift;

		offset = Mathf.Abs(total_offset) - instance_size; 
		if(offset>=0.0f)
		{
			transform.localPosition = original_local_pos;
			if(total_offset<0.0f)
			{
				offset = total_offset + instance_size;
			}
			else
			{
				offset = total_offset - instance_size;
			}
			total_offset = offset;
		}
		else
		{
			offset = shift;
		}

		switch(direction)
		{
		case Direction.kHorizontal:
		{
			offset_vector.Set (offset,0.0f,0.0f);
			break;
		}
		case Direction.kVertical:
		{
			offset_vector.Set (0.0f,offset,0.0f);
			break;
		}
		}
		transform.Translate(offset_vector);
	}
}
