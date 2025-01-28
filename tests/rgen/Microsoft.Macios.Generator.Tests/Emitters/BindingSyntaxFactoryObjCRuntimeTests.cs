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

	class TestDataGetNSArrayAuxVariableTest : IEnumerable<object []> {
		public IEnumerator<object []> GetEnumerator ()
		{
			// not array 
			
			yield return [
				new Parameter(0, ReturnTypeForInt(isNullable: false), "myParam"), null!, false];

			// not nullable string[]
			yield return [
				new Parameter(0, ReturnTypeForArray ("string", isNullable: false), "myParam"), "var nsa_myParam = NSArray.FromStrings (myParam);", false];
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("string", isNullable: false), "myParam"), "using var nsa_myParam = NSArray.FromStrings (myParam);", true];
			
			// nullable string []
			yield return [
				new Parameter(0, ReturnTypeForArray ("string", isNullable: true), "myParam"), "var nsa_myParam = myParam is null ? null : NSArray.FromStrings (myParam);", false];
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("string", isNullable: true), "myParam"), "using var nsa_myParam = myParam is null ? null : NSArray.FromStrings (myParam);", true];

			// nsstrings
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("NSString", isNullable: false), "myParam"), "var nsa_myParam = NSArray.FromNSObjects (myParam);", false];
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("NSString", isNullable: false), "myParam"), "using var nsa_myParam = NSArray.FromNSObjects (myParam);", true];
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("NSString", isNullable: true), "myParam"), "var nsa_myParam = myParam is null ? null : NSArray.FromNSObjects (myParam);", false];
			
			yield return [
				new Parameter(0, ReturnTypeForArray ("NSString", isNullable: true), "myParam"), "using var nsa_myParam = myParam is null ? null : NSArray.FromNSObjects (myParam);", true];
		}

		IEnumerator IEnumerable.GetEnumerator () => GetEnumerator ();
	}

	[Theory]
	[ClassData(typeof(TestDataGetNSArrayAuxVariableTest))]
	void GetNSArrayAuxVariableTests (in Parameter parameter, string? expectedDeclaration, bool withUsing)
	{
		var declaration = GetNSArrayAuxVariable(in parameter, withUsing: withUsing);
		if (expectedDeclaration is null) {
			Assert.Null (declaration);
		} else {
			Assert.NotNull (declaration);
			Assert.Equal (expectedDeclaration, declaration.ToString ());
		}
	} 
}
