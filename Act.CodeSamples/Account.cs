using Act.Framework.CustomEntities;
using System;

namespace Act.CodeSamples
{
	public sealed class Account : CustomSubEntity
	{

		public Account(CustomSubEntityInitializationState state)
			: base(state)
		{
		}

		public Guid AppId { get; set; }
	}

	// Using the type in a call to get the CustomSubEntityManager. 
}
