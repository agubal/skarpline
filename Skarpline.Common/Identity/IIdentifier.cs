namespace Skarpline.Common.Identity
{
    /// <summary>
    /// Interface that verifies that type of Id od DB Entity is the same as  it's ViewModel id type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIdentifier<T>
    {
        /// <summary>
        /// Type of Id property
        /// </summary>
        T Id { get; set; }
    }
}
