namespace CalloutInterfaceHelper.Records
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Represents a database of records.
    /// </summary>
    /// <typeparam name="TEntity">The type of Rage.Entity.</typeparam>
    /// <typeparam name="TRecord">The type of EntityRecord.</typeparam>
    internal abstract class RecordDatabase<TEntity, TRecord>
        where TEntity : Rage.Entity
        where TRecord : EntityRecord<TEntity>
    {
        private readonly Stopwatch pruneTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordDatabase{TEntity, TRecord}"/> class.
        /// </summary>
        protected RecordDatabase()
        {
            this.pruneTimer = Stopwatch.StartNew();
        }

        /// <summary>
        /// Gets a dictionary of entity records with the underlying entity as key.
        /// </summary>
        protected Dictionary<Rage.PoolHandle, TRecord> Entities { get; } = new Dictionary<Rage.PoolHandle, TRecord>();

        /// <summary>
        /// Gets a record from the database based on the given entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The relevant record.</returns>
        public TRecord GetRecord(TEntity entity)
        {
            if (this.pruneTimer.Elapsed.TotalMinutes > 10)
            {
                this.pruneTimer.Restart();
                this.Prune(30);
            }

            TRecord record = null;
            if (entity)
            {
                if (this.Entities.ContainsKey(entity.Handle))
                {
                    record = this.Entities[entity.Handle];
                }
                else
                {
                    record = this.CreateRecord(entity);
                    this.Entities[entity.Handle] = record;
                }
            }

            return record;
        }

        /// <summary>
        /// Prunes the database of records.
        /// </summary>
        /// <param name="minutes">The maximum age of records in minutes.</param>
        internal abstract void Prune(int minutes);

        /// <summary>
        /// Creates a new record based on the entity.
        /// </summary>
        /// <param name="entity">The entity from which to create the record.</param>
        /// <returns>The new record.</returns>
        protected abstract TRecord CreateRecord(TEntity entity);
    }
}
