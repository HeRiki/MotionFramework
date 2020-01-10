//--------------------------------------------------
// Motion Framework
// Copyright©2019-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using UnityEngine;

namespace MotionFramework.AI
{
	/// <summary>
	/// 启发式方法
	/// </summary>
	public static class AStarHeuristic
	{
		private static readonly float D = 1;
		private static readonly float D2 = Mathf.Sqrt(2) * D;

		/// <summary>
		/// 曼哈顿距离
		/// </summary>
		public static float ManhattanDist(Vector3Int a, Vector3Int b)
		{
			return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
		}

		/// <summary>
		/// 对角线距离
		/// </summary>
		public static float DiagonalDist(Vector3Int a, Vector3Int b)
		{
			float dx = Mathf.Abs(a.x - b.x);
			float dy = Mathf.Abs(a.y - b.y);
			return D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);
		}

		/// <summary>
		/// 欧式距离
		/// </summary>
		public static float EuclideanDist(Vector3Int a, Vector3Int b)
		{
			return Vector3Int.Distance(a, b);
		}
	}
}