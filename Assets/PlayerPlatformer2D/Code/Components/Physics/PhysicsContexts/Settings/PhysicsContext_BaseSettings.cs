using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerPlatformer2D
{
	public enum PhysicsContextSettingsType
	{
		Motion,
		Jump
	}

	public abstract class PhysicsContext_BaseSettings : ScriptableObject
	{
		public abstract PhysicsContextSettingsType GetSettingsType();
	}
}