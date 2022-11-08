using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameplayUtility
{
	/// <summary>
	/// The ObjectPool is the storage class for pooled objects of the same kind (e.g. "Pistol Bullet", or "Enemy A")
	/// This is used by the ObjectPoolManager and is not meant to be used separately
	/// At any time, an object pool appears in the hierarchy as a object with many children, 
	/// these children are all the inactive objects sitting in the pool, that can be reactivated when requesting a new instance of the object the pool is handling
	/// </summary>
	public class ObjectPool : MonoBehaviour
	{
		// The type of object this pool is handling
		GameObject m_Prefab;

		// This stores the cached objects waiting to be reactivated
		private Queue<GameObject> m_Pool;

		/// <summary>
		/// initializes the pool and the type of object this pool is handling
		/// </summary>
		/// <param name="prefab">the prefab to be handled by this pool</param>
		public void InitializeWith(GameObject prefab)
		{
			m_Pool = new Queue<GameObject>();
			m_Prefab = prefab;
		}

		/// <summary>
		/// Instantiate an object with position and rotation specified, and returns a reference to that object
		/// </summary>
		/// <param name="position">position that the instance will have</param>
		/// <param name="rotation">rotation that the instance will have</param>
		/// <returns>reference to the new instantiated object</returns>
		public GameObject InstantiateWith(Vector3 position, Quaternion rotation)
		{
			//declare an uninitialized object reference
			GameObject obj;

			//if we don't have any object already in the pool, create a new one
			if (m_Pool.Count < 1)
			{
				obj = Object.Instantiate(m_Prefab, position, rotation) as GameObject;
			}
			//otherwise, pull one from the pool
			else
			{
				//pop unused object from the pool
				obj = m_Pool.Dequeue();

				//the object will not have any parent (out of any hierarchy)
				obj.transform.parent = null;

				//we set the position and rotation specified
				obj.transform.position = position;
				obj.transform.rotation = rotation;

				//then we activate the object (unity will call the OnEnable function on any scripts the object may have)
				obj.SetActive(true);
			}

			//return the object reference
			return obj;
		}

		/// <summary>
		/// put the object in the pool and deactivate it
		/// </summary>
		/// <param name="obj">reference of the instance we want to recycle</param>
		public void Recycle(GameObject obj)
		{
			// deactivate the object
			obj.SetActive(false);

			// put the recycled object in this ObjectPool's bucket
			obj.transform.SetParent(transform);

			// put object back in pool for reuse later
			m_Pool.Enqueue(obj);
		}

	}
}