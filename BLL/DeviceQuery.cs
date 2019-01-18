using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class DeviceQuery : ObjectGraphType
    {
        public DeviceQuery(IHelpDesk hd)
        {
            Field<ListGraphType<DeviceType>>(
                "devices",
                resolve: context =>
                {
                    return hd.AllDevices.Take(100);
                }
            );
        }
    }
}
