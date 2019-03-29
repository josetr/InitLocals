// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Fody;
using Mono.Collections.Generic;

public class ModuleWeaver : BaseModuleWeaver
{
    public override void Execute()
    {
        var assemblyInitLocals = GetInitLocals(ModuleDefinition.Assembly.CustomAttributes);

        foreach (var type in ModuleDefinition.Types)
        {
            var typeInitLocals = GetInitLocals(type.CustomAttributes) ?? assemblyInitLocals;

            foreach (var method in type.Methods.Where(m => m.HasBody))
            {
                var methodInitLocals = GetInitLocals(method.CustomAttributes) ?? typeInitLocals;
                method.Body.InitLocals = methodInitLocals.GetValueOrDefault(method.Body.InitLocals);
            }
        }
    }

    private static bool? GetInitLocals(Collection<CustomAttribute> customAttributes)
    {
        var initLocalsAttribute = customAttributes.FirstOrDefault(
            attr => attr.AttributeType.FullName == "InitLocalsAttribute");

        if (initLocalsAttribute == null ||
            initLocalsAttribute.ConstructorArguments.Count != 1)
            return null;

        return (bool)initLocalsAttribute.ConstructorArguments[0].Value;
    }

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        return Enumerable.Empty<string>();
    }
}
