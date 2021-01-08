// <copyright file="PriorityOrderer.cs" company="Patrick Sachmann">
// Copyright (c) Patrick Sachmann. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace VPEAR.Server.Test
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var attributeName = typeof(PriorityAttribute).AssemblyQualifiedName;
            var argumentName = nameof(PriorityAttribute.Priority);
            var methods = new SortedDictionary<int, List<ITestCase>>();

            foreach (var testCase in testCases)
            {
                var priority = testCase.TestMethod.Method.GetCustomAttributes(attributeName)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(argumentName) ?? 0;

                GetOrCreate(methods, priority)
                    .Add(testCase);
            }

            foreach (TTestCase testCase in methods.Keys.SelectMany(
                priority => methods[priority].OrderBy(
                    testCase => testCase.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new()
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : (dictionary[key] = new TValue());
        }
    }
}
