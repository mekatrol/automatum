namespace Mekatrol.Automatum.Models;

public enum PersistState
{
    /// <summary>
    /// The item is new and never persisted
    /// </summary>
    New,

    /// <summary>
    /// The item has not been modified
    /// </summary>
    Unmodified,

    /// <summary>
    /// The item is in a modified state
    /// </summary>
    Modified
}
