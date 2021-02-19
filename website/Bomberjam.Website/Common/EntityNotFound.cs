using System;
using System.Globalization;
using Bomberjam.Website.Models;

namespace Bomberjam.Website.Common
{
    [Serializable]
    public class EntityNotFound : BomberjamException
    {
        public EntityNotFound(ModelType modelType, Guid id)
            : this(modelType, id.ToString("D"))
        {
        }

        public EntityNotFound(ModelType modelType, int id)
            : this(modelType, id.ToString(CultureInfo.InvariantCulture))
        {
        }

        public EntityNotFound(ModelType modelType, string id)
            : base($"{modelType.ToString()} {id} not found")
        {
        }

        public EntityNotFound(ModelType modelType)
            : base($"{modelType.ToString()} not found")
        {
        }
    }
}