using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameplayUtility
{
	public enum EaseCurveType
	{
		None, // linear

		EaseInSine,
		EaseInCubic,
		EaseInQuint,
		EaseInCirc,
		EaseInElastic,
		EaseInQuad,
		EaseInQuart,
		EaseInExpo,
		EaseInBack,
		EaseInBounce,

		EaseOutSine,
		EaseOutCubic,
		EaseOutQuint,
		EaseOutCirc,
		EaseOutElastic,
		EaseOutQuad,
		EaseOutQuart,
		EaseOutExpo,
		EaseOutBack,
		EaseOutBounce,

		EaseInOutSine,
		EaseInOutCubic,
		EaseInOutQuint,
		EaseInOutCirc,
		EaseInOutElastic,
		EaseInOutQuad,
		EaseInOutQuart,
		EaseInOutExpo,
		EaseInOutBack,
		EaseInOutBounce
	}

	public class EasingCurves
	{
		///////////////////////////////
		// EASE INS

		public static float EaseInSine(float x)
		{
			return 1.0f - Mathf.Cos((x * Mathf.PI) * 0.5f);
		}

		public static float EaseInCubic(float x)
		{
			return x * x * x;
		}

		public static float EaseInQuint(float x)
		{
			return x * x * x * x * x;
		}

		public static float EaseInCirc(float x)
		{
			return 1.0f - Mathf.Sqrt(1.0f - x * x);
		}

		public static float EaseInElastic(float x)
		{
			if (x == 0.0f)
				return 0.0f;

			if (x == 1.0f)
				return 1.0f;

			float c4 = (2.0f * Mathf.PI) * 0.3333333f;
			return -Mathf.Pow(2.0f, 10.0f * x - 10.0f) * Mathf.Sin((x * 10.0f - 10.75f) * c4);
		}

		public static float EaseInQuad(float x)
		{
			return x * x;
		}

		public static float EaseInQuart(float x)
		{
			return x * x * x * x;
		}

		public static float EaseInExpo(float x)
		{
			return x == 0.0f ? 0.0f : Mathf.Pow(2.0f, 10.0f * x - 10.0f);
		}

		public static float EaseInBack(float x)
		{
			const float c1 = 1.70158f;
			const float c3 = c1 + 1.0f;

			return c3 * x * x * x - c1 * x * x;
		}

		public static float EaseInBounce(float x)
		{
			return 1.0f - EaseOutBounce(1.0f - x);
		}

		///////////////////////////////
		// EASE OUTS

		public static float EaseOutSine(float x)
		{
			return Mathf.Sin((x * Mathf.PI) / 2.0f);
		}

		public static float EaseOutCubic(float x)
		{
			float y = 1.0f - x;
			return 1.0f - y * y * y;
		}

		public static float EaseOutQuint(float x)
		{
			float y = 1.0f - x;
			return 1.0f - y * y * y * y * y;
		}

		public static float EaseOutCirc(float x)
		{
			return Mathf.Sqrt(1.0f - Mathf.Pow(x - 1.0f, 2.0f));
		}

		public static float EaseOutElastic(float x)
		{
			if (x == 0.0f)
				return 0.0f;

			if (x == 1.0f)
				return 1.0f;

			float c4 = (2.0f * Mathf.PI) * 0.3333333f;
			return Mathf.Pow(2.0f, -10.0f * x) * Mathf.Sin((x * 10.0f - 0.75f) * c4) + 1.0f;
		}

		public static float EaseOutQuad(float x)
		{
			return 1.0f - (1.0f - x) * (1.0f - x);
		}

		public static float EaseOutQuart(float x)
		{
			float y = 1 - x;
			return 1.0f - y * y * y * y;
		}

		public static float EaseOutExpo(float x)
		{
			return x == 1.0f ? 1.0f : 1.0f - Mathf.Pow(2.0f, -10.0f * x);
		}

		public static float EaseOutBack(float x)
		{
			const float c1 = 1.70158f;
			const float c3 = c1 + 1.0f;

			float y = x - 1.0f;
			return 1 + c3 * y * y * y + c1 * y * y;
		}

		public static float EaseOutBounce(float x)
		{
			const float n1 = 7.5625f;
			const float d1 = 2.75f;

			if (x < 1.0f / d1)
				return n1 * x * x;

			if (x < 2.0f / d1)
				return n1 * (x -= 1.5f / d1) * x + 0.75f;

			if (x < 2.5f / d1)
				return n1 * (x -= 2.25f / d1) * x + 0.9375f;

			return n1 * (x -= 2.625f / d1) * x + 0.984375f;
		}

		///////////////////////////////
		// EASE IN AND OUT

		public static float EaseInOutSine(float x)
		{
			return -(Mathf.Cos(Mathf.PI * x) - 1.0f) / 2.0f;
		}

		public static float EaseInOutCubic(float x)
		{
			if (x < 0.5f)
				return 4.0f * x * x * x;

			float y = -2.0f * x + 2.0f;
			return 1.0f - y * y * y * 0.5f;
		}

		public static float EaseInOutQuint(float x)
		{
			if (x < 0.5f)
				return 16.0f * x * x * x * x * x;

			float y = -2.0f * x + 2.0f;
			return 1.0f - y * y * y * y * y * 0.5f;
		}

		public static float EaseInOutCirc(float x)
		{
			if (x < 0.5f)
				return (1.0f - Mathf.Sqrt(1 - 4.0f * x * x)) * 0.5f;

			float y = -2.0f * x + 2.0f;
			return (Mathf.Sqrt(1 - y * y) + 1) * 0.5f;
		}

		public static float EaseInOutElastic(float x)
		{
			if (x == 0.0f)
				return 0.0f;

			if (x == 1.0f)
				return 1.0f;

			float c5 = (2.0f * Mathf.PI) * 0.222222222f;

			if (x < 0.5f)
				return -(Mathf.Pow(2.0f, 20.0f * x - 10.0f) * Mathf.Sin((20.0f * x - 11.125f) * c5)) * 0.5f;

			return (Mathf.Pow(2.0f, -20.0f * x + 10.0f) * Mathf.Sin((20.0f * x - 11.125f) * c5)) * 0.5f + 1;
		}

		public static float EaseInOutQuad(float x)
		{
			if (x < 0.5f)
				return 2f * x * x;

			float y = -2f * x + 2f;
			return 1.0f - y * y * 0.5f;
		}

		public static float EaseInOutQuart(float x)
		{
			if (x < 0.5f)
				return 8.0f * x * x * x * x;

			float y = -2f * x + 2f;
			return 1.0f - y * y * y * y * 0.5f;
		}

		public static float EaseInOutExpo(float x)
		{
			if (x == 0.0f)
				return 0.0f;

			if (x == 1.0f)
				return 1.0f;

			if (x < 0.5f)
				return Mathf.Pow(2.0f, 20.0f * x - 10.0f) * 0.5f;

			return (2.0f - Mathf.Pow(2.0f, -20.0f * x + 10.0f)) * 0.5f;
		}

		public static float EaseInOutBack(float x)
		{
			const float c1 = 1.70158f;
			const float c2 = c1 * 1.525f;

			if (x < 0.5f)
				return (4 * x * x * ((c2 + 1) * 2 * x - c2)) * 0.5f;

			float y = 2 * x - 2;
			return ( y * y * ((c2 + 1) * (x * 2 - 2) + c2) + 2) * 0.5f;
		}

		public static float EaseInOutBounce(float x)
		{
			return x < 0.5f ? (1 - EaseOutBounce(1.0f - 2.0f * x)) * 0.5f : (1.0f + EaseOutBounce(2.0f * x - 1.0f)) * 0.5f;
		}

		public static float EvaluateEaseType(float x, EaseCurveType anEaseType)
		{
			switch (anEaseType)
			{
				case EaseCurveType.None				: return x;
				case EaseCurveType.EaseInSine		: return EaseInSine(x);
				case EaseCurveType.EaseInCubic		: return EaseInCubic(x);
				case EaseCurveType.EaseInQuint		: return EaseInQuint(x);
				case EaseCurveType.EaseInCirc		: return EaseInCirc(x);
				case EaseCurveType.EaseInElastic	: return EaseInElastic(x);
				case EaseCurveType.EaseInQuad		: return EaseInQuad(x);
				case EaseCurveType.EaseInQuart		: return EaseInQuart(x);
				case EaseCurveType.EaseInExpo		: return EaseInExpo(x);
				case EaseCurveType.EaseInBack		: return EaseInBack(x);
				case EaseCurveType.EaseInBounce		: return EaseInBounce(x);
				case EaseCurveType.EaseOutSine		: return EaseOutSine(x);
				case EaseCurveType.EaseOutCubic		: return EaseOutCubic(x);
				case EaseCurveType.EaseOutQuint		: return EaseOutQuint(x);
				case EaseCurveType.EaseOutCirc		: return EaseOutCirc(x);
				case EaseCurveType.EaseOutElastic	: return EaseOutElastic(x);
				case EaseCurveType.EaseOutQuad		: return EaseOutQuad(x);
				case EaseCurveType.EaseOutQuart		: return EaseOutQuart(x);
				case EaseCurveType.EaseOutExpo		: return EaseOutExpo(x);
				case EaseCurveType.EaseOutBack		: return EaseOutBack(x);
				case EaseCurveType.EaseOutBounce	: return EaseOutBounce(x);
				case EaseCurveType.EaseInOutSine	: return EaseInOutSine(x);
				case EaseCurveType.EaseInOutCubic	: return EaseInOutCubic(x);
				case EaseCurveType.EaseInOutQuint	: return EaseInOutQuint(x);
				case EaseCurveType.EaseInOutCirc	: return EaseInOutCirc(x);
				case EaseCurveType.EaseInOutElastic	: return EaseInOutElastic(x);
				case EaseCurveType.EaseInOutQuad	: return EaseInOutQuad(x);
				case EaseCurveType.EaseInOutQuart	: return EaseInOutQuart(x);
				case EaseCurveType.EaseInOutExpo	: return EaseInOutExpo(x);
				case EaseCurveType.EaseInOutBack	: return EaseInOutBack(x);
				case EaseCurveType.EaseInOutBounce	: return EaseInOutBounce(x);
			}

			Debug.Log("Error, unrecognized ease curve type! : " + anEaseType.ToString());
			return 0.0f;
		}

	}
}