﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using LegendOfCube.Engine.CubeMath;

namespace LegendOfCubeTests
{
	[TestClass]
	public class IntersectionTestsTests
	{
		[TestMethod]
		public void TestOBBvsOBB()
		{
			OBB obb1 = new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));
			OBB obb2 = new OBB(new Vector3(1, 1, 1), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));
			OBB obb3 = new OBB(new Vector3(2.2f, 2.2f, 2.2f), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(2, 2, 2));

			Assert.IsTrue(obb1.Intersects(ref obb2));
			Assert.IsTrue(obb2.Intersects(ref obb3));
			Assert.IsFalse(obb1.Intersects(ref obb3));


			OBB obbSkew1 = new OBB(new Vector3(0,2,0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1, 10, 2);
			OBB obbSkew2 = new OBB(new Vector3(0,8,0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1, 1, 2);
			OBB obbSkew3 = new OBB(new Vector3(0,7,0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1, 2, 2);

			Assert.IsFalse(obbSkew1.Intersects(ref obbSkew2));
			Assert.IsTrue(obbSkew3.Intersects(ref obbSkew1));
			Assert.IsTrue(obbSkew3.Intersects(ref obbSkew2));
			Assert.IsTrue(IntersectionsTestsInside(new Vector3(0.0f, 6.9f, 0.0f), ref obbSkew1));
			// TODO: Add moar.
		}

		private bool IntersectionsTestsInside(Vector3 v, ref OBB obb)
		{
			return IntersectionsTests.Inside(ref v, ref obb);
		}

		[TestMethod]
		public void TestOBBTransform()
		{
			OBB IDENTITY = OBB.IDENTITY;

			OBB obb1 = new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1));

			Matrix identity = Matrix.Identity;
			OBB test1 = OBB.TransformOBB(ref obb1, ref identity);
			Assert.IsTrue(approxEqu(ref test1, ref obb1));
			Matrix identityToThis1 = test1.IdentityToThisMatrix();
			OBB transformedIdentity1 = OBB.TransformOBB(ref IDENTITY, ref identityToThis1);
			Trace.WriteLine("obb1: " + obb1);
			Trace.WriteLine("test1: " + test1);
			Trace.WriteLine("transformedIdentity1: " + transformedIdentity1);
			Assert.IsTrue(transformedIdentity1.ApproxEqu(ref test1, 0.1f));

			Matrix scale = Matrix.CreateScale(2.0f);
			OBB test2 = OBB.TransformOBB(ref obb1, ref scale);
			Assert.IsTrue(approxEqu(obb1.Position, test2.Position));
			Assert.IsTrue(approxEqu(obb1.AxisX, test2.AxisX));
			Assert.IsTrue(approxEqu(obb1.AxisY, test2.AxisY));
			Assert.IsTrue(approxEqu(obb1.AxisZ, test2.AxisZ));
			Assert.IsTrue(approxEqu(test2.ExtentX, 2.0f));
			Assert.IsTrue(approxEqu(test2.ExtentY, 2.0f));
			Assert.IsTrue(approxEqu(test2.ExtentZ, 2.0f));
			Matrix identityToThis2 = test2.IdentityToThisMatrix();
			Assert.IsTrue(OBB.TransformOBB(ref IDENTITY, ref identityToThis2).ApproxEqu(ref test2, 0.01f));
			
			Vector3 translPos = new Vector3(2, 4, 1);
			Matrix transl = Matrix.CreateTranslation(translPos);
			OBB test3 = OBB.TransformOBB(ref obb1, ref transl);
			Assert.IsTrue(approxEqu(translPos, test3.Position));
			Assert.IsTrue(approxEqu(obb1.AxisX, test3.AxisX));
			Assert.IsTrue(approxEqu(obb1.AxisY, test3.AxisY));
			Assert.IsTrue(approxEqu(obb1.AxisZ, test3.AxisZ));
			Assert.IsTrue(approxEqu(test3.ExtentX, 1.0f));
			Assert.IsTrue(approxEqu(test3.ExtentY, 1.0f));
			Assert.IsTrue(approxEqu(test3.ExtentZ, 1.0f));
			Matrix identityToThis3 = test3.IdentityToThisMatrix();
			Assert.IsTrue(OBB.TransformOBB(ref IDENTITY, ref identityToThis3).ApproxEqu(ref test3, 0.01f));

			OBB skewedObb = new OBB(new Vector3(0, 0, 0), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1, 10, 1);
			Matrix transl4 = Matrix.CreateScale(2, 3, 1);
			OBB test4 = OBB.TransformOBB(ref skewedObb, ref transl4);
			Assert.IsTrue(approxEqu(test4.Position, new Vector3(0,0,0)));
			Assert.IsTrue(approxEqu(test4.AxisX, skewedObb.AxisX));
			Assert.IsTrue(approxEqu(test4.AxisY, skewedObb.AxisY));
			Assert.IsTrue(approxEqu(test4.AxisZ, skewedObb.AxisZ));
			Assert.IsTrue(approxEqu(test4.ExtentX, 2));
			Assert.IsTrue(approxEqu(test4.ExtentY, 30));
			Assert.IsTrue(approxEqu(test4.ExtentZ, 1));
			Matrix identityToThis4 = test4.IdentityToThisMatrix();
			Assert.IsTrue(OBB.TransformOBB(ref IDENTITY, ref identityToThis4).ApproxEqu(ref test4, 0.01f));
		}

		[TestMethod]
		public void TestTransformFromOBB()
		{
			OBB obb1 = new OBB(new Vector3(0.5f, 0.5f, 0.5f), Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, 1, 1, 1);
			Matrix m1 = Matrix.Identity;
			OBB obb1Post = OBB.TransformOBB(ref obb1, ref m1);
			Matrix m1Recr = OBB.TransformFromOBBs(ref obb1, ref obb1Post);
			Trace.WriteLine("m1 =\n" + MatrixToString(m1) + "\nm1Recr =\n" + MatrixToString(m1Recr));
			Assert.IsTrue(MathUtils.ApproxEqu(m1, m1Recr, 0.01f));

			OBB obb2 = obb1;
			Matrix m2 = Matrix.CreateRotationX(0.5f) * Matrix.CreateRotationY(0.5f);
			OBB obb2Post = OBB.TransformOBB(ref obb2, ref m2);
			Matrix m2Recr = OBB.TransformFromOBBs(ref obb2, ref obb2Post);
			Trace.WriteLine("\nm2 =\n" + MatrixToString(m2) + "\nm2Recr =\n" + MatrixToString(m2Recr));
			Assert.IsTrue(MathUtils.ApproxEqu(m2, m2Recr, 0.01f));
		}

		private String MatrixToString(Matrix m)
		{
			return "{ {" + m.M11 + ", " + m.M12 + ", " + m.M13 + ", " + m.M14 + "}, "
			     + "\n{" + m.M21 + ", " + m.M22 + ", " + m.M23 + ", " + m.M24 + "}, "
			     + "\n{" + m.M31 + ", " + m.M32 + ", " + m.M33 + ", " + m.M34 + "}, "
			     + "\n{" + m.M41 + ", " + m.M42 + ", " + m.M43 + ", " + m.M44 + "} }";
		}

		private const float EPSILON = 0.001f;

		private bool approxEqu(float lhs, float rhs)
		{
			return lhs <= rhs + EPSILON && lhs >= rhs - EPSILON;
		}

		private bool approxEqu(Vector3 lhs, Vector3 rhs)
		{
			if (!approxEqu(lhs.X, rhs.X)) return false;
			if (!approxEqu(lhs.Y, rhs.Y)) return false;
			if (!approxEqu(lhs.Z, rhs.Z)) return false;
			return true;
		}

		private bool approxEqu(ref OBB lhs, ref OBB rhs)
		{
			if (!approxEqu(lhs.Position, rhs.Position)) return false;
			if (!approxEqu(lhs.AxisX, rhs.AxisX)) return false;
			if (!approxEqu(lhs.AxisY, rhs.AxisY)) return false;
			if (!approxEqu(lhs.AxisZ, rhs.AxisZ)) return false;
			if (!approxEqu(lhs.HalfExtents, rhs.HalfExtents)) return false;
			return true;
		}
	}
}
