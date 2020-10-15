namespace DevsDNA.Application.Tests
{
	using DevsDNA.Application.Services;
	using System;
	using System.Collections.Generic;
	using System.Text;

	public class MockDependencyService : IDependencyService
	{
		private readonly IDictionary<Type, object> registeredServices = new Dictionary<Type, object>();

		public void Register<T>(T implementation)
		{
			this.registeredServices.Add(typeof(T), implementation);
		}

		public T Get<T>() where T : class
		{
			return (T)this.registeredServices[typeof(T)];
		}
	}
}
