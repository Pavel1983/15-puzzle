using System;
using UnityEngine;

namespace Puzzle15.UI
{
	public abstract class TileView : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer _backgroundImg;

		public virtual void Setup(TileData tileData)
		{
			throw new NotImplementedException();
		}
	}
}