using UnityEngine;

namespace DebugSystems
{
	public class DebugSystemBase : MonoBehaviour
	{
		[SerializeField]
		protected UImGui.UImGui m_uimGuiInstance;

		private void OnEnable()
		{
			if (m_uimGuiInstance == null)
				Debug.LogError("Must assign a UImGuiInstance or use UImGuiUtility with Do Global Events on UImGui component.");

			m_uimGuiInstance.OnInitialize += OnInitialize;
			m_uimGuiInstance.Layout += OnLayout;
		}

		private void OnDisable()
		{
			m_uimGuiInstance.OnInitialize -= OnInitialize;
			m_uimGuiInstance.Layout -= OnLayout;
		}

		private void OnInitialize(UImGui.UImGui obj)
		{
			InitializeDebugDraw(obj);
		}

		private void OnLayout(UImGui.UImGui obj)
		{
			DebugDraw(obj);
		}

		protected virtual void InitializeDebugDraw(UImGui.UImGui obj) { }
		protected virtual void DebugDraw(UImGui.UImGui obj) { }
	}
}

