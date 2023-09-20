using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Trail : MeshInstance3D
{
	/// <summary>
	/// A list of the trail's points.
	/// <para>
	/// This list has the trail's points in the center. Which we use to generate meshes based on startWidth and endWidth.
	/// </para>
	/// </summary>
	List<Vector3> points;

	/// <summary>
	/// A list of time values for each point.
	/// <para>
	/// A list of time values for each point so the point would be removed when the time exceeds its lifetime.
	/// </para>
	/// </summary>
    List<double> lifePoints;

	/// <summary>
	/// Last trail point's position.
	/// <para>
	/// Storing last point's position to compare it with current frame's position to know if it exceeded the motionDelta or not.
	/// </para>
	/// </summary>
	Vector3 oldPos;

	/// <summary>
	/// Trail point's width.
	/// <para>
	/// Has each point's width based on their distances and lifespans.
	/// </para>
	/// </summary>
	List<Vector3[]> widths;

	/// <summary>
	/// Generated mesh.
	/// <para>
	/// The ImmediateMesh we perform all the trail's kung fu on, then we assign it to MeshInstance3D.Mesh.
	/// </para>
	/// </summary>
	ImmediateMesh mesh;

	[ExportGroup("Internal parameters")]
    [Export] public bool trailEnabled = true;
	[Export] public bool scaleTexture = true;
	[Export] public float startWidth = 0.5f;
	[Export] public float endWidth = 0f;
	[Export(PropertyHint.ExpEasing)] public float scaleAcceleration;
	[Export] public float motionDelta = 0.1f;
	[Export] public float lifeSpan = 1f;
	[Export] public Color startColor = Colors.White;
	[Export] public Color endColor = Colors.Transparent;

    public override void _Ready()
    {
		points = new List<Vector3>();
		widths = new List<Vector3[]>();
		lifePoints = new List<double>();
		oldPos = GlobalPosition;
		Mesh = new ImmediateMesh();
		mesh = (ImmediateMesh) Mesh;
    }

	/// <summary>
	/// Adds the current's GlobalPosition to the points lists.
	/// </summary>
    public void AppendPoint()
    {
		points.Add(GlobalPosition);
		widths.Add(new Vector3[] { GlobalTransform.Basis.X * startWidth, GlobalTransform.Basis.X * startWidth - GlobalTransform.Basis.X * endWidth });
		lifePoints.Add(0);
	}

	/// <summary>
	/// Removes the indexed point from the points lists.
	/// </summary>
	/// <param name="i">Point's index.</param>
	public void RemovePoint(int i)
	{
		points.RemoveAt(i);
		widths.RemoveAt(i);
        lifePoints.RemoveAt(i);
	}

	/// <summary>
	/// Clears the points lists and the generated mesh.
	/// </summary>
	public void Clear()
	{
		points.Clear();
		widths.Clear();
		lifePoints.Clear();
		mesh.ClearSurfaces();
		Mesh = mesh;
	}

    public override void _Process(double delta)
    {
		if (!trailEnabled) return;

		if ((oldPos - GlobalPosition).Length() > motionDelta)
		{
			AppendPoint();
			oldPos = GlobalPosition;
		}

		int p = 0;
		int maxPoints = points.Count;
		while (p < maxPoints)
		{
			lifePoints[p] += delta;
			if (lifePoints[p] > lifeSpan)
			{
				RemovePoint(p);
				p--;
				if (p < 0) p = 0;
			}

			maxPoints = points.Count;
			p++;
		}

		mesh.ClearSurfaces();

		if (points.Count < 2) return;

        mesh.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip);

		for (int i = 0; i < points.Count; i++)
		{
			float t = (float) i / (points.Count - 1f);
			Vector3 currentWidth = new Vector3();

			Color currentColor = startColor.Lerp(endColor, 1 - t);
			
			mesh.SurfaceSetColor(currentColor);

            currentWidth = widths[i][0] - Mathf.Pow(1 - t, scaleAcceleration) * widths[i][1];
			float t0;
			float t1;
			if (scaleTexture)
			{
				t0 = motionDelta * i;
				t1 = motionDelta * (i+1);
            }
			else
			{
				t0 = (float)1 / points.Count;
				t1 = t;
			}
            mesh.SurfaceSetUV(new Vector2(t0, 0));
            mesh.SurfaceAddVertex(ToLocal(points[i] + currentWidth));
            mesh.SurfaceSetNormal(Vector3.Up);
            mesh.SurfaceSetUV(new Vector2(t1, 1));
            mesh.SurfaceAddVertex(ToLocal(points[i] - currentWidth));
        }
        mesh.SurfaceEnd();
    }

}
