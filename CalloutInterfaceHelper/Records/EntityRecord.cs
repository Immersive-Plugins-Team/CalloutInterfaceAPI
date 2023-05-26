namespace CalloutInterfaceHelper.Records
{
    using System;

    /// <summary>
    /// Represents a single entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for the record.</typeparam>
    public abstract class EntityRecord<TEntity>
        where TEntity : Rage.Entity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRecord{TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The underlying entity for the record.</param>
        protected EntityRecord(TEntity entity)
        {
            this.Entity = entity;
            this.CreationDateTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the underlying entity for the record.
        /// </summary>
        public TEntity Entity { get; protected set; }

        /// <summary>
        /// Gets or sets the real world DateTime for when the record was created.
        /// </summary>
        public DateTime CreationDateTime { get; protected set; }
    }
}
