using System;
using UnityEngine;

namespace Puzzle15
{
	public abstract class TilesMappingSO: ScriptableObject, ITilesMapping
	{
		public virtual object OrderedTilesContent => throw new NotImplementedException();
	}
}