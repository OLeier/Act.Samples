using Act.Framework.CustomEntities;
using Act.Framework.MutableEntities;
using System;

namespace IBLeier.CustomEntitiesSamples
{
	/// <summary>
	/// IbolTests - IBOL_Tests
	/// </summary>
	[CLSCompliant(false)]
	public sealed class IbolTests : CustomSubEntity
	{
		/// <summary>
		/// IbolTableName
		/// </summary>
		/// <remarks>
		/// IBOL - IB-Leier.net
		/// </remarks>
		public const string IbolTableName = "IBOL_Tests";

		public IbolTests(CustomSubEntityInitializationState state)
			: base(state)
		{

		}

		/// <summary>
		/// Created Date from <see cref="MutableEntity"/>.
		/// </summary>
		public override DateTime Created { get { return base.Created; } }

		/// <summary>
		/// TypeId - TestTypeId
		/// </summary>
		public int TypeId
		{
			get
			{
				return (int)this.Fields["TypeId", FieldNameType.Alias];
			}
			set
			{
				this.Fields["TypeId", FieldNameType.Alias] = value;
			}
		}

		public Guid TestId
		{
			get
			{
				return new Guid((string)this.Fields["TestId", FieldNameType.Alias]);
			}
			set
			{
				this.Fields["TestId", FieldNameType.Alias] = value.ToString();
			}
		}
	}
}
