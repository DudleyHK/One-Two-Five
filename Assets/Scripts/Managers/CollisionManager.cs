using System.Collections;
using System.Collections.Generic;


public class CollisionManager
{
	private List<PoolObject> objectList;
	private bool init = false;

	
	public bool Initialise(ref List<PoolObject> _objects)
	{
		if (init) return false;

		objectList = _objects;

		init = true;
		return true;
	}
	
	
	
	
}
