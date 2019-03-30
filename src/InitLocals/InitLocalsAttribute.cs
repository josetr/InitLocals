// Copyright (c) 2019 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

using System;

/// <summary>
/// Attribute used to control whether the local variables in a method should be zero-initialized.
/// Applying this attribute to a struct/class/module is the same as applying it to every method within that type/module.
/// </summary>
/// <remarks>
/// See <see href="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod.initlocals"/> for more information.
/// </remarks>
[AttributeUsage(AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = false)]
public sealed class InitLocalsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InitLocalsAttribute"/> attribute.
    /// </summary>
    /// <param name="initLocals">Whether the local variables in a method should be zero-initialized.</param>
    /// <remarks>
    /// See <see href="https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.dynamicmethod.initlocals"/> for more information.
    /// </remarks>
    public InitLocalsAttribute(bool initLocals)
    {
    }
}