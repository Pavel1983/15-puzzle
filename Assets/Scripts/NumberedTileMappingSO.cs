using UnityEngine;

namespace Puzzle15
{
	[CreateAssetMenu]
	public class NumberedTileMappingSO: TilesMappingSO
	{
		[SerializeField] private int[] _orderedTileset;
		
		public override object OrderedTilesContent => _orderedTileset;
	}
}