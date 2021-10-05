using System;


namespace BIT.Data.Sync
{
    /// <summary>
    /// Represents a transaction made to the database 
    /// </summary>
    public interface IDelta
    {
        /// <summary>
        /// The date when the operation happened
        /// </summary>
        DateTime Date { get; set; }
        /// <summary>
        /// The date when the operation happened
        /// </summary>
        double Epoch { get; set; }
        /// <summary>
        /// Who created the delta
        /// </summary>
        string Identity { get; set; }
        /// <summary>
        /// The unique identifier of the delta
        /// </summary>
        Guid Index { get; }
        /// <summary>
        /// The database transaction(s) that represents this delta
        /// </summary>
        byte[] Operation { get; set; }
        /// <summary>
        /// True if the delta has been already processed 
        /// </summary>
        bool Processed { get; set; }

    }
}