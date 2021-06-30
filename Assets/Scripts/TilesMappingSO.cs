using System;
using UnityEngine;

namespace Puzzle15
{
	public abstract class TilesMappingSO: ScriptableObject, ITilesMapping
	{
		[SerializeField] private string _id;
		public string Id => _id;
		public virtual object OrderedTilesContent => throw new NotImplementedException();
	}
}