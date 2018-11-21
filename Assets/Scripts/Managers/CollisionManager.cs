using System.Collections;
using System.Collections.Generic;


public class CollisionManager
{
	private List<Object> objectList;
	private bool init = false;

	
	public bool Initialise(ref List<Object> _objects)
	{
		if (init) return false;

		objectList = _objects;

		init = true;
		return true;
	}
	
	
	
	
}
