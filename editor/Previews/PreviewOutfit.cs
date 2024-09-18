using Editor;
using Editor.Assets;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AssetPreview( "outfit" )]
class OutfitPreview : AssetPreview
{
	public OutfitPreview( Asset asset ) : base( asset ) { }

	List<SceneModel> sceneClothes = new List<SceneModel>();

	public override async Task InitializeAsset()
	{
		using ( EditorUtility.DisableTextureStreaming() )
		{
			var model = await Model.LoadAsync( "models/citizen/citizen.vmdl" );
			if ( model is null ) return;

			SceneCenter = model.RenderBounds.Center;
			SceneSize = Vector3.Zero;

			if ( model.MeshCount == 0 )
				return;

			var modelObj = new SceneModel( World, model, Transform.Zero );
			modelObj.Update( 1 );

			PrimarySceneObject = modelObj;

			SceneSize = model.RenderBounds.Size;
			SceneCenter = modelObj.Rotation * SceneCenter;

			var asset = ResourceLibrary.Get<Outfit>( Asset.Path );
			foreach(var clothe in asset.Clothing)
			{
				var mdl = new SceneModel(World, await Model.LoadAsync( clothe.Model ), Transform.Zero);
				mdl.MergeBones( modelObj );
				mdl.Update( 1 );
			}
		}
	}

	public override void UpdateScene( float cycle, float timeStep )
	{
		base.UpdateScene( cycle, timeStep );

		if ( PrimarySceneObject is SceneModel sm )
		{
			sm.Update( timeStep  );
			sm.ColorTint = new( 1, 1, 1, 0 );
			foreach( var clothe in sceneClothes )
			{
				clothe.MergeBones( PrimarySceneObject as SceneModel );
				clothe.Transform = sm.Transform;
				clothe.Update( timeStep );
				
			}
		}
	}
}

