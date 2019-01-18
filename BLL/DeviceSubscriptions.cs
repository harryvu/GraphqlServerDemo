using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace BLL
{
    public class DeviceSubscriptions : ObjectGraphType<object>
    {
        private readonly IHelpDesk _hd;

        public DeviceSubscriptions(IHelpDesk hd)
        {
            _hd = hd;

            AddField(new EventStreamFieldType
            {
                Name = "deviceAdded",
                Type = typeof(DeviceType),
                Resolver = new FuncFieldResolver<Device>(ResolveDevice),
                Subscriber = new EventStreamResolver<Device>(Subscribe)
            });

            AddField(new EventStreamFieldType
            {
                Name = "deviceStatusChanged",
                Arguments = new QueryArguments(
                    new QueryArgument<NonNullGraphType<BooleanGraphType>> { Name = "isOnline" }
                ),
                Type = typeof(DeviceType),
                Resolver = new FuncFieldResolver<Device>(ResolveDevice),
                Subscriber = new EventStreamResolver<Device>(SubscribeByStatus)
            });

            AddField(new EventStreamFieldType
            {
                Name = "deviceUpdated",
                Arguments = new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "deviceJson" }
                ),
                Type = typeof(DeviceType),
                Resolver = new FuncFieldResolver<Device>(ResolveDevice),
                Subscriber = new EventStreamResolver<Device>(deviceUpdatedSubscribe)
            });
        }

        private IObservable<Device> deviceUpdatedSubscribe(ResolveEventStreamContext context)
        {
            var id = context.GetArgument<int>("id");
            var deviceJson = context.GetArgument<string>("deviceJson");
            return _hd.ObservableDevices(d => true);
        }

        private IObservable<Device> Subscribe(ResolveEventStreamContext context)
        {
            //var id = context.GetArgument<int>("id");
            return _hd.ObservableDevices(d => true);
        }

		private IObservable<Device> SubscribeByStatus(ResolveEventStreamContext context)
		{
			var isOnline = context.GetArgument<bool>("isOnline");
			// find all devices that has status match the "isOnline" input

			var devices = _hd.ObservableDevices(d => d.IsOnline == isOnline);

			return devices;
		}

		private Device ResolveDevice(ResolveFieldContext context)
        {
            var device = context.Source as Device;

            return device;
        }
    }
}
