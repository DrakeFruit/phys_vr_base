using Sandbox;
using Sandbox.Citizen;

[Title("VR Player Controller")]
[Category("VR")]
[Icon("directions_walk")]
[EditorHandle("materials/gizmo/charactercontroller.png")]
public sealed class VRPlayerController : Component
{
	[RequireComponent] CitizenAnimationHelper Anim { get; set; }
	[RequireComponent] Rigidbody RigidBody { get; set; }
	
	[Property, Group( "Collider" )] HullCollider LowerCollider { get; set; }
	[Property, Group( "Collider" )] HullCollider UpperCollider { get; set; }
	
	[Property, Group( "Head" )] GameObject Head { get; set; }
	[Property, Group( "Head" )] GameObject Eyes { get; set; }
	[Property, Group( "Left Hand" )] GameObject HandL { get; set; }
	[Property, Group( "Left Hand" )] GameObject IKTargetL { get; set; }
	[Property, Group( "Right Hand" )] GameObject HandR { get; set; }
	[Property, Group( "Right Hand" )] GameObject IKTargetR { get; set; }
	
	public SkinnedModelRenderer ShadowCaster { get; set; }
	protected override void OnStart()
	{
		if ( IsProxy ) return;
		
		ShadowCaster = Anim.Target.GameObject.Clone().GetComponent<SkinnedModelRenderer>();
		ShadowCaster.GameObject.SetParent( GameObject );
		ShadowCaster.GameObject.Name = "ShadowCaster";
		ShadowCaster.BoneMergeTarget = Anim.Target;
		ShadowCaster.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
		
		Anim.Target.SetBodyGroup( "Head", 3 );
		Anim.IkLeftHand = IKTargetL;
		Anim.IkRightHand = IKTargetR;
	}

	protected override void OnUpdate()
	{
		if ( IsProxy ) return;

		Anim.DuckLevel = 1 - (Head.LocalPosition.z - 10 ).Remap( 32, 55 );
		Anim.Target.WorldRotation = Rotation.LookAt( Scene.Camera.WorldRotation.Forward ).Angles().WithPitch( 0 );
		Anim.Target.GameObject.Parent.LocalPosition += Head.WorldPosition.WithZ( 0 ) - Eyes.WorldPosition.WithZ( 0 );
		
		Scene.Camera.WorldPosition = Head.WorldPosition;
		Scene.Camera.WorldRotation = Head.WorldRotation;
	}
	
 	protected override void OnFixedUpdate()
    {
	    if ( IsProxy ) return;

	    LowerCollider.Height = Head.LocalPosition.z.Clamp( 8, 70 ) + 4;
	    LowerCollider.Center = LowerCollider.Center.WithZ( Head.LocalPosition.z / 2 + 2 );
    }
}
