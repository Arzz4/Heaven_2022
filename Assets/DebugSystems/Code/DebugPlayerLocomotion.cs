using ImGuiNET;
using UnityEngine;

namespace DebugSystems
{
	public class DebugPlayerLocomotion : DebugSystemBase
	{
		[SerializeField]
		private PlayerPlatformer2D.PlayerRuntimeData m_RuntimeData = default;

		protected override void DebugDraw(UImGui.UImGui obj)
		{
			var data = m_RuntimeData.PhysicsContextMainRuntimeData;

			ImGui.Begin("Player - Locomotion Runtime");

			// JUMP
			ImGui.TextColored(new Vector4(0, 1, 0, 0.5f), "----------------------- ");

			ImGui.TextColored(Color.cyan, "Jump");
			ImGui.TextColored(data.queuedJump ? Color.green : Color.red,   "Queued Jumping ?                  : " + data.queuedJump);
			ImGui.Text       (                                             "Jump State                        : " + data.jumpState);
			ImGui.TextColored(data.lowJump ? Color.green : Color.red,      "Low Jumping ?                     : " + data.lowJump);
			ImGui.TextColored(data.isWallJumping ? Color.green : Color.red,"Wall Jumping ?                    : " + data.isWallJumping);
			ImGui.TextColored(data.isWallJumping ? Color.green : Color.red,"Sticky Surface ?                  : " + data.onStickySurface);

			// Settings
			ImGui.TextColored(new Vector4(0, 1, 0, 0.5f), "----------------------- ");

			ImGui.TextColored(Color.cyan, "Settings");
			ImGui.Text("Motion Settings                : " + data.mainSettings.name);
			ImGui.Text("Jump Settings                  : " + data.jumpSettings.name);

			ImGui.End();
		}
	}
}