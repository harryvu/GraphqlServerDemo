using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace BLL
{
    public interface IHelpDesk
    {
        ConcurrentStack<Device> AllDevices { get; }

        Device AddDevice(Device device);

		Device UpdateDevice(Device device);

		/// what properties subscribers might interested to, such as
		/// - isOnline
		/// - startsWith (name)
		/// - and many others
		IObservable<Device> ObservableDevices(Expression<Func<Device, bool>> predicate);
    }

    public class HelpDesk : IHelpDesk
    {
        private readonly ISubject<Device> _deviceStream = new ReplaySubject<Device>(1);

        public HelpDesk()
        {
            AllDevices = new ConcurrentStack<Device>();
            AllDevices.Push(new Device { Id = 1, Name = "Simulated SMP", IsOnline = true });
        }

        public ConcurrentStack<Device> AllDevices { get; }

        public Device AddDevice(Device device)
        {
            AllDevices.Push(device);
            _deviceStream.OnNext(device);
            return device;
        }

		public Device UpdateDevice(Device device)
		{
			var arrDevices = AllDevices.ToArray();
			var deviceFound = Array.Find(arrDevices, d => d.Id == device.Id);
			deviceFound.Name = device.Name;
			deviceFound.IsOnline = device.IsOnline;

			_deviceStream.OnNext(device);
			return deviceFound;
		}

		public IObservable<Device> ObservableDevices(Expression<Func<Device, bool>> predicate)
        {
			return _deviceStream.Where(predicate.Compile());
		}
    }
}
