// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

using System;

/// <summary>
/// Attribute used to control whether the local variables in a method are zero-initialized.
/// Applying this attribute to a struct/class/assembly is the same as applying it to every method contained in that type or assembly.
/// Seee <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod.initlocals"/> for more information.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property)]
public class InitLocalsAttribute : Attribute
{
    /// <summary>
    /// Attribute used to control whether the local variables in a method are zero-initialized.
    /// Applying this attribute to a struct/class/assembly is the same as applying it to every method contained in that type or assembly.
    /// Seee <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod.initlocals"/> for more information.
    /// </summary>
    public InitLocalsAttribute(bool initLocals)
    {
    }
}