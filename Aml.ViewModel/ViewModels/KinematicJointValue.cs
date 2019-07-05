﻿using System;
using System.Globalization;
using System.Linq;
using Aml.Contracts;
using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;

namespace Aml.ViewModel
{
	public class KinematicJointValue : DoublePropertyViewModel
	{
		#region Consts

		internal const string AttributeRefTypeName = "Kinematik/JointValue";
		private const double Epsilon = 1e-6;

		#endregion // Consts

		public double Minimum
		{
			get
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null) return 0d;
				return Convert.ToDouble(requirement.OrdinalScaledType.RequiredMinValue, CultureInfo.InvariantCulture);
			}
			set
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null)
				{
					requirement = Provider.CaexDocument.Create<AttributeValueRequirementType>();
					requirement.New_OrdinalType();
					_attribute.Constraint.Insert(requirement);
				}
				requirement.OrdinalScaledType.RequiredMinValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public double Maximum
		{
			get
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null) return 0d;
				return Convert.ToDouble(requirement.OrdinalScaledType.RequiredMaxValue, CultureInfo.InvariantCulture);
			}
			set
			{
				var requirement = _attribute.Constraint.FirstOrDefault(x => x.OrdinalScaledType != null);
				if (requirement == null)
				{
					requirement = Provider.CaexDocument.Create<AttributeValueRequirementType>();
					requirement.New_OrdinalType();
					_attribute.Constraint.Insert(requirement);
				}
				requirement.OrdinalScaledType.RequiredMaxValue = Convert.ToString(value, CultureInfo.InvariantCulture);
			}
		}

		public KinematicJointValue(IAmlProvider provider) : base(provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
		}

		public KinematicJointValue(AttributeType model, IAmlProvider provider) : base(model, provider)
		{
			_attribute.RefAttributeType = AttributeRefTypeName;
		}
	}
}