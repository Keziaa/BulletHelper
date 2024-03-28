using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyVoid.BulletHelper
{
	public static class BulletHelper
	{
		public static bool RandomBool()
		{
			return Random.value > 0.5f;
		}

		public static Vector2 AngleToDirection(float angle)
		{
			float angleRad = angle * Mathf.PI * 2f / 360.0f;
			return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
		}

		public static float DirectionToAngle(Vector2 direction)
		{
			return Mathf.Atan2(direction.y, direction.x) * 360 / (Mathf.PI * 2);
		}

		public static float[] CalculateSpreadshotAngles(float rootAngle, float spreadAngle, int numberOfBullets)
		{
			float[] angles = new float[numberOfBullets];
			for (int i = 0; i < numberOfBullets; i++)
			{
				float angle = (spreadAngle * i) - spreadAngle;
				float finalAngle = rootAngle + angle;
				angles[i] = finalAngle;
			}
			return angles;
		}

		public static Vector3 CalculateCurveshot(Vector2 startPosition, Vector2 endPosition, float apexHeight, float gravity)
		{
			float endPosY = endPosition.y;

			Vector3 newRoot = new Vector3(startPosition.x - endPosition.x, endPosY);
			float x = newRoot.x - startPosition.x;

			float apexDist = apexHeight;

			float toSqrRootForviY = Mathf.Abs(2 * gravity * apexDist);
			float viY = Mathf.Sqrt(toSqrRootForviY);

			float timeToApex = Mathf.Abs(viY / gravity);

			float toSqrtForTimeToG = Mathf.Abs(2 * (startPosition.y + apexDist) / gravity);
			float timeToGround = Mathf.Sqrt(toSqrtForTimeToG);

			float viX = x / (timeToApex + timeToGround);

			Vector3 speed = new Vector3(viX, viY);
			return speed;
		}

		public static bool SignsAreOpposite(Vector2 a, Vector2 b)
		{
			if (a.x != 0f && b.x != 0f && Mathf.Sign(a.x) != Mathf.Sign(b.x))
			{
				return true;
			}
			if (a.y != 0f && b.y != 0f && Mathf.Sign(a.y) != Mathf.Sign(b.y))
			{
				return true;
			}
			return false;
		}

		public static Vector2 GetRotatedPosition(float radians, Vector2 targetPos, float circleCircum)
		{
			Vector2 pivot = targetPos;
			Vector2 point = new Vector2(targetPos.x, targetPos.y + circleCircum);

			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			float x = cos * (point.x - pivot.x) - sin * (point.y - pivot.y) + pivot.x;
			float y = sin * (point.x - pivot.x) - cos * (point.y - pivot.y) + pivot.y;

			return new Vector2(x, y);
		}

		public static float GetDeflectedAngle(float angle, Vector2 contactPoint, Vector2 spawnOrigin)
		{
			Vector3 currentDir = AngleToDirection(angle);

			Vector2 point = spawnOrigin - contactPoint;
			Vector2 newDir = 1.0f * (-2 * Vector3.Dot(currentDir, Vector3.Normalize(point.normalized)) * Vector3.Normalize(point.normalized) + currentDir);

			float newAngle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
			return newAngle;
		}

		public static float ClampAngle(float angle, float min, float max)
		{
			float start = (min + max) * 0.5f - 180;
			float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
			return Mathf.Clamp(angle, min + floor, max + floor);
		}
	}
}
