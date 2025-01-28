// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using Microsoft.Macios.Generator.DataModel;
using Xunit;
using static Microsoft.Macios.Generator.Emitters.BindingSyntaxFactory;
using static Microsoft.Macios.Generator.Tests.TestDataFactory;

namespace Microsoft.Macios.Generator.Tests.Emitters;

public class BindingSyntaxFactoryObjCRuntimeTests {
	
	class TestDataCodeChangesFromClassDeclaration : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// nsobject type
			yield return [
				new Parameter (0, ReturnTypeForNSObject ("MyNSObject"), "myParam"), 
				"var myParam__handle__ = myParam.GetHandle ();", 
				false
			];
			
			yield return [
				new Parameter (0, ReturnTypeForNSObject ("MyNSObject"), "myParam"), 
				"var myParam__handle__ = myParam!.GetNonNullHandle ( nameof (myParam));", 
				true
			];	
			
			// interface type
			yield return [
				new Parameter (0, ReturnTypeForINativeObject ("MyNativeObject"), "myParam"),
				"var myParam__handle__ = myParam.GetHandle ();", 
				false
			];	
			
			yield return [
				new Parameter (0, ReturnTypeForINativeObject ("MyNativeObject"), "myParam"),
				"var myParam__handle__ = myParam!.GetNonNullHandle ( nameof (myParam));", 
				true
			];	
			
			// value type
			yield return [
				new Parameter (0, ReturnTypeForBool(), "myParam"),
				null!,
				false
			];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData (typeof(TestDataCodeChangesFromClassDeclaration))]
	void GetHandleAuxVariableTests (in Parameter parameter, string? expectedDeclaration, bool withNullAllowed)
	{
		var declaration = GetHandleAuxVariable (parameter, withNullAllowed: withNullAllowed);
		if (expectedDeclaration is null) {
			Assert.Null (declaration);
		} else {
			Assert.NotNull (declaration);
			Assert.Equal (expectedDeclaration, declaration.ToString ());
		}
	} 
}
