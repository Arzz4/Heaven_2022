using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameplayUtility
{
	/// <summary>
	/// When a recycled object is revived from the cache, the ObjectPool will set that object to active and Unity will call the OnEnable() method again, 
	/// so this object can reset itself as if it just got newly created.
	/// For using this system, you should create and delete objects through the interface this system provides. Common errors can happen when:
	///   1. you use unity instantiate and then try to recycle that object through the object pool
	///   2. you use the object pool system to instantiate and then use unity to delete an object
	/// </summary>
	public class ObjectPoolSystem : MonoBehaviour
	{
		//this is the object root that will have as children all different object pools
		private Transform m_PoolSystemRoot;

		//a reference to the trash of this pool system
		private ObjectPoolTrash m_Trash;

		// This maps a prefab to its ObjectPool
		private Dictionary<GameObject, ObjectPool> m_PrefabToPool;

		// This maps a game object instance to the ObjectPool that created/recycled it
		private Dictionary<GameObject, ObjectPool> m_InstanceToPool;

		private static ObjectPoolSystem m_Instance = null;
		public static ObjectPoolSystem Instance { get { return m_Instance; } }

		/// <summary>
		/// called for initializing the object pool system
		/// </summary>
		private void Awake()
		{
			// initialize singleton
			m_Instance = this;

			//initializes the pool system hiearchy 
			m_PoolSystemRoot = transform;

			//initializes the tables to keep track of objects that have been instantiated and pools
			m_PrefabToPool = new Dictionary<GameObject, ObjectPool>();
			m_InstanceToPool = new Dictionary<GameObject, ObjectPool>();

			//initializes the trash
			GameObject trash = new GameObject("TRASH");
			m_Trash = trash.AddComponent<ObjectPoolTrash>();
			m_Trash.transform.parent = m_PoolSystemRoot;
		}

		/// <summary>
		/// Instantiate an object from a prefab, with specified position and rotation.
		/// This will either reuse an object from the pool or allocate a new one using Unity instantiate
		/// </summary>
		/// <param name="prefab">prefab to instantiate</param>
		/// <param name="position">position the instance will have when instantiated</param>
		/// <param name="rotation">rotation the instance will have when instantiated</param>
		/// <returns></returns>
		public GameObject InstantiatePrefabWith(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			//first declare a reference to an object pool 
			//this will reference the pool of the type of object of the input prefab
			ObjectPool pool;

			//if a pool of that prefab type doesnt exists
			if (!m_PrefabToPool.ContainsKey(prefab))
			{
				//create a new pool
				pool = CreatePool(prefab);

				//put the pool on the pool system root hierarchy
				pool.transform.parent = m_PoolSystemRoot;

				//and insert that to our dictionary of prefabs that have been created
				m_PrefabToPool[prefab] = pool;
			}
			//here the pool of that prefab type already existed
			else
			{
				//get the reference to the pool
				pool = m_PrefabToPool[prefab];
			}

			// now get a new object reference from the pool 
			GameObject obj = pool.InstantiateWith(position, rotation);

			//and keep track of which pool this object was created from
			m_InstanceToPool[obj] = pool;

			//return the new object reference
			return obj;
		}

		/// <summary>
		/// Recycle the object from the input reference.
		/// If the object was not created by the pool system, it will destroy the object using unity Destroy.
		/// </summary>
		/// <param name="obj">the object instance that we are recycling</param>
		public void RecycleInstance(GameObject obj)
		{
			//check the instance has been actually created with this pooling system
			if (m_InstanceToPool.ContainsKey(obj))
			{
				//if it has been, then get the pool of that object and recycle the object
				ObjectPool pool = m_InstanceToPool[obj];
				pool.Recycle(obj);
			}
			//otherwise we are deleting an object that was not created by this pooling system
			else
			{
				// give a warning 
				Debug.LogWarning("Destroying non-pooled object " + obj.name);

				//and destroy it using Unity
				Object.Destroy(obj);
			}
		}

		/// <summary>
		/// Recycle the object from the input reference after <delay> seconds have elapsed
		/// </summary>
		/// <param name="obj">the object instance that we are recycling</param>
		/// <param name="delay">delay before the recycling</param>
		/// <returns></returns>
		public IEnumerator RecycleInstance(GameObject obj, float delay)
		{
			//delay the recycling process
			yield return new WaitForSeconds(delay);

			//and recycle
			RecycleInstance(obj);
		}

		/// <summary>
		/// recycle totally a pool and all its instances 
		/// this will not free any memory. this will merely fill the trash with the objects, 
		/// afterwards you can free memory by empting the trash
		/// </summary>
		/// <param name="prefab">the prefab of the pool we want to trash</param>
		/// <returns></returns>
		public IEnumerator SendPoolToTrash(GameObject prefab)
		{
			//dont do anything if there arent pools of that kind of prefab
			if (!m_PrefabToPool.ContainsKey(prefab))
			{
				yield break; //breaks the iterator cycle
			}

			//otherwise get the pool root object
			Transform PoolRoot = m_PrefabToPool[prefab].transform;

			//and send all of its children (object instances) to the trash
			for (int k = PoolRoot.childCount - 1; k >= 0; k--)
			{
				//get the instance transform
				Transform p = PoolRoot.GetChild(k);

				//delete it from the instance pool
				m_InstanceToPool.Remove(p.gameObject);

				//and add it to the trash
				m_Trash.AddGameObjectToTrash(p);

				//we do this asynchronously (for the moment every half a second we send an object to the trash)
				yield return new WaitForSeconds(0.25f);
			}

			//remove also the pool from the pool's table
			m_PrefabToPool.Remove(prefab);

			//and add it to the trash as well
			m_Trash.AddGameObjectToTrash(PoolRoot);
		}

		/// <summary>
		/// this is used internally to create a new pool for the type of object the input prefab specifies
		/// </summary>
		/// <param name="prefab">the prefab from which we want to create a new pool </param>
		/// <returns></returns>
		private ObjectPool CreatePool(GameObject prefab)
		{
			//create a new object with the prefab name 
			GameObject obj = new GameObject(prefab.name + "-POOL");

			//add a pool component 
			ObjectPool pool = obj.AddComponent<ObjectPool>();

			//initialize the pool with the input prefab
			pool.InitializeWith(prefab);

			//return the new pool reference
			return pool;
		}

	}
}