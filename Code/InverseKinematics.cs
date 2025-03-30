using System;
using Sandbox;

public sealed class InverseKinematics : Component
{
	[Property] SkinnedModelRenderer Renderer { get; set; }
	[Property] GameObject Shoulder { get; set; }
	[Property] GameObject LeftHand { get; set; }

	protected override void OnStart()
	{
	}
	
	protected override void OnUpdate()
	{
		LeftHand.WorldRotation = Rotation.Random;
	}
}
