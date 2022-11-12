using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public class PlayerOrientationRuntimeData
	{
		public float facingSign = 1.0f;
	}

	public class PlayerOrientation : MonoBehaviour
	{
		[SerializeField]
		private PlayerRuntimeData m_RuntimeData = default;

		private void OnDrawGizmos()
		{
			var orientationData = m_RuntimeData ? m_RuntimeData.PlayerOrientationRuntimeData : null;
			if (orientationData == null)
				return;

			Gizmos.color = Color.green;
			Gizmos.DrawLine(transform.position, transform.position + Vector3.right * orientationData.facingSign);
		}

		public void PostUpdateCharacterOrientation()
		{
			var data = m_RuntimeData.PlayerOrientationRuntimeData;
			var visualsTransform = m_RuntimeData.PlayerUnityComponentsRuntimeData.visualsTransform;

			visualsTransform.right = Vector3.right * data.facingSign;
		}
	}
}