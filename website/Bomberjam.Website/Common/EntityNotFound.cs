using System;
using System.Globalization;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Common
{
    [Serializable]
    public class EntityNotFound : BomberjamException
    {
        public EntityNotFound(EntityType entityType, Guid id)
            : this(entityType, id.ToString("D"))
        {
        }

        public EntityNotFound(EntityType entityType, int id)
            : this(entityType, id.ToString(CultureInfo.InvariantCulture))
        {
        }

        public EntityNotFound(EntityType entityType, string id)
            : base($"{entityType.ToString()} {id} not found")
        {
        }

        public EntityNotFound(EntityType entityType)
            : base($"{entityType.ToString()} not found")
        {
        }
    }
}