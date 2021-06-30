namespace Puzzle15
{
	// todo: переименовать ITilesMapping в ITilesDataProvider
	public interface ITilesMapping
	{
		string Id { get; }
		object OrderedTilesContent { get; }		// Совокупность тайлов отсортированных по порядку на игровом поле
	}
}