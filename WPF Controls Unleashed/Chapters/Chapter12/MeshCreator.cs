using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Chapters.Chapter12
{
	public static class MeshCreator
	{
		public static MeshGeometry3D CreateMesh(SurfaceType surface, int hPoints, int vPoints)
		{
			MeshGeometry3D mesh = new MeshGeometry3D();
			mesh.Positions = CreatePositions(surface, hPoints, vPoints);
			mesh.TextureCoordinates = CreateTextureCoordinates(hPoints, vPoints);
			mesh.TriangleIndices = CreateTriangleIndices(hPoints, vPoints);

			return mesh;
		}

		public static Point3DCollection CreatePositions(SurfaceType surface, int hPoints, int vPoints)
		{
			VerifyHVPoints(hPoints, vPoints);

			switch (surface)
			{
				case SurfaceType.Plane:
					return CreatePlane(hPoints, vPoints);
				case SurfaceType.Sphere:
					return CreateSphere(hPoints, vPoints);
				case SurfaceType.Cylinder:
					return CreateCylinder(hPoints, vPoints);
				case SurfaceType.Cone:
					return CreateCone(hPoints, vPoints);
			}

			return null;
		}

		public static Int32Collection CreateTriangleIndices(int hPoints, int vPoints)
		{
			VerifyHVPoints(hPoints, vPoints);

			Int32Collection indices = new Int32Collection();
			for (int y = 0; y < vPoints - 1; y++)
			{
				for (int x = 0; x < hPoints - 1; x++)
				{
					int v1 = x + y*hPoints;
					int v2 = v1 + 1;
					int v3 = v1 + hPoints;
					int v4 = v3 + 1;

					indices.Add(v1);
					indices.Add(v3);
					indices.Add(v4);
					indices.Add(v1);
					indices.Add(v4);
					indices.Add(v2);
				}
			}
			return indices;
		}

		public static PointCollection CreateTextureCoordinates(int hPoints, int vPoints)
		{
			PointCollection points = new PointCollection();
			for (int i = 0; i < vPoints; i++)
			{
				double vStep = (double) i/(vPoints - 1);
				for (int j = 0; j < hPoints; j++)
				{
					double hStep = (double) j/(hPoints - 1);

					Point point = new Point(hStep, vStep);
					points.Add(point);
				}
			}

			return points;
		}

		private static void VerifyHVPoints(int hPoints, int vPoints)
		{
			if (hPoints < 2 || vPoints < 2)
			{
				throw new ArgumentException("hPoints and vPoints have to be greater than or equal to 2");
			}
		}

		#region Surfaces

		private static Point3DCollection CreatePlane(int hPoints, int vPoints)
		{
			Point3DCollection points = new Point3DCollection();
			for (int i = 0; i < vPoints; i++)
			{
				double vStep = (double) i/(vPoints - 1);
				double y = 1 - 2*vStep;
				for (int j = 0; j < hPoints; j++)
				{
					double hStep = (double) j/(hPoints - 1);
					double x = -1 + 2*hStep;
					double z = 0;

					Point3D point = new Point3D(x, y, z);
					points.Add(point);
				}
			}

			return points;
		}

		private static Point3DCollection CreateSphere(int hPoints, int vPoints)
		{
			// Generate the vertices
			Point3DCollection points = new Point3DCollection();
			for (int i = 0; i < vPoints; i++)
			{
				double s = (double) i/(vPoints - 1);
				for (int j = 0; j < hPoints; j++)
				{
					double t = (double) j/(hPoints - 1);

					double z = -Math.Cos(t*2*Math.PI)*Math.Sin(s*Math.PI);
					double x = -Math.Sin(t*2*Math.PI)*Math.Sin(s*Math.PI);
					double y = Math.Cos(s*Math.PI);

					points.Add(new Point3D(x, y, z));
				}
			}

			return points;
		}

		private static Point3DCollection CreateCylinder(int hPoints, int vPoints)
		{
			// Generate the vertices
			Point3DCollection points = new Point3DCollection();
			for (int i = 0; i < vPoints; i++)
			{
				double s = (double) i/(vPoints - 1);
				for (int j = 0; j < hPoints; j++)
				{
					double t = (double) j/(hPoints - 1);

					double z = -Math.Cos(t*2*Math.PI);
					double x = -Math.Sin(t*2*Math.PI);
					double y = Math.Cos(s*Math.PI);

					Point3D point = new Point3D(x, y, z);
					points.Add(point);
				}
			}

			return points;
		}

		private static Point3DCollection CreateCone(int hPoints, int vPoints)
		{
			// Generate the vertices
			Point3DCollection points = new Point3DCollection();
			for (int i = 0; i < vPoints; i++)
			{
				double s = (double) i/(vPoints - 1);
				for (int j = 0; j < hPoints; j++)
				{
					double t = (double) j/(hPoints - 1);

					double z = -Math.Cos(t*2*Math.PI)*s;
					double x = -Math.Sin(t*2*Math.PI)*s;
					double y = 1 - 2*s;

					points.Add(new Point3D(x, y, z));
				}
			}

			return points;
		}

		#endregion
	}
}