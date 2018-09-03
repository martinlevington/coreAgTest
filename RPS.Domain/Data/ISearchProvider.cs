using System.Collections.Generic;

namespace RPS.Domain.Data
{
    public interface ISearchProvider<TEntity>
    {
        IEnumerable<TEntity> QueryString(string term);

        void AddUpdateEntity(TEntity skill);
        void UpdateSkill(long updateId, string updateName, string updateDescription);
        void DeleteSkill(long updateId);
    }
}
