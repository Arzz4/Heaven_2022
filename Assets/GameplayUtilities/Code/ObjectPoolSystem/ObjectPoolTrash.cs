using UnityEngine;
using System.Collections;

namespace GameplayUtility
{
	/// <summary>
	///This is used by the object pool system and is not suppose to be used otherwise
	///the ObjectPoolTrash main functionality is to delete objects of any pool using unity destroy method. 
	///this deletion is done asynchrhously with coroutines
	///there is a variable called empty rate. This corresponds to the time in between Unity Destroy method is called
	/// </summary>
	public class ObjectPoolTrash : MonoBehaviour
	{
		//This corresponds to the time in between calls to Unity Destroy, default is 1 second
		private float m_EmptyRate = 1f;

		//is the destroy cycle currently on?
		bool m_OnCycle = false;

		//a reference to the trash transform, for better performance is cached here
		//it is used during the deletion cycle
		Transform m_Transform;

		/// <summary>
		/// called by unity when this script is loaded
		/// </summary>
		void Awake()
		{
			m_Transform = transform;
		}

		/// <summary>
		/// empties the trash, this will not call the garbage collector, that is left for the caller to decide
		/// </summary>
		/// <returns></returns>
		public IEnumerator EmptyTrashCycle()
		{
			//do nothing is the deletion is currently under progress
			if (m_OnCycle)
				yield break;

			//otherwise mark we are currently 
			m_OnCycle = true;

			//while the trash has objects to delete (childs)
			while (m_Transform.childCount > 0)
			{
				//destroy the first child that we find
				Destroy(m_Transform.GetChild(0).gameObject);

				//wait for the next deletion iteration
				yield return new WaitForSeconds(m_EmptyRate);
			}

			//here there arent more objects to be deleted, so we exit the cycle
			m_OnCycle = false;
		}

		/// <summary>
		/// adds an object to the trash
		/// </summary>
		/// <param name="objT">the object that is being sent to the trash</param>
		public void AddGameObjectToTrash(Transform objT)
		{
			objT.parent = m_Transform;
		}

		/// <summary>
		/// sets the empty rate of the trash
		/// </summary>
		/// <param name="rate">delay to be applied in between calls to the Destroy method when empting the trash</param>
		public void SetEmptyRate(float rate)
		{
			m_EmptyRate = rate;
		}
	}
}