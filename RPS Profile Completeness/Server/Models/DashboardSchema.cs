using System;
using GraphQL.Types;

namespace RPS.Presentation.Server.Models
{
    public class DashboardSchema : Schema,IDashboardSchema
    {

        public DashboardSchema(Func<Type, GraphType> resolveType)
            :base(resolveType)
        {
            Query = (DashboardQuery)resolveType(typeof(DashboardQuery));
        }
    }
}
