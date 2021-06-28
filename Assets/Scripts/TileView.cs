using System;
using UnityEngine;

namespace Puzzle15.UI
{
	public abstract class TileView : MonoBehaviour
	{
		[SerializeField] protected SpriteRenderer _backgroundImg;

		public void ScaleToSize(Vector2 size)
		{
			var currentSize = _backgroundImg.bounds.size;
			
			_backgroundImg.transform.localScale = new Vector3(size.x / currentSize.x, size.y / currentSize.y, 1);
		}

		public virtual void Setup(TileData tileData)
		{
			throw new NotImplementedException();
		}
	}
}