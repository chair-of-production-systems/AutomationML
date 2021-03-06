﻿using System;
using System.Collections.Generic;
using System.Linq;
using Aml.Engine.CAEX;

namespace Aml.ViewModel
{
	public class CaexViewModelFactoryManager
	{
		#region Singleton Implementation

		/// <summary>
		/// Static holder for instance, need to use lambda to construct since constructor private
		/// </summary>
		private static readonly Lazy<CaexViewModelFactoryManager> LazyInstance = new Lazy<CaexViewModelFactoryManager>(() => new CaexViewModelFactoryManager());

		/// <summary>
		/// Gets the singleton instance.
		/// </summary>
		/// <value>The instance.</value>
		public static CaexViewModelFactoryManager Instance => LazyInstance.Value;

		#endregion // Singleton Implementation

		private readonly List<ICaexViewModelFactory> _factories;

		private CaexViewModelFactoryManager()
		{
			_factories = new List<ICaexViewModelFactory>
			{
				new CaexViewModelFactory(),
				new ExternalDataConnectorViewModelFactory(),
				new GeometryDataConnectorViewModelFactory(),
				new ExternalGeometryConnectorViewModelFactory(),
				new AssemblyViewModelFactory(),
				new PartViewModelFactory(),
				new PropertyViewModelFactory(),
				new KinematicFactory()
			};
		}

		public ICaexViewModelFactory GetFactory<T>(ICAEXWrapper model) where T : CaexObjectViewModel
		{
			var factories = new List<ICaexViewModelFactory>();
			foreach (var factory in _factories)
			{
				// only factories that can create the destination type
				var canCreateDestinationType = factory.CanCreate<T>(model);
				if (!canCreateDestinationType) continue;
				
				// only factories that can handle the given CAEX object
				if (factory.TypeOfViewModel(model) == null) continue;

				factories.Add(factory);
			}

			if (factories.Count == 0) return null;
			if (factories.Count == 1) return factories[0];

			ICaexViewModelFactory match = null;
			foreach (var factory in factories)
			{
				var hasSubClass = false;
				foreach (var f in factories)
				{
					var thisType = factory.TypeOfViewModel(model);
					var otherType = f.TypeOfViewModel(model);
					if (otherType.IsSubclassOf(thisType)) hasSubClass = true;
				}

				if (!hasSubClass)
				{
					if (match != null) throw new Exception("Multiple factories found");
					match = factory;
				}
			}

			if (match == null) throw new Exception("Weird: factory missing for type " + model.GetType());
			return match;
		}

		public void Register(ICaexViewModelFactory factory)
		{
			_factories.Add(factory);
		}
	}
}