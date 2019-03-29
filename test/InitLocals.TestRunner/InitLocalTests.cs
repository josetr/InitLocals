// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace InitLocals.TestRunner
{
    using System.Linq;
    using Fody;
    using Xunit;

    public class InitLocalTests
    {
        [Fact]
        public void Run()
        {
            var testResult = new ModuleWeaver().ExecuteTestRun("InitLocals.Tests.dll", runPeVerify: false);
            var programType = testResult.Assembly.GetType("InitLocals.Tests.Program");
            var mainMethod = programType.GetMethod("Main");
            mainMethod.Invoke(null, null);
        }
    }
}