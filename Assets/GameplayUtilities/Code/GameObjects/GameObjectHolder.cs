using UnityEngine;

namespace GameplayUtility
{
	public class GameObjectHolder : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_GameObjectRef;
		public GameObject GameObjectRef { get { return m_GameObjectRef; } }
	}
}