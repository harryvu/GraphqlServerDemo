using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class DeviceSchema : Schema
    {
        public DeviceSchema(IHelpDesk hd)
        {
            Query = new DeviceQuery(hd);
            Mutation = new DeviceMutation(hd);
            Subscription = new DeviceSubscriptions(hd);
        }
    }
}
