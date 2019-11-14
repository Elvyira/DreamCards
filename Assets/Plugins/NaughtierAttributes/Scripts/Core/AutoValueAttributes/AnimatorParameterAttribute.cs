using UnityEngine;

namespace NaughtierAttributes
{
	public class AnimatorParameterAttribute : BaseAutoValueAttribute
	{
		public readonly int ParameterId;

		public AnimatorParameterAttribute(string parameterName, bool playUpdate = false) : base(playUpdate)
		{
			ParameterId = Animator.StringToHash(parameterName);
		}
	}
}