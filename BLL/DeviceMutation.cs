using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class DeviceMutation : ObjectGraphType<object>
    {
        public DeviceMutation(IHelpDesk hd)
        {
            Field<DeviceType>(
                "addDevice",
                arguments: new QueryArguments(
                    new QueryArgument<DeviceInputType> { Name = "device" }
                ),
                resolve: context =>
                {
                    var receivedDevice = context.GetArgument<Device>("device");
                    var device = hd.AddDevice(receivedDevice);
                    return device;
                });

            Field<DeviceType>(
                "updateDevice",
                arguments: new QueryArguments(
                    new QueryArgument<DeviceInputType> { Name = "device" }
                ),
                resolve: context =>
                {
                    var receivedDevice = context.GetArgument<Device>("device");
                    var devices = hd.AllDevices.ToArray();
                    var device = Array.Find(devices, d => d.Id == receivedDevice.Id);
                    device.IsOnline = receivedDevice.IsOnline;

					hd.UpdateDevice(device);

                    return device;
                });
        }
    }

    public class DeviceInputType : InputObjectGraphType
    {
        public DeviceInputType()
        {
            Field<IntGraphType>("Id");
            Field<StringGraphType>("Name");
            Field<BooleanGraphType>("IsOnline");
        }
    }
}
