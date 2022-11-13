using ImGuiNET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystems
{
	public class DebugPlayerCollision : DebugSystemBase
	{
		[SerializeField]
		private PlayerPlatformer2D.PlayerRuntimeData m_RuntimeData = default;

		protected override void DebugDraw(UImGui.UImGui obj)
		{
			var data = m_RuntimeData.PlayerCollisionRuntimeData;

			ImGui.Begin("Player - Collision");

			// Ground
			ImGui.TextColored(new Vector4(0, 1, 0, 0.5f), "----------------------- ");

			ImGui.TextColored(Color.cyan, "Ground State");
			ImGui.TextColored(data.onGround ? Color.green : Color.red            , "On Ground?                             : " + data.onGround);
			ImGui.TextColored(data.onGround ? Color.green : Color.red            , "Ground timestamp                       : " + data.onGroundTimestamp);
			ImGui.TextColored(data.onStickySurface ? Color.green : Color.red     , "On Sticky Surface?                     : " + data.onStickySurface);
			ImGui.TextColored(data.onBouncySurface ? Color.green : Color.red     , "On Bouncy Surface?                     : " + data.onBouncySurface);
			ImGui.TextColored(data.onSpeedySurface ? Color.green : Color.red     , "On Speedy Surface?                     : " + data.onSpeedySurface);
			//ImGui.TextColored(data.groundObject != null ? Color.green : Color.red, "Ground Object                          : " + (data.groundObject != null ? data.groundObject.name : "NULL"));

			// WALL
			ImGui.TextColored(new Vector4(0, 1, 0, 0.5f), "----------------------- ");

			ImGui.TextColored(Color.cyan, "Wall State");
			ImGui.TextColored(data.onWall ? Color.green : Color.red     , "On Wall?                  : " + data.onWall);
			ImGui.TextColored(data.onRightWall ? Color.green : Color.red, "On Right Wall?            : " + data.onRightWall);
			ImGui.TextColored(data.onLeftWall ? Color.green : Color.red , "On Left Wall ?            : " + data.onLeftWall);

			// Settings
			ImGui.TextColored(new Vector4(0, 1, 0, 0.5f), "----------------------- ");

			ImGui.TextColored(Color.cyan, "Settings");
			ImGui.Text("Collision Settings                : " + data.settings.name);

			ImGui.End();
		}
	}
}