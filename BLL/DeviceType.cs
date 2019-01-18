using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class DeviceType : ObjectGraphType<Device>
    {
        public DeviceType()
        {
            Field(d => d.Id);
            Field(d => d.Name);
            Field(d => d.IsOnline);
        }
    }
}
